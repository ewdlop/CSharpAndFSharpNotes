namespace AzureSearchIndxer
{
    public class SearchIndexerClientOptions
    {
        public const string SearchIndexerClient = "SearchIndexerClient";
        public string IndexerName { get; set; }
        public string DataSourceName { get; set; }
        public string IndexName { get; set; }
    }
}
