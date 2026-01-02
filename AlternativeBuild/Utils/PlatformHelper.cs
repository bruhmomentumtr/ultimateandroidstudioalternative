using System.Runtime.InteropServices;

namespace AlternativeBuild.Utils;

/// <summary>
/// Cross-platform helper utilities
/// </summary>
public static class PlatformHelper
{
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    public static string PlatformName
    {
        get
        {
            if (IsWindows) return "Windows";
            if (IsLinux) return "Linux";
            if (IsMacOS) return "macOS";
            return "Unknown";
        }
    }

    /// <summary>
    /// Get executable extension (.exe for Windows, empty for Unix)
    /// </summary>
    public static string ExecutableExtension => IsWindows ? ".exe" : "";

    /// <summary>
    /// Get script extension (.bat for Windows, .sh for Unix)
    /// </summary>
    public static string ScriptExtension => IsWindows ? ".bat" : ".sh";

    /// <summary>
    /// Get user home directory
    /// </summary>
    public static string HomeDirectory => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    /// <summary>
    /// Get platform-specific SDK directory
    /// </summary>
    public static string GetDefaultSdkPath()
    {
        return Path.Combine(HomeDirectory, ".alternative-sdk");
    }

    /// <summary>
    /// Get platform-specific config directory
    /// </summary>
    public static string GetConfigDirectory()
    {
        return Path.Combine(HomeDirectory, ".alternative-build");
    }

    /// <summary>
    /// Make file executable (Linux/macOS)
    /// </summary>
    public static void MakeExecutable(string filePath)
    {
        if (IsWindows) return;

        try
        {
            // chmod +x
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "chmod",
                Arguments = $"+x \"{filePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            process?.WaitForExit();
        }
        catch
        {
            // Ignore errors
        }
    }

    /// <summary>
    /// Get platform-specific ANDROID_HOME default path
    /// </summary>
    public static string? GetDefaultAndroidHome()
    {
        if (IsWindows)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Android", "Sdk");
        }
        else if (IsMacOS)
        {
            return Path.Combine(HomeDirectory, "Library", "Android", "sdk");
        }
        else if (IsLinux)
        {
            return Path.Combine(HomeDirectory, "Android", "Sdk");
        }
        return null;
    }
}
