using Microsoft.Extensions.Logging;

namespace AzureSearchIndxer;

internal class App
{
    private readonly ILogger _logger; 
    private readonly AzurSearchIndexerService _azurSearchIndexerService;

    public App(AzurSearchIndexerService azurSearchDocumentService, ILogger _logger)
    {
        _azurSearchIndexerService = azurSearchDocumentService;
    }

    public void Run()
    {
        _logger.LogInformation("Hello From App");
    }
}