using CommandLine;

namespace az_kv.cli;

internal class CmdOption
{
    [Option(
        't',
        "type",
        HelpText = "Configuration type.\r\nfn: Azure function setings (default).\r\napp: Azure App Service configuration"
        )]
    public string Type { get; set; }

    [Option(
        'i',
        "input",
        HelpText = "Path to the file",
        MetaValue = "")]
    public string Input { get; set; }

    [Option(
        'o',
        "output",
        HelpText = "Path to the output file",
        MetaValue = "")]
    public string Output { get; set; }
}
