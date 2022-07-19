using BenchmarkDotNet.Attributes;

namespace BenchmarkApp;

[MemoryDiagnoser]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net60)]
public class Collection
{
    [Benchmark]
    public void TestListNotSized()
    {
        var list = new List<int>();
        for (int i = 0; i < 1000; i++)
        {
            list.Add(i);
        }
    }
    [Benchmark]
    public void TestListSized()
    {
        var list = new List<int>(1000);
        for (int i = 0; i < 1000; i++)
        {
            list.Add(i);
        }
    }
    
    [Benchmark]
    public void TestArray()
    {
        var list = new int[1000];
        for (int i = 0; i < 1000; i++)
        {
            list[i] = i;
        }
    }
}
