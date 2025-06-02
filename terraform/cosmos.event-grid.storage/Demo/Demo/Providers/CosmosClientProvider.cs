using Demo.Settings;
using Microsoft.Azure.Cosmos;

namespace Demo.Providers;

public interface ICosmosClientProvider
{
    Container ItemsContainer { get; init; }
}

public class CosmosClientProvider : ICosmosClientProvider
{
    public Container ItemsContainer { get; init; }
    
    public CosmosClientProvider(ICredentialProvider credentialProvider, CosmosSettings cosmosSettings)
    {
        var client = new CosmosClient(cosmosSettings.AccountEndpoint, credentialProvider.GetCredential());
        var database = client.GetDatabase(cosmosSettings.DatabaseName);
        ItemsContainer = database.GetContainer(cosmosSettings.ContainerName);
    }
}