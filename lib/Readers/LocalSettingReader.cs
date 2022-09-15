using Azure.Identity;
using System.Text.RegularExpressions;

namespace az_kv.lib.Readers;

public sealed record AppServiceConfiguration(string Name, string Value);

public interface ILocalSettingReader
{
    Task ReadAsync(string path, string outputFile);
}

public class LocalSettingReader// : ILocalSettingReader
{
    private readonly IVaultSecretReader vaultSecretReader;

    public LocalSettingReader(IVaultSecretReader vaultSecretReader)
    {
        this.vaultSecretReader = vaultSecretReader;
    }

    public async Task<string[]> ReadAsync(string[] lines)
    {
        var found = 0;

        var result = new List<string>();
        try
        {
            foreach (var line in lines)
            {
                if (!IsKeyvault(line))
                {
                    result.Add(line);
                    continue;
                }

                found++;
                var (_, value) = ParserHelper.ExtractConfigKey(line);
                var vault = ParserHelper.ParseUri(value);
                var secret = await vaultSecretReader.GetSecretAsync(vault.vault, vault.key);

                Console.WriteLine($"{vault.key} = {secret}");

                var final = ParserHelper.ReplaceSecretValue(line, secret);
                result.Add(final);
            }
        }
        catch (CredentialUnavailableException)
        {
            Console.WriteLine("Unable to login. Try \"az login\" and try again");
            return;
        }

        Console.WriteLine($"> Done process file. Found {found} secret keys");

        //var dir = Path.GetDirectoryName(path);
        //if (string.IsNullOrWhiteSpace(outputFile))
        //{
        //    outputFile = "local.settings.log";
        //}

        //outputFile = Path.Combine(dir, outputFile);

        //await File.WriteAllLinesAsync(outputFile, result);

        //Console.WriteLine($"Output saved to {outputFile}");

        return null;
    }

}
public class KeyMatcher
{
    public static bool IsKeyVault(string text, out string secret, out string s2)
    {
        var valid= Regex.IsMatch(text, "@Microsoft.KeyVault.*https:\\/\\/.*\\.vault\\.azure\\.net.*");
        if (!valid)
        {
            return false;
        }
        var match = Regex.Match(line, "\"(.*)?\":\\s{0,}\"@Microsoft.KeyVault\\(SecretUri=(.*)\\/\\)\"");
        if (!match.Success)
        {
            throw new ArgumentException($"Something wrong with this line: {line}. Recheck the regex pattern.");
        }

        return (match.Groups[1].Value, match.Groups[2].Value);
    }
}
