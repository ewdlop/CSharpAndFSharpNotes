using BenchmarkDotNet.Attributes;

namespace ConsoleApp1;

//[ThreadingDiagnoser]

//[RPlotExporter]
[MemoryDiagnoser]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
public class LazyTasks
{
    private static string data = "";
    private static string data2 = "";
    private int[] numbers;

    public static Lazy<Task<string>> _lazyStringTask = new Lazy<Task<string>>(async () =>
    {
        if(data2 is null)
        {
            await Task.Delay(1000);
            data2 = "test";
        }

        return data2;
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

    [Benchmark]
    public async Task<string> LazyStringTask() => await _lazyStringTask.Value;

    [Benchmark]
    public async ValueTask<string> ValueTask()
    {

        if (data is null)
        {
            await Task.Delay(1000);
            data = "test";
        }

         return data;
     }


    //[GlobalSetup]
    //public void Setup()
    //{
    //    numbers = Enumerable.Range(0, 16).ToArray();
    //}


    //[Benchmark]
    //public void Run()
    //{
    //    Parallel.ForEachAsync(numbers, async (x, token) =>
    //    {
    //        await Task.Delay(100,token);
    //    });
    //}

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
