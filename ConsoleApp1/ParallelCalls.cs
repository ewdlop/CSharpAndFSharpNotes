using BenchmarkDotNet.Attributes;
using Dasync.Collections;

namespace ConsoleApp1;

[MemoryDiagnoser]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
public class ParallelCalls
{
    private int[] numbers;

    [GlobalSetup]
    public void Setup()
    {
        numbers = Enumerable.Range(0, 1000).ToArray();
    }

    [Benchmark]
    public async Task Test()
    {
        //ParallelOptions option = new()
        //{
        //    MaxDegreeOfParallelism = 5
        //};
        //await Parallel.ForEachAsync(numbers, option, async (x, token) =>
        //{
        //    await Task.Delay(1, token);
        //});

        await Parallel.ForEachAsync(numbers, async (x, token) =>
        {
            await Task.Delay(1, token);
        });
    }

    [Benchmark]
    public async Task Test2()
    {
        await numbers.ParallelForEachAsync(async (x) =>
        {
            CancellationToken cancellationToken = new CancellationToken();
            await Task.Delay(1, cancellationToken);
        }/*, maxDegreeOfParallelism: 5*/);
    }
}
