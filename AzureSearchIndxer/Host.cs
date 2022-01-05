using Microsoft.Extensions.Hosting;

namespace AzureSearchIndxer;

internal class Host : IHostedService
{
    private readonly AzurSearchIndexerService _azurSearchIndexerService;

    public Host(AzurSearchIndexerService azurSearchDocumentService)
    {
        _azurSearchIndexerService = azurSearchDocumentService;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _azurSearchIndexerService.Run(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _azurSearchIndexerService.Stop(cancellationToken);
    }
}
