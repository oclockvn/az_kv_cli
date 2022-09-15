using Newtonsoft.Json;

namespace az_kv.lib;

internal sealed record NameValue(string name, string value);

public class AppServiceConfigurationParser
{
    public static async Task<string[]> ParseAppSettingAsync(string filePath)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var arr = JsonConvert.DeserializeObject<List<NameValue>>(json);

        if (arr == null || arr.Count == 0)
            return null;

        var lines = new List<string>
        { "{" };

        foreach (var slot in arr)
        {
            lines.Add(" ".PadLeft(2) + slot.name.Wrap('"') + ": " + slot.value.Wrap('"') + ',');
        }
        lines[^1] = lines.Last().TrimEnd(','); // trim , from last line
        lines.Add("}");

        return lines.ToArray();
    }
}
