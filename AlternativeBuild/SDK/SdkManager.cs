using AlternativeBuild.Utils;

namespace AlternativeBuild.SDK;

/// <summary>
/// Manages Android SDK, NDK, and Flutter SDK installations
/// </summary>
public class SdkManager
{
    private readonly string _sdkRoot;

    public SdkManager()
    {
        _sdkRoot = ConfigManager.Instance.SdkPath;
    }

    public string AndroidSdkPath => Path.Combine(_sdkRoot, "android");
    public string NdkPath => Path.Combine(_sdkRoot, "ndk");
    public string FlutterSdkPath => Path.Combine(_sdkRoot, "flutter");

    /// <summary>
    /// Install Android SDK (CommandLine Tools)
    /// </summary>
    public async Task<bool> InstallAndroidSdkAsync(string version)
    {
        try
        {
            ConsoleLogger.Header($"Installing Android SDK {version}");

            // Android SDK CommandLine Tools URL
            var url = $"https://dl.google.com/android/repository/commandlinetools-win-{version}_latest.zip";
            var targetDir = Path.Combine(AndroidSdkPath, $"cmdline-tools-{version}");

            if (Directory.Exists(targetDir))
            {
                ConsoleLogger.Warning($"Android SDK {version} already installed");
                return true;
            }

            await SdkDownloader.DownloadAndExtractAsync(url, targetDir, $"Android SDK {version}");

            // Set ANDROID_HOME environment variable recommendation
            ConsoleLogger.Info($"Add to environment: ANDROID_HOME={AndroidSdkPath}");

            return true;
        }
        catch (Exception ex)
        {
            ConsoleLogger.Error($"Failed to install Android SDK: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Install Android NDK
    /// </summary>
    public async Task<bool> InstallNdkAsync(string version)
    {
        try
        {
            ConsoleLogger.Header($"Installing Android NDK {version}");

            // NDK download URL
            var url = $"https://dl.google.com/android/repository/android-ndk-r{version}-windows.zip";
            var targetDir = Path.Combine(NdkPath, version);

            if (Directory.Exists(targetDir))
            {
                ConsoleLogger.Warning($"NDK {version} already installed");
                return true;
            }

            await SdkDownloader.DownloadAndExtractAsync(url, targetDir, $"NDK {version}");

            return true;
        }
        catch (Exception ex)
        {
            ConsoleLogger.Error($"Failed to install NDK: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Install Flutter SDK
    /// </summary>
    public async Task<bool> InstallFlutterSdkAsync(string version)
    {
        try
        {
            ConsoleLogger.Header($"Installing Flutter SDK {version}");

            // Flutter SDK URL
            var url = $"https://storage.googleapis.com/flutter_infra_release/releases/stable/windows/flutter_windows_{version}-stable.zip";
            var targetDir = Path.Combine(FlutterSdkPath, version);

            if (Directory.Exists(targetDir))
            {
                ConsoleLogger.Warning($"Flutter SDK {version} already installed");
                return true;
            }

            await SdkDownloader.DownloadAndExtractAsync(url, targetDir, $"Flutter SDK {version}");

            // Add flutter to PATH recommendation
            var flutterBin = Path.Combine(targetDir, "flutter", "bin");
            ConsoleLogger.Info($"Add to PATH: {flutterBin}");

            return true;
        }
        catch (Exception ex)
        {
            ConsoleLogger.Error($"Failed to install Flutter SDK: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// List installed SDKs
    /// </summary>
    public void ListInstalled()
    {
        ConsoleLogger.Header("INSTALLED SDKs");

        Console.WriteLine("\nAndroid SDKs:");
        ListDirectory(AndroidSdkPath);

        Console.WriteLine("\nNDKs:");
        ListDirectory(NdkPath);

        Console.WriteLine("\nFlutter SDKs:");
        ListDirectory(FlutterSdkPath);
    }

    private void ListDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            ConsoleLogger.Progress("(none installed)");
            return;
        }

        var subdirs = Directory.GetDirectories(path);
        if (subdirs.Length == 0)
        {
            ConsoleLogger.Progress("(none installed)");
            return;
        }

        foreach (var dir in subdirs)
        {
            var name = Path.GetFileName(dir);
            ConsoleLogger.Progress($"✓ {name}");
        }
    }

    /// <summary>
    /// Get path to specific SDK installation
    /// </summary>
    public string? GetSdkPath(string sdkType, string version)
    {
        var basePath = sdkType.ToLower() switch
        {
            "android" => AndroidSdkPath,
            "ndk" => NdkPath,
            "flutter" => FlutterSdkPath,
            _ => null
        };

        if (basePath == null) return null;

        var sdkPath = Path.Combine(basePath, version);
        return Directory.Exists(sdkPath) ? sdkPath : null;
    }
}
