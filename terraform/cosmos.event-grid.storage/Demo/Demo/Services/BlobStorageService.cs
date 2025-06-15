using System.Text;
using Azure.Storage.Blobs;
using Demo.Models;
using Demo.Providers;
using ILogger = Serilog.ILogger;

namespace Demo.Services;

public interface IBlobStorageService
{
    Task<string> UploadDocumentAsync(DemoBlobDocument document);
    Task<string> GetDocumentContentAsync(string blobId);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly IBlobStorageProvider _blobStorageProvider;
    private readonly ILogger _logger;

    public BlobStorageService(IBlobStorageProvider blobStorageProvider, ILogger logger)
    {
        _blobStorageProvider = blobStorageProvider;
        _logger = logger.ForContext<BlobStorageService>();
    }

    public async Task<string> UploadDocumentAsync(DemoBlobDocument document)
    {
        _logger.Information("Uploading document to blob storage: {DocumentId}", document.Id);

        var containerClient = _blobStorageProvider.GetBlobContainerClient();
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(document.Id);
        var content = $"This is a demo blob document with name: {document.Name}\nCreated at: {document.CreatedAt:yyyy-MM-dd HH:mm:ss}";

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        await blobClient.UploadAsync(memoryStream, overwrite: true);

        _logger.Information("Document uploaded successfully to blob storage: {DocumentId}", document.Id);
        return document.Id;
    }

    public async Task<string> GetDocumentContentAsync(string blobId)
    {
        _logger.Information("Retrieving document from blob storage: {BlobId}", blobId);

        var containerClient = _blobStorageProvider.GetBlobContainerClient();
        var blobClient = containerClient.GetBlobClient(blobId);

        using var memoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(memoryStream);
        memoryStream.Position = 0;

        using var reader = new StreamReader(memoryStream);
        var content = await reader.ReadToEndAsync();

        return content;
    }
}
