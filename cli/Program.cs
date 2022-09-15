// See https://aka.ms/new-console-template for more information
using az_kv.cli;
using az_kv.lib;
using az_kv.lib.Readers;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;

static IServiceProvider Setup()
{
    var serviceCollection = new ServiceCollection();

    serviceCollection.AddLib();

    return serviceCollection.BuildServiceProvider();
}

Console.WriteLine("This App uses AAD to authenticate, ensure you have already logged in using az cli");

var sp = Setup();

await Parser.Default.ParseArguments<CmdOption>(() => new CmdOption { Type = "fn", Input = "." }, args)
    .WithParsedAsync(async opt =>
    {
        var validFile = PathValidator.IsValidFile(opt.Input, out var path);
        if (!validFile)
        {
            Console.WriteLine($"Input param is invalid file path.");
            return;
        }

        var lines = await File.ReadAllLinesAsync(path);

        if (opt.Type == "fn") // azure function
        {
            Console.WriteLine($"1. type = {opt.Type}, input = {opt.Input}, output = {opt.Output}");
        }
        else // azure app service
        {
            Console.WriteLine($"2. type = {opt.Type}, input = {opt.Input}, output = {opt.Output}");
        }
    });

//Console.Write("Enter path to local.settings.json: ");
//var settingFile = Console.ReadLine()?.Trim('"'); // trim "" in case you copy path from Windows

//if (settingFile?.StartsWith('/') == true) // I'm on Windows
//{
//    settingFile = settingFile.TrimStart('/').Replace('/', '\\');
//    settingFile = settingFile[0].ToString().ToUpper() + ":" + (settingFile.Length > 1 ? settingFile.Substring(1) : string.Empty);
//}

//Console.Write("Enter output file without path (default is local.settings.log): ");
//var output = Console.ReadLine();

//var reader = sp.GetRequiredService<ILocalSettingReader>();
//await reader.ReadAsync(settingFile, output);

