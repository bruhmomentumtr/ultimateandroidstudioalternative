using AlternativeBuild.SDK;
using AlternativeBuild.Utils;

namespace AlternativeBuild.Build;

/// <summary>
/// Handles Flutter Android builds
/// </summary>
public class FlutterBuilder
{
    private readonly SdkManager _sdkManager = new();

    public async Task<bool> BuildDebugAsync(string projectPath)
    {
        ConsoleLogger.Header("Building Flutter Debug APK");
        return await RunFlutterAsync(projectPath, "build apk --debug");
    }

    public async Task<bool> BuildReleaseAsync(string projectPath)
    {
        ConsoleLogger.Header("Building Flutter Release APK");
        return await RunFlutterAsync(projectPath, "build apk --release");
    }

    public async Task<bool> CleanAsync(string projectPath)
    {
        ConsoleLogger.Header("Cleaning Flutter Project");
        return await RunFlutterAsync(projectPath, "clean");
    }

    private async Task<bool> RunFlutterAsync(string projectPath, string arguments)
    {
        // Try to find Flutter
        var flutterPath = FindFlutter();

        if (string.IsNullOrEmpty(flutterPath))
        {
            ConsoleLogger.Error("Flutter not found in PATH");
            ConsoleLogger.Info("You may need to install Flutter SDK using: alternative.exe --flutter-sdk install <version>");
            ConsoleLogger.Info("Or add Flutter to your PATH environment variable");
            return false;
        }

        ConsoleLogger.Info($"Using Flutter: {flutterPath}");
        ConsoleLogger.Info($"Running: flutter {arguments}");
        Console.WriteLine();

        var result = await ProcessRunner.RunAsync(
            flutterPath,
            arguments,
            projectPath,
            null,
            line => ConsoleLogger.Progress(line),
            line => ConsoleLogger.Warning(line)
        );

        Console.WriteLine();

        if (result.Success)
        {
            ConsoleLogger.Success("Build completed successfully!");

            // Display APK location
            FindAndDisplayApk(projectPath);
            return true;
        }
        else
        {
            ConsoleLogger.Error($"Build failed with exit code {result.ExitCode}");
            return false;
        }
    }

    private string? FindFlutter()
    {
        // First, check PATH
        var pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (!string.IsNullOrEmpty(pathEnv))
        {
            var paths = pathEnv.Split(';');
            foreach (var path in paths)
            {
                var flutterExe = Path.Combine(path, "flutter.bat");
                if (File.Exists(flutterExe))
                {
                    return flutterExe;
                }
            }
        }

        // Check installed Flutter SDKs
        var flutterSdkPath = _sdkManager.FlutterSdkPath;
        if (Directory.Exists(flutterSdkPath))
        {
            var versions = Directory.GetDirectories(flutterSdkPath);
            if (versions.Length > 0)
            {
                // Use the first (or latest) version
                var flutterBin = Path.Combine(versions[0], "flutter", "bin", "flutter.bat");
                if (File.Exists(flutterBin))
                {
                    return flutterBin;
                }
            }
        }

        return null;
    }

    private void FindAndDisplayApk(string projectPath)
    {
        try
        {
            var buildDir = Path.Combine(projectPath, "build", "app", "outputs", "flutter-apk");
            if (Directory.Exists(buildDir))
            {
                var apkFiles = Directory.GetFiles(buildDir, "*.apk", SearchOption.AllDirectories);
                if (apkFiles.Length > 0)
                {
                    ConsoleLogger.Info("APK files:");
                    foreach (var apk in apkFiles)
                    {
                        var fileInfo = new FileInfo(apk);
                        var sizeMb = fileInfo.Length / (1024.0 * 1024.0);
                        ConsoleLogger.Progress($"  {apk} ({sizeMb:F2} MB)");
                    }
                }
            }
        }
        catch
        {
            // Ignore errors in APK display
        }
    }
}
