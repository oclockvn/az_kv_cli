namespace az_kv.lib;

public static class PathValidator
{
    public static bool IsValidFile(string path, out string result)
    {
        result = string.Empty;
        if (string.IsNullOrEmpty(path))
            return false;

        path = path.Trim();
        if (!Path.IsPathFullyQualified(path))
            return false;

        if (!Path.HasExtension(path))
            return false;

        var ext = Path.GetExtension(path);
        if (!new[] { ".txt", ".log", ".json" }.Contains(ext.ToLower()))
            return false;

        // valid file
        if (File.Exists(path))
        {
            result = path;
            return true;
        }
        else // try to convert path from fw slash to back slack
        {
            path = NormalizePath(path);
            if (File.Exists(path))
            {
                result = path;
                return true;
            }
        }

        return false;
    }

    // if you're encountering issue, pls raise a PR
    // sorry linux :v
    static string NormalizePath(string path)
    {
        path = path.Trim('/').Replace('/', '\\');
        return path[0] + ":" + path[1..];
    }
}
