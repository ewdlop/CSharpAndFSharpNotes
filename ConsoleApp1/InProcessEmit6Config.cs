using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;

namespace BenchmarkApp;

public class InProcessEmit6Config : ManualConfig
{
    public InProcessEmit6Config()
    {
        AddJob(Job.Default
            .WithToolchain(InProcessNoEmitToolchain.Instance)
            .WithRuntime(CoreRuntime.Core60));
    }
}
