using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using Dasync.Collections;

namespace BenchmarkApp;

[MemoryDiagnoser]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
public class ParallelCalls
{
    private int[]? _numbers;

    [GlobalSetup]
    public void Setup()
    {
        _numbers = Enumerable.Range(0, 1000).ToArray();
    }

    [Benchmark]
    public async Task Test()
    {
        //ParallelOptions option = new()
        //{
        //    MaxDegreeOfParallelism = 5
        //};
        //await Parallel.ForEachAsync(_numbers, option, async (x, token) =>
        //{
        //    await Task.Delay(1, token);
        //});

        Debug.Assert(_numbers != null, nameof(_numbers) + " != null");
        await Parallel.ForEachAsync(_numbers, async (_, token) =>
        {
            await Task.Delay(1, token);
        });
    }

    [Benchmark]
    public async Task Test2()
    {
        await _numbers.ParallelForEachAsync(async _ =>
        {
            CancellationToken cancellationToken = new();
            await Task.Delay(1, cancellationToken);
        }/*, maxDegreeOfParallelism: 5*/);
    }
}
