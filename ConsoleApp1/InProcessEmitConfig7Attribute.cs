using BenchmarkDotNet.Attributes;

namespace BenchmarkApp;

public class InProcessEmitConfig7Attribute : ConfigAttribute
{
    public InProcessEmitConfig7Attribute() : base(typeof(InProcessEmit7Config))
    {
    }
}
