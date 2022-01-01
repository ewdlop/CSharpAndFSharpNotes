using Microsoft.Extensions.Hosting;

namespace AzureSearchIndxer;

internal class Host : IHostedService
{
    private readonly AzurSearcIndexerService _azurSearchDocumentService;

    public Host(AzurSearcIndexerService azurSearchDocumentService)
    {
        _azurSearchDocumentService = azurSearchDocumentService;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _azurSearchDocumentService.Run(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _azurSearchDocumentService.Stop(cancellationToken);
    }
}
