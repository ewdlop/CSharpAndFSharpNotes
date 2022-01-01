using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((context, services) =>
{
    services.AddAzureClients(builder =>
    {
        
    });
});

