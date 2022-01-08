using AzureSearchIndxer;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

//Two-stage initialization
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "log.txt",
        fileSizeLimitBytes: 1_000_000,
        rollOnFileSizeLimit: true,
        shared: true,
        flushToDiskInterval: TimeSpan.FromSeconds(1),
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
    .UseSerilog()
    //.UseSerilog((context, services, configuration) => configuration
    //                .ReadFrom.Configuration(context.Configuration)
    //                .ReadFrom.Services(services))
    .ConfigureServices((HostBuilderContext context, IServiceCollection services) =>
    {
        services.AddOptions<AzureBlobOptions>().Bind(context.Configuration.GetSection(AzureBlobOptions.AzureBlob));
        services.AddOptions<SearchIndexerClientOptions>().Bind(context.Configuration.GetSection(SearchIndexerClientOptions.SearchIndexerClient));

        //services.AddDbContextPool<BloggingContext>(
        //    options => options.UseSqlServer(connectionString));

        services.AddAzureClients(builder =>
        {
            builder.AddSearchClient(context.Configuration.GetSection(SearchClientOptions.SearchClient));
            builder.AddSearchIndexClient(context.Configuration.GetSection(SearchIndexClientOptions.SearchIndexClient))
                .ConfigureOptions(options =>
                    {
                        options.Retry.MaxRetries = 10;
                    });
        });
        services.AddScoped<AzurSearchIndexerService>();
        services.AddScoped<AzurSearchService>();
        //services.AddHostedService<AzureSearchIndxer.Host>();
        services.AddScoped<Genearator>();
    });

try
{
    IHost host = builder.Build();

    await host.StartAsync();

    //OR

    int[] targets = Enumerable.Range(0, 5).ToArray();
    await Parallel.ForEachAsync(targets, async (target, token) =>
    {
        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            try
            {
                var genearator = services.GetRequiredService<Genearator>();
                await genearator.Run(target);
                Log.Information("Success");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error Occured");
            }
        }
    });
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

