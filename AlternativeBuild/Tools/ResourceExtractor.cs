using System.Reflection;

namespace AlternativeBuild.Tools;

/// <summary>
/// Extracts embedded resources to disk
/// </summary>
public class ResourceExtractor
{
    private static readonly string ToolsDir = Path.Combine(
        Path.GetTempPath(),
        "alternative-build-tools"
    );

    /// <summary>
    /// Extract all embedded tools to temp directory
    /// </summary>
    public static string ExtractTools()
    {
        if (!Directory.Exists(ToolsDir))
        {
            Directory.CreateDirectory(ToolsDir);
        }

        var assembly = Assembly.GetExecutingAssembly();
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(r => r.Contains("EmbeddedResources.tools"));

        foreach (var resourceName in resourceNames)
        {
            ExtractResource(assembly, resourceName, ToolsDir);
        }

        return ToolsDir;
    }

    /// <summary>
    /// Get path to specific embedded tool
    /// </summary>
    public static string GetToolPath(string toolName)
    {
        ExtractTools();
        return Path.Combine(ToolsDir, toolName);
    }

    private static void ExtractResource(Assembly assembly, string resourceName, string outputDir)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) return;

        // Calculate relative path from resource name
        var parts = resourceName.Split('.');
        var relativePath = string.Join("\\", parts.Skip(2)); // Skip namespace parts

        var outputPath = Path.Combine(outputDir, relativePath);
        var directory = Path.GetDirectoryName(outputPath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var fileStream = File.Create(outputPath);
        stream.CopyTo(fileStream);
    }
}
