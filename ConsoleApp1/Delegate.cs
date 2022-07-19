using BenchmarkDotNet.Attributes;

namespace BenchmarkApp;

[MemoryDiagnoser]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
public class DelegateClass
{
    public int Func() => 1;
    [Benchmark]
    public void DelegateTest1()
    {
        Func<int> func = () =>
        {
            return 1;
        };
        func();
    }

    [Benchmark]
    public void DelegateTest2()
    {
        Func();
    }
}
