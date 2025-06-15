using Azure.Core;
using Azure.Identity;

namespace Demo.Providers;

public interface ICredentialProvider
{
    TokenCredential GetCredential();
}

public class CredentialProvider : ICredentialProvider
{
    private readonly bool _isProd;
    private readonly string? _clientId;

    public CredentialProvider()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(environment))
        {
            throw new Exception("ASPNETCORE_ENVIRONMENT environment variable not set");
        }
        _isProd = environment == "Production";
        
        _clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID");
        if (_isProd && string.IsNullOrEmpty(_clientId))
        {
            throw new Exception("AZURE_CLIENT_ID environment variable not set");
        }
    }

    public CredentialProvider(string clientId)
    {
        _clientId = clientId;
    }

    public TokenCredential GetCredential()
    {
        if (_isProd)
        {
            return new ManagedIdentityCredential(clientId: _clientId);
        }

        // You can also create a credential that excludes the credential types that you don't want to use. This is
        // useful if there are multiple credentials you wish to allow but don't want to use them all which can
        // slow down the authentication process.
        // return new DefaultAzureCredential(new DefaultAzureCredentialOptions
        //     {
        //         ExcludeEnvironmentCredential = true,
        //         ExcludeInteractiveBrowserCredential = true,
        //     });

        return new AzureCliCredential();
    }
}