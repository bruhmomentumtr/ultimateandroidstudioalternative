using AlternativeBuild.Utils;
using System.Reflection;

namespace AlternativeBuild.Tools;

/// <summary>
/// Manages embedded Android tools (ADB, AAPT2, etc.)
/// </summary>
public static class ToolsManager
{
    private static readonly string ToolsDir = Path.Combine(
        Path.GetTempPath(),
        "alternative-build-tools"
    );

    private static bool _toolsExtracted = false;

    /// <summary>
    /// Extract all embedded tools on first use
    /// </summary>
    public static void EnsureToolsExtracted()
    {
        if (_toolsExtracted) return;

        ConsoleLogger.Info("Extracting embedded build tools...");

        try
        {
            Directory.CreateDirectory(ToolsDir);

            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames()
                .Where(r => r.Contains("EmbeddedResources.tools"));

            foreach (var resourceName in resourceNames)
            {
                ExtractResource(assembly, resourceName);
            }

            _toolsExtracted = true;
            ConsoleLogger.Success("Build tools extracted successfully");
        }
        catch (Exception ex)
        {
            ConsoleLogger.Warning($"Failed to extract some tools: {ex.Message}");
        }
    }

    /// <summary>
    /// Get path to ADB executable
    /// </summary>
    public static string GetAdbPath()
    {
        EnsureToolsExtracted();
        return Path.Combine(ToolsDir, "adb", "adb.exe");
    }

    /// <summary>
    /// Get path to AAPT2
    /// </summary>
    public static string GetAapt2Path()
    {
        EnsureToolsExtracted();
        return Path.Combine(ToolsDir, "build-tools", "aapt2.exe");
    }

    /// <summary>
    /// Get path to Zipalign
    /// </summary>
    public static string GetZipalignPath()
    {
        EnsureToolsExtracted();
        return Path.Combine(ToolsDir, "build-tools", "zipalign.exe");
    }

    /// <summary>
    /// Get path to Apksigner JAR
    /// </summary>
    public static string GetApksignerPath()
    {
        EnsureToolsExtracted();
        return Path.Combine(ToolsDir, "build-tools", "apksigner.jar");
    }

    private static void ExtractResource(Assembly assembly, string resourceName)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) return;

        // Parse resource name to get relative path
        // Format: AlternativeBuild.EmbeddedResources.tools.adb.adb.exe
        var parts = resourceName.Split('.');
        var startIndex = Array.IndexOf(parts, "tools");

        if (startIndex < 0) return;

        var pathParts = new List<string>();
        for (int i = startIndex + 1; i < parts.Length; i++)
        {
            pathParts.Add(parts[i]);
        }

        var relativePath = string.Join("\\", pathParts);
        var outputPath = Path.Combine(ToolsDir, relativePath);

        var directory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var fileStream = File.Create(outputPath);
        stream.CopyTo(fileStream);
    }

    /// <summary>
    /// Check if specific tool is available
    /// </summary>
    public static bool IsToolAvailable(string toolName)
    {
        EnsureToolsExtracted();

        return toolName.ToLower() switch
        {
            "adb" => File.Exists(GetAdbPath()),
            "aapt2" => File.Exists(GetAapt2Path()),
            "zipalign" => File.Exists(GetZipalignPath()),
            "apksigner" => File.Exists(GetApksignerPath()),
            _ => false
        };
    }
}
