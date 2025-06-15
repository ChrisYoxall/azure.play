using Azure.Storage.Blobs;
using Demo.Settings;

namespace Demo.Providers;

public interface IBlobStorageProvider
{
    BlobContainerClient GetBlobContainerClient();
}

public class BlobStorageProvider : IBlobStorageProvider
{
    private readonly BlobStorageSettings _settings;
    private readonly ICredentialProvider _credentialProvider;

    public BlobStorageProvider(BlobStorageSettings settings, ICredentialProvider credentialProvider)
    {
        _settings = settings;
        _credentialProvider = credentialProvider;
    }

    public BlobContainerClient GetBlobContainerClient()
    {
        var credential = _credentialProvider.GetCredential();
        var blobServiceClient = new BlobServiceClient(new Uri(_settings.BlobServiceUrl), credential);
        return blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
    }
}
