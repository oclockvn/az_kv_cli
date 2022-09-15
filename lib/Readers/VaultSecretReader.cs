using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace az_kv.lib.Readers;

public interface IVaultSecretReader
{
    Task<string> GetSecretAsync(string vault, string secretName);
}

public class VaultSecretReader : IVaultSecretReader
{
    public async Task<string> GetSecretAsync(string vault, string secretName)
    {
        var kvUri = "https://" + vault + ".vault.azure.net";

        var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
        var secret = await client.GetSecretAsync(secretName);

        return secret?.Value?.Value;
    }
}

