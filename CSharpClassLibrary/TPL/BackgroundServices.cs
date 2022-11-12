using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CSharpClassLibrary.DataFlow;

public class BackgroundService2<T>
{
    private readonly BufferBlock<T> _buffer = new BufferBlock<T>(
        new DataflowBlockOptions { 
            BoundedCapacity = 10
        });

    private async Task Produce(IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            await _buffer.SendAsync(value);
        }
    }

    private async Task MultipleProducers(params IEnumerable<T>[] producers)
    {
        await Task.WhenAll(producers.Select(value => Produce(value)).ToArray())
        .ContinueWith(_ => _buffer.Complete());
    }

    public async Task Consumer(Action<T> process)
    {
        while (await _buffer.OutputAvailableAsync())
            process(await _buffer.ReceiveAsync());
    }

    public async Task Run(IEnumerable<T> range)
    {
        await Task.WhenAll(MultipleProducers(range, range, range),
        Consumer(n => Console.WriteLine($"value {n} - ThreadId{Environment.CurrentManagedThreadId}")));
    }
}     

public class BackgroundService<T>
{
    //private readonly BlockingCollection<T> _queue = new BlockingCollection<T>();
    private readonly BufferBlock<T> _queue = new BufferBlock<T>();
    private int _sleep = 1000;
    public void AddItem(T item)
    {
        _queue.Post(item);
    }

    // Call this when the app starts
    public void Start()
    {
        Task.Factory.StartNew(async () =>
        {
            while (await _queue.OutputAvailableAsync())
            {
                var item = await _queue.ReceiveAsync();
                await ProcessItem(item);
            }
        }, TaskCreationOptions.LongRunning);
    }


    // Call this when the app starts
    public void Start2()
    {
        Task.Run(async () =>
        {
            while (await _queue.OutputAvailableAsync())
            {
                var item = await _queue.ReceiveAsync();
                await ProcessItem(item);
            }
        });
    }


    // Call this when the app stops
    public Task Stop()
    {
        _queue.Complete();
        return _queue.Completion;
    }

    private async Task ProcessItem(T item)
    {
        await Task.Delay(_sleep);
        Console.WriteLine($"Processed item {item}");
    }
    
    //private void ProcessItem(int item)
    //{
    //    // Do some processing
    //    Thread.Sleep(item);
    //}

    //public void Dispose()
    //{
    //    _queue.Dispose();
    //}
}
