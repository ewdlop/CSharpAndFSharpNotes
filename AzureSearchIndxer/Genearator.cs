using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Microsoft.Extensions.Logging;

namespace AzureSearchIndxer;

internal class Genearator
{
//    private readonly ILogger _logger; 
//    private readonly AzurSearchIndexerService _azurSearchIndexerService;

    //public Genearator(AzurSearchIndexerService azurSearchDocumentService, ILogger _logger)
    //{
    //    _azurSearchIndexerService = azurSearchDocumentService;
    //}

    private readonly Azure.Search.Documents.Indexes.SearchIndexClient _searchIndexClient;
    private readonly Azure.Search.Documents.SearchClient _searchClient;

    public Genearator(SearchClient searchClient)
    {
        _searchClient = searchClient;
    }

    public async Task Run(int target)
    {
        Console.WriteLine("yolo");
        //_logger.LogInformation("Hello From App");
        await Task.CompletedTask;
    }
}