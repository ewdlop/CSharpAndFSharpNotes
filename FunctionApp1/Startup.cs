using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(FunctionApp1.Startup))]


namespace FunctionApp1
{
    /// <summary>
    /// https://devblogs.microsoft.com/cosmosdb/httpclientfactory-cosmos-db-net-sdk/
    /// </summary>
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton(serviceProvider =>
            {
                IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                CosmosClientOptions cosmosClientOptions = new CosmosClientOptions()
                {
                    HttpClientFactory = httpClientFactory.CreateClient
                };
                return new CosmosClient("<connection-string>", cosmosClientOptions);
            });
        }
    }
}
