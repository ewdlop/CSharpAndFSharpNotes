using BenchmarkDotNet.Attributes;

namespace BenchmarkApp;

[MemoryDiagnoser]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
public class CollectionInit
{
    [Benchmark]
    public void TestListNotSized()
    {
        var list = new List<int>() { 1, 2, 3 };
    }
    
    [Benchmark]
    public void TestListSized()
    {
        var list = new List<int>(3) { 1, 2, 3 };
    }
    
    [Benchmark]
    public void TesArrayNotSized()
    {
        var list = new int[] { 1, 2, 3 };
    }

    [Benchmark]
    public void TestArraySized()
    {
        var list = new int[3] { 1, 2, 3 };
    }
}