// See https://aka.ms/new-console-template for more information
using az_kv.lib;
using az_kv.lib.Readers;
using Microsoft.Extensions.DependencyInjection;

static IServiceProvider Setup()
{
    var serviceCollection = new ServiceCollection();

    serviceCollection.AddLib();

    return serviceCollection.BuildServiceProvider();
}

Console.WriteLine("This App uses AAD to authenticate, ensure you have already logged in using az cli");

var sp = Setup();

Console.Write("Enter path to local.settings.json: ");
var settingFile = Console.ReadLine()?.Trim('"'); // trim "" in case you copy path from Windows

if (settingFile?.StartsWith('/') == true) // I'm on Windows
{
    settingFile = settingFile.TrimStart('/').Replace('/', '\\');
    settingFile = settingFile[0].ToString().ToUpper() + ":" + (settingFile.Length > 1 ? settingFile.Substring(1) : string.Empty);
}

Console.Write("Enter output file without path (default is local.settings.log): ");
var output = Console.ReadLine();

var reader = sp.GetRequiredService<ILocalSettingReader>();
await reader.ReadAsync(settingFile, output);

