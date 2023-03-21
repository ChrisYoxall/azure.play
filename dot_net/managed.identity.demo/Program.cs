using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

const string storageAccountName = "chrisidentitydemosa";    // Name of storage account
const string container = "chris-identity-demo-blob";        // Name of file share in the blob container
const string blob = "demo.csv";                             // Name of blob

const string blobEndpoint = $"https://{storageAccountName}.blob.core.windows.net/{container}/{blob}";

// The Managed Identity ClientID. Needs to be granted appropriate access such as 'Storage Blob Data Contributor'
// on the storage account.
const string managedIdentityId = "7fbee3a4-c13b-4131-af4f-ea2c0ec99346";

// From https://learn.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
//
//    - BlobServiceClient: The BlobServiceClient class allows you to manipulate Azure Storage resources and blob containers.
//    - BlobContainerClient: The BlobContainerClient class allows you to manipulate Azure Storage containers and their blobs.
//    - BlobClient: The BlobClient class allows you to manipulate Azure Storage blobs.

// DefaultAzureCredential will use credentials of person running code when developing locally. Need to be logged
// in (via az account login) and have access to blob. Will use the managed identity when running in Azure.
var blobClient = new BlobClient(new Uri(blobEndpoint),
    new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = managedIdentityId}));

using (var stream = blobClient.OpenRead(new BlobOpenReadOptions(false)))
using (var reader = new StreamReader(stream))
{
    while (!reader.EndOfStream)
    {
        Console.WriteLine(reader.ReadLine());
    }
}

Console.WriteLine($"{Environment.NewLine}Done!");