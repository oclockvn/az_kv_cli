using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace az_kv.lib;

public static class ParserHelper
{
    public static string Wrap(this string text, char wrapper)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        return wrapper + text + wrapper;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="uri">https://kv-somevault-dev.vault.azure.net/secrets/SomeServiceKey/</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static (string vault, string key) ParseUri(string uri)
    {
        var match = Regex.Match(uri.Trim().Trim('/'), "https:\\/\\/(.*)\\.vault\\.azure\\.net\\/secrets\\/([0-9a-zA-Z_-]*)");
        if (!match.Success)
        {
            throw new ArgumentException($"{uri} is not a keyvault uri");
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
    //public static (string key, string value) ExtractConfigKey(string line)
    //{
    //    var match = Regex.Match(line, "\"(.*)?\":\\s{0,}\"@Microsoft.KeyVault\\(SecretUri=(.*)\\/\\)\"");
    //    if (!match.Success)
    //    {
    //        throw new ArgumentException($"Something wrong with this line: {line}. Recheck the regex pattern.");
    //    }

    //    return (match.Groups[1].Value, match.Groups[2].Value);
    //}

    /// <summary>
    /// "SomeConfigKey": "@Microsoft.KeyVault(SecretUri=https://vault-name.vault.azure.net/secrets/SecretKey/)"
    /// </summary>
    /// <param name="line">This is a line of text in the local.settings.json file</param>
    /// <returns>A tupe contains key and value</returns>
    public static string ReplaceSecretValue(string original, string secretValue)
    {
        return Regex.Replace(original, @"""@Microsoft.KeyVault\(SecretUri=.*\)""", string.Format("\"{0}\"", secretValue));
    }

    public static bool IsKeyVaultConfig(string text, out string uri)
    {
        uri = string.Empty;
        // "@Microsoft.KeyVault(SecretUri=https://sample.vault.azure.net/secrets/JwtCertificateThumbprint/)"
        var match = Regex.Match(text, @".*@Microsoft.KeyVault\(SecretUri=(https:\/\/.*\.vault\.azure\.net/.*)/\)");
        if (!match.Success)
        {
            return false;
        }

        uri = match.Groups[1].Value;
        return true;
    }

    public static bool TryParseKeyPair(string text, out string key, out string value)
    {
        key = null;
        value = null;

        if (string.IsNullOrWhiteSpace(text))
            return false;

        text = "{" + text.Trim() + "}";

        try
        {
            var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            key = dic.Keys.First();
            value = dic.Values.First();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

