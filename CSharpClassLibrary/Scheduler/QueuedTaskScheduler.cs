﻿using System.Collections.Concurrent;
using System.Diagnostics;

namespace CSharpClassLibrary.Scheduler;

/// <summary>
/// MS's QueuedTaskScheduler with a few modifications
/// without the custom threadpool
/// using ConcurrentQueue
/// Provides a TaskScheduler that provides control over priorities, fairness, and the underlying threads utilized.
/// </summary>
[DebuggerTypeProxy(typeof(QueuedTaskSchedulerDebugView))]
[DebuggerDisplay("Id={Id}, Queues={DebugQueueCount}, ScheduledTasks = {DebugTaskCount}")]
public sealed class QueuedTaskScheduler : TaskScheduler, IDisposable
{
    /// <summary>Debug view for the QueuedTaskScheduler.</summary>
    private class QueuedTaskSchedulerDebugView
    {
        /// <summary>The scheduler.</summary>
        private QueuedTaskScheduler _scheduler;

        /// <summary>Initializes the debug view.</summary>
        /// <param name="scheduler">The scheduler.</param>
        public QueuedTaskSchedulerDebugView(QueuedTaskScheduler scheduler)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }

        /// <summary>Gets all of the Tasks queued to the scheduler directly.</summary>
        public IEnumerable<Task?> ScheduledTasks
        {
            get
            {
                return _scheduler.taskQueue.Where(t => t != null).ToList();
            }
        }

