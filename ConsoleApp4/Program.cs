// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

CustomTaskScheduler customTaskScheduler = new();
CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
TaskCreationOptions creationOptions =
    TaskCreationOptions.PreferFairness | TaskCreationOptions.LongRunning | TaskCreationOptions.AttachedToParent
   | TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler | TaskCreationOptions.RunContinuationsAsynchronously;

Task.Factory.StartNew(async () =>
{
    await Task.Delay(5000, cancellationTokenSource.Token);
    Console.WriteLine("Hello, World!");
    
}, cancellationTokenSource.Token, creationOptions, customTaskScheduler);


//https://devblogs.microsoft.com/oldnewthing/20120120-00/?p=8493
//https://devblogs.microsoft.com/oldnewthing/20200819-00/?p=104093
//https://stackoverflow.com/questions/3033771/file-i-o-with-streams-best-memory-buffer-size
await using FileStream appendStream = new FileStream("sad", new FileStreamOptions()
{
    Access = FileAccess.Write | FileAccess.Read | FileAccess.ReadWrite,
    Mode = FileMode.Append | FileMode.CreateNew | FileMode.Open | FileMode.OpenOrCreate | FileMode.Truncate | FileMode.Append,
    Options = FileOptions.Asynchronous | FileOptions.WriteThrough | FileOptions.DeleteOnClose | FileOptions.SequentialScan,
    BufferSize = 4096,
    Share = FileShare.None | FileShare.Read | FileShare.Write | FileShare.ReadWrite | FileShare.Delete | FileShare.Inheritable,
    PreallocationSize = 0
});

public class CustomTaskScheduler : TaskScheduler
{

    protected override IEnumerable<Task>? GetScheduledTasks()
    {
        throw new NotImplementedException();
    }

    protected override void QueueTask(Task task)
    {
        throw new NotImplementedException();
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        throw new NotImplementedException();
    }
}