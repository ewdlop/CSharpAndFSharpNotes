using AzureSearchIndxer;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args);
builder.ConfigureServices((HostBuilderContext context, IServiceCollection services) =>
{
    services.AddOptions<AzureBlobOptions>().Bind(context.Configuration.GetSection(AzureBlobOptions.AzureBlob));
    services.AddOptions<AzureBlobOptions>().Bind(context.Configuration.GetSection(SearchIndexerClientOptions.SearchIndexerClient));
    
    services.AddAzureClients(builder =>
    {
        builder.AddSearchClient(context.Configuration.GetSection(SearchClientOptions.SearchClient));
        builder.AddSearchIndexClient(context.Configuration.GetSection(SearchIndexClientOptions.SearchIndexClient))
            .ConfigureOptions<Azure.Search.Documents.Indexes.SearchIndexClient, 
                Azure.Search.Documents.SearchClientOptions>(options => {
                    options.Retry.MaxRetries = 10;
                });
    });
    services.AddScoped<AzurSearchIndexerService>();
    services.AddScoped<AzurSearchService>();
    services.AddHostedService<AzureSearchIndxer.Host>();
});

