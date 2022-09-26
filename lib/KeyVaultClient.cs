using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace az_kv.lib;

public class KeyVaultClient
{
    static readonly Dictionary<string, SecretClient> cache = new Dictionary<string, SecretClient>();

    public async Task<string> GetSecretAsync(string vault, string secretName)
    {
        var kvUri = "https://" + vault + ".vault.azure.net";

        SecretClient client;
        if (!cache.ContainsKey(kvUri))
        {
            client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            cache.Add(kvUri, client);
        }
        else
        {
            client = cache[kvUri];
        }

        try
        {
            var secret = await client.GetSecretAsync(secretName);

            return secret?.Value?.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Something went wrong with the key {secretName}: {ex.Message}");
            return $"--{secretName}--";
        }
    }
}

