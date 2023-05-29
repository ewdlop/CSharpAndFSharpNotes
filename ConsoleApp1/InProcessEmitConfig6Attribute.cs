using BenchmarkDotNet.Attributes;

namespace BenchmarkApp;

public class InProcessEmitConfig6Attribute : ConfigAttribute
{
    public InProcessEmitConfig6Attribute() : base(typeof(InProcessEmit6Config))
    {
    }
}
