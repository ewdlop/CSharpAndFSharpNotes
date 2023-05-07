using BenchmarkApp;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

//BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<LazyTasks>();
//BenchmarkRunner.Run<ReadOnlyListRecordClass>(ManualConfig.CreateMinimumViable()
//    .AddJob(Job.Default
//        .WithToolchain(InProcessEmitToolchain.Instance)
//        .WithRuntime(CoreRuntime.Core60)));

BenchmarkRunner.Run<ReadOnlyListRecordClass>();