using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace BenchmarkApp;

public class InProcessEmit7Config : ManualConfig
{
    public InProcessEmit7Config()
    {
        AddJob(Job.Default
            .WithToolchain(InProcessEmitToolchain.Instance)
            .WithRuntime(CoreRuntime.Core70));
    }
}
