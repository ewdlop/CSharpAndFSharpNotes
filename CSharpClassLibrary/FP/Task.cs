using CSharpClassLibrary.DataFlow;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading.Tasks.Dataflow;

namespace CSharpClassLibrary.FP;

public static class TaskExtension
{
    public static Task<R> Map<T, R>(this Task<T> task, Func<T, R> map) =>
        task.ContinueWith(t => map(t.Result));

    public static Task ForEachAsync<T>(this IEnumerable<T> source,
        int maxDegreeOfParallelism, Func<T, Task> body)
    {
        return Task.WhenAll(Partitioner.Create(source).
            GetPartitions(maxDegreeOfParallelism)
            .Select(partition => Task.Run(async () =>
            {
                using (partition)
                    while (partition.MoveNext())
                        await body(partition.Current);
            })));
    }

    public static Task ForEachAsync2<T>(this IEnumerable<T> source,
        int maxDegreeOfParallelism, Func<T, Task> body)
        {
            return Task.WhenAll(Partitioner.Create(source).
                GetPartitions(maxDegreeOfParallelism)
                .Select(partition => Task.Run(async () =>
                {
                    using (partition)
                        while (partition.MoveNext())
                            await body(partition.Current);
                })));
        }

    public static Task<R> Apply<T, R>(this Task<Func<T, R>> liftedFn, Task<T> task)
    {
        var tcs = new TaskCompletionSource<R>();
        liftedFn.ContinueWith(innerLiftTask =>
            task.ContinueWith(innerTask =>
                tcs.SetResult(innerLiftTask.Result(innerTask.Result))
        ));
        return tcs.Task;
    }

    public static Task<R> Apply2<T, R>(this Task<Func<T, R>> liftedFn, Task<T> task)
    {
        return liftedFn.ContinueWith(innerLiftTask =>
                task.ContinueWith(innerTask => innerLiftTask
                    .Result(innerTask.Result))).Result;
    }

    public static async Task<R> Apply3<T, R>(
        this Task<Func<T, R>> f, Task<T> arg)
        => (await f.ConfigureAwait(false))(await arg.ConfigureAwait(false));

    public static Task<Func<b, c>> Apply<a, b, c>(
        this Task<Func<a, b, c>> liftedFn,
        Task<a> input) =>
            Apply(liftedFn.Map(x=>x.Curry()), input);

    public static async Task<R> ForkJoinAsync<T1, T2, R>(
        this IEnumerable<T1> source,
        Func<T1, Task<IEnumerable<T2>>> map,
        Func<R, T2, Task<R>> aggregate,
        R initialState,
        CancellationTokenSource? cts = default,
        int partitionLevel = 8,
        int boundCapacity = 20)
    {
        cts = cts ?? new CancellationTokenSource();
        var blockOptions = new ExecutionDataflowBlockOptions
        {
            CancellationToken = cts.Token,
            MaxDegreeOfParallelism = partitionLevel,
            BoundedCapacity = boundCapacity
        };

        var inputBuffer = new BufferBlock<T1>(new DataflowBlockOptions
        {
            CancellationToken = cts.Token,
            BoundedCapacity = boundCapacity
        });

        var mapperBlock = new TransformManyBlock<T1, T2>(map, blockOptions);
        var reducerAgent = StatefulDataFlowAgent<R, T2>.Start(initialState, aggregate, cts);
        var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
        inputBuffer.LinkTo(mapperBlock, linkOptions);
        IDisposable disposable = mapperBlock.AsObservable().Subscribe(async item => await reducerAgent.SendAsync(item));
        foreach (var item in source)
        {
            await inputBuffer.SendAsync(item);
        }
        inputBuffer.Complete();
        var tcs = new TaskCompletionSource<R>();
        await inputBuffer.Completion.ContinueWith(task => mapperBlock.Completion);
        await mapperBlock.Completion.ContinueWith(task => {
            disposable.Dispose();
            tcs.SetResult(reducerAgent.State);
        });
        return await tcs.Task;
    }
}

