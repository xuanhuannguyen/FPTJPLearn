using System.Text.Json;

namespace JPLearn.Infrastructure.Data.Seed;

internal static class ImportDataFileLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public static IReadOnlyList<T> LoadAll<T>(params string[] pathParts)
    {
        var directory = ResolveDirectory(pathParts);
        if (!Directory.Exists(directory))
        {
            return [];
        }

        return Directory.EnumerateFiles(directory, "*.json")
            .OrderBy(path => path)
            .Select(path =>
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<T>(json, JsonOptions)
                    ?? throw new InvalidOperationException($"Import file is empty or invalid: {path}");
            })
            .ToList();
    }

    private static string ResolveDirectory(params string[] pathParts)
    {
        var copiedDirectory = Path.Combine([AppContext.BaseDirectory, "Data", "Imports", .. pathParts]);
        if (Directory.Exists(copiedDirectory))
        {
            return copiedDirectory;
        }

        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null)
        {
            var sourceDirectory = Path.Combine([current.FullName, "JPLearn.Infrastructure", "Data", "Imports", .. pathParts]);
            if (Directory.Exists(sourceDirectory))
            {
                return sourceDirectory;
            }

            current = current.Parent;
        }

        return copiedDirectory;
    }
}
