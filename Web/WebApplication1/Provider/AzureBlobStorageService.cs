using Azure.Storage.Blobs;

namespace WebApplication1.Provider;

internal class AzureBlobStorageServiceProvider
{
    private readonly BlobServiceClient _blobClient;
    public AzureBlobStorageServiceProvider(BlobServiceClient blobClient)
    {
        _blobClient = blobClient;
    }
}
