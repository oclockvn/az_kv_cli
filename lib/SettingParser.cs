﻿using Azure.Identity;

namespace az_kv.lib;

public class SettingParser
{
    private readonly KeyVaultClient kvClient;

    public SettingParser(KeyVaultClient kvClient)
    {
        this.kvClient = kvClient;
    }

    public async Task<string[]> ParseAsync(string[] lines)
    {
        var found = 0;
        var result = new List<string>();

        try
        {
            foreach (var line in lines)
            {
                if (!ParserHelper.IsKeyVaultConfig(line, out var uri))
                {
                    result.Add(line);
                    continue;
                }

                if (!ParserHelper.TryParseKeyPair(line, out var key, out var value))
                {
                    result.Add(line);
                    continue;
                }

                found++;
                var vault = ParserHelper.ParseUri(value);
                var secret = await kvClient.GetSecretAsync(vault.vault, vault.key);

                var space = SpaceCount(line);
                var pad = space > 0 ? " ".PadLeft(space) : "";
                var ending = line.Trim().EndsWith(',') ? "," : string.Empty;
                var final = pad + "\"" + key + "\": \"" + secret + "\"" + ending;

                Console.WriteLine(found + " found -> " + final.Trim());

                result.Add(final);
            }
        }
        catch (CredentialUnavailableException)
        {
            Console.WriteLine("Unable to login. Try \"az login\" and try again");
        }

        return result.ToArray();
    }

    static int SpaceCount(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return 0;

        return s.Length - s.TrimStart().Length;
    }
}