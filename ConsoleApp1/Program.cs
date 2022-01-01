using Azure;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((context, services) =>
{
    //services.Configure<MyApplicationOptions>(
    //      context.Configuration.GetSection(nameof(MyApplicationOptions)));

    services.AddAzureClients(builder =>
    {

        builder.AddSearchIndexClient(context.Configuration.GetSection("SearchDocument"))
            .ConfigureOptions(options => options.Retry.MaxRetries = 10) ;
        builder.AddSearchClient(new Uri(""), "", new AzureKeyCredential(""));
        //builder.AddClient<SecretClient, SecretClientOptions>((provider, credential, options) =>
        //{
        //    var appOptions = provider.GetService<IOptions<MyApplicationOptions>>();
        //    return new SecretClient(appOptions.Value.KeyVaultEndpoint, credential, options);
        //});
    });

    services.AddHostedService<Startup>();
});

await builder.Build().RunAsync();
