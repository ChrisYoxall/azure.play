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

    public CredentialProvider()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        _isProd = environment == "Production";
    }

    public TokenCredential GetCredential()
    {
        if (_isProd)
        {
            return new ManagedIdentityCredential();
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