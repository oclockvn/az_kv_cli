namespace az_kv.lib;

public class SettingWriter
{
    public static async Task<(bool success, string msg)> WriteAsync(string inputPath, string outputFile, string[] lines)
    {
        var dir = Path.GetDirectoryName(inputPath);
        var inputFile = Path.GetFileName(inputPath);

        if (string.IsNullOrWhiteSpace(outputFile))
            outputFile = inputFile + ".log";

        outputFile = Path.Combine(dir, outputFile);

        try
        {
            await File.WriteAllLinesAsync(outputFile, lines);

            return (true, outputFile);
        }
        catch (Exception ex)
        {
            return (false, $"Could not write file {outputFile}, reason: {ex.Message}");
        }
    }
}
