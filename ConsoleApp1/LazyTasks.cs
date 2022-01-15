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
    private static string data3 = "";
    private static string data4 = "";
    private static string data5 = "";
    private static string data6 = "";

    private Lazy<Task<string>> _lazyStringTask = new(async () =>
    {
        if(data is null)
        {
            await Task.Delay(1000);
            data = "test";
        }

        return data;
    });

    private Lazy<ValueTask<string>> _lazyStringValueTask = new(async () =>
    {
        if (data2 is null)
        {
            await Task.Delay(1000);
            data2 = "test";
        }

        return data2;
    });

    private Lazy<Task<string>> _lazyStringTask2 = new(async () =>
    {
        if (data3 is null)
        {
            await Task.Delay(1000);
            data3 = "test";
        }

        return data3;
    });

    private Lazy<ValueTask<string>> _lazyStringValueTask2 = new(async () =>
    {
        if (data4 is null)
        {
            await Task.Delay(1000);
            data4 = "test";
        }

        return data4;
    });
    //private ValueTask<string> _valueTask()
    //{
    //    //not allowed
    //    if (data5 is null)
    //    {
    //        return new Task(() =>
    //        {

    //        });
    //    }

    //    return data5;
    //}

    private async ValueTask<string> _valueTask()
    {
        //not allowed
        if (data5 is null)
        {
            await Task.Delay(1000);
            data5 = "test";
        }

        return data5;
    }
    private async ValueTask<string> _valueTask2()
    {
        //not allowed
        if (data6 is null)
        {
            await Task.Delay(1000);
            data6 = "test";
        }

        return data6;
    }
    [Benchmark]
    public async Task<string> LazyStringTask() => await _lazyStringTask.Value;

    [Benchmark]
    public async Task<string> LazyStringValueTask() => await _lazyStringValueTask.Value;

    [Benchmark]
    public async ValueTask<string> LazyStringTask2() => await _lazyStringTask2.Value;

    [Benchmark]
    public async ValueTask<string> LazyStringValueTask2() => await _lazyStringValueTask2.Value;

    [Benchmark]
    public async Task<string> ValueTask() => await _valueTask();
    [Benchmark]
    public async ValueTask<string> ValueTask2() => await _valueTask2();
}
