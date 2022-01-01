using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Extensions.Options;
using Microsoft.Rest.Azure;
using System.Net;

namespace AzureSearchIndxer
{

    internal class AzurSearchIndexerService : IRunable
    {
        private readonly IOptions<AzureBlobOptions> _azureBlobOptions;
        private readonly SearchIndexer _azureBlobIndexer;
        private readonly SearchIndexerClient _searchIndexerClient;
        private readonly IndexingParameters _indexingParameters;
        private readonly SearchIndexerDataContainer _searchIndexerDataContainer;

        public AzurSearchIndexerService(
            IOptions<AzureBlobOptions> azureBlobOptions,
            IOptions<SearchIndexerClientOptions> searchIndexerClientOptions
            )
        {
            _azureBlobOptions = azureBlobOptions;

            _indexingParameters = new();
            _indexingParameters.Configuration.Add("parsingMode", "json");

            _searchIndexerClient = new SearchIndexerClient(null, null);

            _azureBlobIndexer = new SearchIndexer(
                name: searchIndexerClientOptions.Value.IndexerName,
                dataSourceName: searchIndexerClientOptions.Value.DataSourceName,
                targetIndexName: searchIndexerClientOptions.Value.IndexName)
            {
                Parameters = _indexingParameters,
                Schedule = new IndexingSchedule(TimeSpan.FromDays(1))
            };

            _azureBlobIndexer.FieldMappings.Add(
                new FieldMapping("Id")
                {
                    TargetFieldName = "HotelId"
                });
            _searchIndexerDataContainer = new SearchIndexerDataContainer("hotel-rooms");
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            await CreateAndRunBlobIndexerAsync(cancellationToken);
        }

        private async Task CreateAndRunBlobIndexerAsync(CancellationToken cancellationToken)
        {
            SearchIndexerDataSourceConnection blobDataSource = new SearchIndexerDataSourceConnection(
                name: _azureBlobOptions.Value.Name,
                type: SearchIndexerDataSourceType.AzureBlob,
                connectionString: _azureBlobOptions.Value.ConnectionString,
                container: _searchIndexerDataContainer);

            await _searchIndexerClient.CreateOrUpdateDataSourceConnectionAsync(blobDataSource, false, cancellationToken);
            
            try
            {
                await _searchIndexerClient.GetIndexerAsync(_azureBlobIndexer.Name);
                await _searchIndexerClient.ResetIndexerAsync(_azureBlobIndexer.Name);
            }
            catch (RequestFailedException ex) when (ex.Status == 404) { }

            await _searchIndexerClient.CreateOrUpdateIndexerAsync(_azureBlobIndexer);

            try
            {
                // Run the indexer.
                await _searchIndexerClient.RunIndexerAsync(_azureBlobIndexer.Name,cancellationToken);
            }
            catch (CloudException e) when (e.Response.StatusCode == (HttpStatusCode)429)
            {
                Console.WriteLine("Failed to run indexer: {0}", e.Response.Content);
            }
        }


        public async Task Stop(CancellationToken cancellationToken) //cancellationToken is cancelling the stop
        {

        }
    }
}
