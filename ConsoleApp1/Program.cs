using ConsoleApp1;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;

//BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<LazyTasks>();
BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<ParallelCalls>();

////Two-stage initialization
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
//    .Enrich.FromLogContext()
//    .WriteTo.Console()
//    .WriteTo.File(
//        "log.txt",
//        fileSizeLimitBytes: 1_000_000,
//        rollOnFileSizeLimit: true,
//        shared: true,
//        flushToDiskInterval: TimeSpan.FromSeconds(1),
//        rollingInterval: RollingInterval.Day)
//    .CreateLogger();

//var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
//    .UseSerilog()
//    //.UseSerilog((context, services, configuration) => configuration
//    //                .ReadFrom.Configuration(context.Configuration)
//    //                .ReadFrom.Services(services))
//    .ConfigureServices((HostBuilderContext context, IServiceCollection services) =>
//    {
//    });

//try
//{
//    //await Parallel.ForEachAsync(Enumerable.Range(0, 2), async (target, token) =>
//    //{
//    //    int x = await LazyTasks._lazyInt.Value;
//    //    Log.Information(x.ToString());
//    //});
//}
//catch (Exception ex)
//{
//    Log.Fatal(ex, "Application start-up failed");
//}
//finally
//{
//    Log.CloseAndFlush();
//}