namespace az_kv.lib;

public static class PathValidator
{
    public static bool IsValidFile(string path, out string result)
    {
        result = string.Empty;
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("Empty path");
            return false;
        }

        path = path.Trim();
        if (!Path.HasExtension(path))
        {
            Console.WriteLine($"{path} has no extension");
            return false;
        }

        var ext = Path.GetExtension(path);
        if (!new[] { ".txt", ".log", ".json" }.Contains(ext.ToLower()))
        {
            Console.WriteLine($"{ext} is invalid. Only accept txt|log|json");
            return false;
        }

        // valid file
        if (File.Exists(path))
        {
            result = path;
            return true;
        }
        else // try to convert path from fw slash to back slack
        {
            path = NormalizePath(path);
            Console.WriteLine("Normalized=" + path);
            if (File.Exists(path))
            {
                result = path;
                return true;
            }
        }

        Console.WriteLine($"File {path} does not exist");
        return false;
    }

    static string NormalizePath(string path)
    {
        path = path.Trim('/');
        path = Path.Combine(path.Split("/"));

        if (path[1] == ':')
            return path;

        return path[0] + ":" + path[1..];
    }
}
