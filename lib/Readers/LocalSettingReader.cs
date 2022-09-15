using Azure.Identity;
using System.Text.RegularExpressions;

namespace az_kv.lib.Readers;

public interface ILocalSettingReader
{
    Task ReadAsync(string path, string outputFile);
}

public class LocalSettingReader : ILocalSettingReader
{
    private readonly IVaultSecretReader vaultSecretReader;

    public LocalSettingReader(IVaultSecretReader vaultSecretReader)
    {
        this.vaultSecretReader = vaultSecretReader;
    }

    public async Task ReadAsync(string path, string outputFile)
    {
        if (File.Exists(path) == false)
        {
            Console.WriteLine($"File {path} does not exist.");
            return;
        }

        var lines = await File.ReadAllLinesAsync(path);
        if (lines.Length == 0)
        {
            return;
        }

        Console.WriteLine("Starting processing lines...");
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

        var dir = Path.GetDirectoryName(path);
        if (string.IsNullOrWhiteSpace(outputFile))
        {
            outputFile = "local.settings.log";
        }

        outputFile = Path.Combine(dir, outputFile);

        await File.WriteAllLinesAsync(outputFile, result);

        Console.WriteLine($"Output saved to {outputFile}");
    }

    private static bool IsKeyvault(string line)
    {
        return Regex.IsMatch(line, "@Microsoft.KeyVault.*https:\\/\\/.*\\.vault\\.azure\\.net.*");
    }
}

