using az_kv.lib;
using cli;
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
            Console.WriteLine($"Input param {opt.Input} is an invalid file path.");
            return;
        }

        var parser = sp.GetRequiredService<SettingParser>();
        string[] lines;

        if (opt.Type == "app") // azure app service
        {
            // convert lines into valid json
            lines = await AppServiceConfigurationParser.ParseAppSettingAsync(path);
        }
        else
        {
            lines = await File.ReadAllLinesAsync(path);
        }

        var result = await parser.ParseAsync(lines);
        var (success, msg) = await SettingWriter.WriteAsync(path, opt.Output, result);

        if (success)
        {
            Console.WriteLine("\r\n*****************\r\nParse successfully > " + msg + "\r\n*****************");
        }
        else
        {
            Console.WriteLine(msg);
        }
    });

