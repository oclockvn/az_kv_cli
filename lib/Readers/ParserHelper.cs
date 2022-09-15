using System.Text.RegularExpressions;

namespace az_kv.lib.Readers;

public static class ParserHelper
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="uri">https://kv-somevault-dev.vault.azure.net/secrets/SomeServiceKey/</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static (string vault, string key) ParseUri(string uri)
    {
        var match = Regex.Match(uri, ".*https:\\/\\/(.*)\\.vault\\.azure\\.net\\/secrets\\/([a-zA-Z_-]*)\\/{0,1}.*");
        if (!match.Success)
        {
            throw new ArgumentException("{url} is not a keyvault uri");
        }

        var groups = match.Groups;

        var vault = groups[1].Value;
        var key = groups[2].Value;

        return (vault, key);
    }

    /// <summary>
    /// "SomeConfigKey": "@Microsoft.KeyVault(SecretUri=https://vault-name.vault.azure.net/secrets/SecretKey/)"
    /// </summary>
    /// <param name="line">This is a line of text in the local.settings.json file</param>
    /// <returns>A tupe contains key and value</returns>
    public static (string key, string value) ExtractConfigKey(string line)
    {
        var match = Regex.Match(line, "\"(.*)?\":\\s{0,}\"@Microsoft.KeyVault\\(SecretUri=(.*)\\/\\)\"");
        if (!match.Success)
        {
            throw new ArgumentException($"Something wrong with this line: {line}. Recheck the regex pattern.");
        }

        return (match.Groups[1].Value, match.Groups[2].Value);
    }

    /// <summary>
    /// "SomeConfigKey": "@Microsoft.KeyVault(SecretUri=https://vault-name.vault.azure.net/secrets/SecretKey/)"
    /// </summary>
    /// <param name="line">This is a line of text in the local.settings.json file</param>
    /// <returns>A tupe contains key and value</returns>
    public static string ReplaceSecretValue(string original, string secretValue)
    {
        return Regex.Replace(original, @"""@Microsoft.KeyVault\(SecretUri=.*\)""", string.Format("\"{0}\"", secretValue));
    }
}

