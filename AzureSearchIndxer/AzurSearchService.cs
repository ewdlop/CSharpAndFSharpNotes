using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace AzureSearchIndxer
{
    internal class AzurSearchService : IRunable
    {
        private readonly SearchClient _searchclient;
        private readonly SearchIndexClient _searchindexclient;

        public AzurSearchService(SearchClient searchClient, SearchIndexClient searchindexclient)
        {
            _searchclient = searchClient;
            _searchindexclient = searchindexclient;
        }

        public Task Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Stop(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
