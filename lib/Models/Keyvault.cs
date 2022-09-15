namespace az_kv.lib.Models;

public class Keyvault
{
    /// <summary>
    /// Given url like this SecretUri=https://kv-somevault-dev.vault.azure.net/secrets/SomeServiceKey/<br/>
    /// Then vault name will be "kb-somevault-dev"
    /// </summary>
    public string VaultName { get; set; }

    /// <summary>
    /// Given url like this SecretUri=https://kv-somevault-dev.vault.azure.net/secrets/SomeServiceKey/<br/>
    /// Then key name will be "SomeServiceKey"
    /// </summary>
    public string SecretName { get; set; }
}
