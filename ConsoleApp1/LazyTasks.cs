using BenchmarkDotNet.Attributes;

namespace ConsoleApp1;

//[ThreadingDiagnoser]
[MemoryDiagnoser]
//[RPlotExporter]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
public class LazyTasks
{
    private static string data;
    private static string data2;
    private int[] numbers;

    public static Lazy<Task<string>> _lazyIntTask = new Lazy<Task<string>>(async () =>
    {
        if(data is null)
        {
            await Task.Delay(1000);
            data = "test";
        }

        return data;
    });

    public static Lazy<ValueTask<string>> _lazyIntValueTask = new Lazy<ValueTask<string>>(async () =>
    {
        if (data is null)
        {
            await Task.Delay(1000);
            data = "test";
        }

        return data;
    });

    public async Task LazyIntTask()
    {
        await _lazyIntTask.Value;
    }

    public async Task LazyIntValueTask()
    {
        await _lazyIntValueTask.Value;
    }

    [GlobalSetup]
    public void Setup()
    {
        numbers = Enumerable.Range(0, 16).ToArray();
    }


    [Benchmark]
    public void Run()
    {
        Parallel.ForEachAsync(numbers, async (x, token) =>
        {
            await Task.Delay(100,token);
        });
    }

    //[Benchmark]
    //public async ValueTask<string> ValueTasks()
    //{

    //    if (data is null)
    //    {
    //        await Task.Delay(1000);
    //        data = "test";
    //    }

    //    return data;
    //}


    //[Benchmark]
    //public async Task<string> Tasks()
    //{

    //    if (data2 is null)
    //    {
    //        await Task.Delay(1000);
    //        data2 = "test";
    //    }

    //    return data2;
    //}
}