        /// <summary>Gets the prioritized and fair queues.</summary>
        public IEnumerable<TaskScheduler> Queues
        {
            get
            {
                List<TaskScheduler> queues = new List<TaskScheduler>();
                foreach (var group in _scheduler._queueGroups) queues.AddRange(group.Value.Cast<TaskScheduler>());
                return queues;
            }
        }
    }

    /// <summary>
    /// A sorted list of round-robin queue lists.  Tasks with the smallest priority value
    /// are preferred.  Priority groups are round-robin'd through in order of priority.
    /// </summary>
    private readonly SortedList<int, PriortiedTaskSchedulerGroup> _queueGroups = new();
    /// <summary>Cancellation token used for disposal.</summary>
    private readonly CancellationTokenSource _disposeCancellation = new();
    /// <summary>
    /// The maximum allowed concurrency level of this scheduler.  If custom threads are
    /// used, this represents the number of created threads.
    /// </summary>
    private readonly int _concurrencyLevel;
    /// <summary>Whether we're processing tasks on the current thread.</summary>
    private static readonly ThreadLocal<bool> _taskProcessingThread = new();

    // ***
    // *** For when using a target scheduler
    // ***

    /// <summary>The scheduler onto which actual work is scheduled.</summary>
    private readonly TaskScheduler _targetScheduler;
    /// <summary>The queue of tasks to process when using an underlying target scheduler.</summary>
    private readonly ConcurrentQueue<Task?> taskQueue;
    /// <summary>The number of Tasks that have been queued or that are running whiel using an underlying scheduler.</summary>
    private int _delegatesQueuedOrRunning = 0;


    /// <summary>Initializes the scheduler.</summary>
    public QueuedTaskScheduler() : this(Default, 0) { }

    /// <summary>Initializes the scheduler.</summary>
    /// <param name="targetScheduler">The target underlying scheduler onto which this sceduler's work is queued.</param>
    public QueuedTaskScheduler(TaskScheduler targetScheduler) : this(targetScheduler, 0) { }

    /// <summary>Initializes the scheduler.</summary>
    /// <param name="targetScheduler">The target underlying scheduler onto which this sceduler's work is queued.</param>
    /// <param name="maxConcurrencyLevel">The maximum degree of concurrency allowed for this scheduler's work.</param>
    public QueuedTaskScheduler(
        TaskScheduler targetScheduler,
        int maxConcurrencyLevel)
    {
        if (maxConcurrencyLevel < 0) throw new ArgumentOutOfRangeException(nameof(maxConcurrencyLevel));

        // Initialize only those fields relevant to use an underlying scheduler.  We don't
        // initialize the fields relevant to using our own custom threads.
        _targetScheduler = targetScheduler ?? throw new ArgumentNullException(nameof(targetScheduler));
        taskQueue = new ConcurrentQueue<Task?>();

        // If 0, use the number of logical processors.  But make sure whatever value we pick
        // is not greater than the degree of parallelism allowed by the underlying scheduler.
        _concurrencyLevel = maxConcurrencyLevel != 0 ? maxConcurrencyLevel : Environment.ProcessorCount;
        if (targetScheduler.MaximumConcurrencyLevel > 0 &&
            targetScheduler.MaximumConcurrencyLevel < _concurrencyLevel)
        {
            _concurrencyLevel = targetScheduler.MaximumConcurrencyLevel;
        }
    }

    /// <summary>Gets the number of queues currently activated.</summary>
    private int DebugQueueCount
    {
        get
        {
            int count = 0;
            foreach (var group in _queueGroups) count += group.Value.Count;
            return count;
        }
    }

    /// <summary>Gets the number of tasks currently scheduled.</summary>
    private int DebugTaskCount => taskQueue.Where(t => t != null).Count();

    /// <summary>Find the next task that should be executed, based on priorities and fairness and the like.</summary>
    /// <param name="targetTask">The found task, or null if none was found.</param>
    /// <param name="scheduler">
    /// The scheduler associated with the found task.  Due to security checks inside of TPL,  
    /// this scheduler needs to be used to execute that task.
    /// </param>
    private (Task? targetTask, PriortizedTaskScheduler? scheduler) FindNextTask()
    {
        // Look through each of our queue groups in sorted order.
        // This ordering is based on the priority of the queues.
        lock(_queueGroups)
        {
            foreach (var queueGroup in _queueGroups)
            {
                var queues = queueGroup.Value;

                // Within each group, iterate through the queues in a round-robin
                // fashion.  Every time we iterate again and successfully find a task, 
                // we'll start in the next location in the group.

                foreach (int i in queues.CreateSearchOrder())
                {
                    var items = queues[i]._workItems;
                    if (!items.IsEmpty && items.TryDequeue(out Task? task))
                    {
                        if (queues[i]._disposed && items.IsEmpty)
                        {
                            RemoveQueue(queues[i]);
                        }
                        queues.NextQueueIndex = (queues.NextQueueIndex + 1) % queueGroup.Value.Count;
                        return (task, queues[i]);
                    }
                }
            }
        }
        return (null, null);
    }

    /// <summary>Queues a task to the scheduler.</summary>
    /// <param name="task">The task to be queued.</param>
    protected override void QueueTask(Task? task)
    {
        // If we've been disposed, no one should be queueing
        if (_disposeCancellation.IsCancellationRequested) throw new ObjectDisposedException(GetType().Name);

        // Queue the task and check whether we should launch a processing
        // task (noting it if we do, so that other threads don't result
        // in queueing up too many).
        taskQueue.Enqueue(task);
        if (_delegatesQueuedOrRunning < _concurrencyLevel)
        {
            ++_delegatesQueuedOrRunning;
            Task.Factory.StartNew(ProcessPrioritizedAndBatchedTasks,
                CancellationToken.None, TaskCreationOptions.None, _targetScheduler);
        }
    }

    /// <summary>
    /// Process tasks one at a time in the best order.  
    /// This should be run in a Task generated by QueueTask.
    /// It's been separated out into its own method to show up better in Parallel Tasks.
    /// </summary>
    private void ProcessPrioritizedAndBatchedTasks()
    {
        bool continueProcessing = true;
        while (!_disposeCancellation.IsCancellationRequested && continueProcessing)
        {
            try
            {
                // Note that we're processing tasks on this thread
                _taskProcessingThread.Value = true;

                // Until there are no more tasks to process
                while (!_disposeCancellation.IsCancellationRequested)
                {
                    // Try to get the next task.  If there aren't any more, we're done.
                    if (taskQueue.IsEmpty) break;
                    if (taskQueue.TryDequeue(out Task? result))
                    {
                        if (result == null)
                        {
                            // If the task is null, it's a placeholder for a task in the round-robin queues.
                            // Find the next one that should be processed.
                            (result, PriortizedTaskScheduler? queueForTargetTask) = FindNextTask();

                            // Now if we finally have a task, run it.  If the task
                            // was associated with one of the round-robin schedulers, we need to use it
                            // as a thunk to execute its task.
                            if (result != null)
                            {
                                if (queueForTargetTask != null) queueForTargetTask.ExecuteTask(result);
                                else TryExecuteTask(result);
                            }
                        }
                    }
                }
            }
            finally
            {
                // Now that we think we're done, verify that there really is
                // no more work to do.  If there's not, highlight
                // that we're now less parallel than we were a moment ago.
                if (taskQueue.IsEmpty)
                {
                    _delegatesQueuedOrRunning--;
                    continueProcessing = false;
                    _taskProcessingThread.Value = false;
                }
            }
        }
    }

    /// <summary>Notifies the pool that there's a new item to be executed in one of the round-robin queues.</summary>
    private void NotifyNewWorkItem() => QueueTask(null);

    /// <summary>Tries to execute a task synchronously on the current thread.</summary>
    /// <param name="task">The task to execute.</param>
    /// <param name="taskWasPreviouslyQueued">Whether the task was previously queued.</param>
    /// <returns>true if the task was executed; otherwise, false.</returns>
    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        // If we're already running tasks on this threads, enable inlining
        return _taskProcessingThread.Value && TryExecuteTask(task);
    }

    /// <summary>Gets the tasks scheduled to this scheduler.</summary>
    /// <returns>An enumerable of all tasks queued to this scheduler.</returns>
    /// <remarks>This does not include the tasks on sub-schedulers.  Those will be retrieved by the debugger separately.</remarks>
    protected override IEnumerable<Task> GetScheduledTasks() => taskQueue.Where(t => t != null).ToList();
    /// <summary>Gets the maximum concurrency level to use when processing tasks.</summary>
    public override int MaximumConcurrencyLevel => _concurrencyLevel;

    /// <summary>Initiates shutdown of the scheduler.</summary>
    public void Dispose() => _disposeCancellation.Cancel();

    /// <summary>Creates and activates a new scheduling queue for this scheduler.</summary>
    /// <returns>The newly created and activated queue at priority 0.</returns>
    public TaskScheduler ActivateNewQueue() => ActivateNewQueue(0);

    /// <summary>Creates and activates a new scheduling queue for this scheduler.</summary>
    /// <param name="priority">The priority level for the new queue.</param>
    /// <returns>The newly created and activated queue at the specified priority.</returns>
    public TaskScheduler ActivateNewQueue(int priority)
    {
        // Create the queue
        var scheduler = new PriortizedTaskScheduler(priority, this);

        // Add the queue to the appropriate queue group based on priority
        lock (_queueGroups)
        {
            if (!_queueGroups.TryGetValue(priority, out PriortiedTaskSchedulerGroup? list))
            {
                list = new PriortiedTaskSchedulerGroup();
                _queueGroups.Add(priority, list);
            }
            list.Add(scheduler);
        }

        // Hand the new queue back
        return scheduler;
    }

    /// <summary>Removes a scheduler from the group.</summary>
    /// <param name="queue">The scheduler to be removed.</param>
    private void RemoveQueue(PriortizedTaskScheduler queue)
    {
        // Find the group that contains the queue and the queue's index within the group
        var queueGroup = _queueGroups[queue._priority];
        lock(queueGroup)
        {
            int index = queueGroup.IndexOf(queue);
            // We're about to remove the queue, so adjust the index of the next
            // round-robin starting location if it'll be affected by the removal
            if (queueGroup.NextQueueIndex >= index) queueGroup.NextQueueIndex--;
            queueGroup.RemoveAt(index);
        }
    }

    /// <summary>A group of queues a the same priority level.</summary>
    private class PriortiedTaskSchedulerGroup : List<PriortizedTaskScheduler>
    {
        /// <summary>The starting index for the next round-robin traversal.</summary>
        public int NextQueueIndex = 0;

        /// <summary>Creates a search order through this group.</summary>
        /// <returns>An enumerable of indices for this group.</returns>
        public IEnumerable<int> CreateSearchOrder()
        {
            for (int i = NextQueueIndex; i < Count; i++) yield return i;
            for (int i = 0; i < NextQueueIndex; i++) yield return i;
        }
    }

    /// <summary>Provides a scheduling queue associatd with a QueuedTaskScheduler.</summary>
    [DebuggerDisplay("QueuePriority = {_priority}, WaitingTasks = {WaitingTasks}")]
    [DebuggerTypeProxy(typeof(QueuedTaskSchedulerQueueDebugView))]
    private sealed class PriortizedTaskScheduler : TaskScheduler, IDisposable
    {
        /// <summary>A debug view for the queue.</summary>
        private sealed class QueuedTaskSchedulerQueueDebugView
        {
            /// <summary>The queue.</summary>
            private readonly PriortizedTaskScheduler _queue;

            /// <summary>Initializes the debug view.</summary>
            /// <param name="queue">The queue to be debugged.</param>
            public QueuedTaskSchedulerQueueDebugView(PriortizedTaskScheduler queue)
            {
                _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            }

            /// <summary>Gets the priority of this queue in its associated scheduler.</summary>
            public int Priority => _queue._priority;
            /// <summary>Gets the ID of this scheduler.</summary>
            public int Id => _queue.Id;
            /// <summary>Gets all of the tasks scheduled to this queue.</summary>
            public IEnumerable<Task> ScheduledTasks => _queue.GetScheduledTasks();
            /// <summary>Gets the QueuedTaskScheduler with which this queue is associated.</summary>
            public QueuedTaskScheduler AssociatedScheduler => _queue._pool;
        }

        /// <summary>The scheduler with which this pool is associated.</summary>
        private readonly QueuedTaskScheduler _pool;
        /// <summary>The work items stored in this queue.</summary>
        internal readonly ConcurrentQueue<Task> _workItems;
        /// <summary>Whether this queue has been disposed.</summary>
        internal bool _disposed;
        /// <summary>Gets the priority for this queue.</summary>
        internal int _priority;

        /// <summary>Initializes the queue.</summary>
        /// <param name="priority">The priority associated with this queue.</param>
        /// <param name="pool">The scheduler with which this queue is associated.</param>
        internal PriortizedTaskScheduler(int priority, QueuedTaskScheduler pool)
        {
            _priority = priority;
            _pool = pool;
            _workItems = new ConcurrentQueue<Task>();
        }

        /// <summary>Gets the number of tasks waiting in this scheduler.</summary>
        internal int WaitingTasks => _workItems.Count;

        /// <summary>Gets the tasks scheduled to this scheduler.</summary>
        /// <returns>An enumerable of all tasks queued to this scheduler.</returns>
        protected override IEnumerable<Task> GetScheduledTasks() => _workItems.ToList();

        /// <summary>Queues a task to the scheduler.</summary>
        /// <param name="task">The task to be queued.</param>
        protected override void QueueTask(Task task)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().Name);

            // Queue up the task locally to this queue, and then notify
            // the parent scheduler that there's work available
            _workItems.Enqueue(task);
            _pool.NotifyNewWorkItem();
        }

        /// <summary>Tries to execute a task synchronously on the current thread.</summary>
        /// <param name="task">The task to execute.</param>
        /// <param name="taskWasPreviouslyQueued">Whether the task was previously queued.</param>
        /// <returns>true if the task was executed; otherwise, false.</returns>
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If we're using our own threads and if this is being called from one of them,
            // or if we're currently processing another task on this thread, try running it inline.
            return _taskProcessingThread.Value && TryExecuteTask(task);
        }

        /// <summary>Runs the specified ask.</summary>
        /// <param name="task">The task to execute.</param>
        internal void ExecuteTask(Task task) => TryExecuteTask(task);

        /// <summary>Gets the maximum concurrency level to use when processing tasks.</summary>
        public override int MaximumConcurrencyLevel => _pool.MaximumConcurrencyLevel;

        /// <summary>Signals that the queue should be removed from the scheduler as soon as the queue is empty.</summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                // We only remove the queue if it's empty.  If it's not empty,
                // we still mark it as disposed, and the associated QueuedTaskScheduler
                // will remove the queue when its count hits 0 and its _disposed is true.
                if (_workItems.IsEmpty)
                {
                    _pool.RemoveQueue(this);
                }
                _disposed = true;
            }
        }
    }
}