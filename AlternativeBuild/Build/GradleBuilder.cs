using AlternativeBuild.SDK;
using AlternativeBuild.Utils;
using System.Text.RegularExpressions;

namespace AlternativeBuild.Build;

/// <summary>
/// Handles Gradle-based Android builds (Kotlin/Java)
/// </summary>
public class GradleBuilder
{
    private readonly SdkManager _sdkManager = new();

    public async Task<bool> BuildDebugAsync(string projectPath)
    {
        ConsoleLogger.Header("Building Android Debug APK");

        // Auto-detect and install required SDK/NDK
        await EnsureRequiredSdksAsync(projectPath);

        return await RunGradleAsync(projectPath, "assembleDebug");
    }

    public async Task<bool> BuildReleaseAsync(string projectPath)
    {
        ConsoleLogger.Header("Building Android Release APK");

        // Auto-detect and install required SDK/NDK
        await EnsureRequiredSdksAsync(projectPath);

        return await RunGradleAsync(projectPath, "assembleRelease");
    }

    public async Task<bool> BuildSignedAsync(
        string projectPath,
        string keystorePath,
        string? storePassword = null,
        string? keyPassword = null,
        string? alias = null)
    {
        ConsoleLogger.Header("Building Signed Android APK");

        if (!File.Exists(keystorePath))
        {
            ConsoleLogger.Error($"Keystore not found: {keystorePath}");
            return false;
        }

        // Build with signing config
        var args = "assembleRelease";

        if (!string.IsNullOrEmpty(storePassword))
            args += $" -Pandroid.injected.signing.store.password={storePassword}";
        if (!string.IsNullOrEmpty(keyPassword))
            args += $" -Pandroid.injected.signing.key.password={keyPassword}";
        if (!string.IsNullOrEmpty(alias))
            args += $" -Pandroid.injected.signing.key.alias={alias}";

        args += $" -Pandroid.injected.signing.store.file={keystorePath}";

        return await RunGradleAsync(projectPath, args);
    }

    public async Task<bool> CleanAsync(string projectPath)
    {
        ConsoleLogger.Header("Cleaning Android Project");
        return await RunGradleAsync(projectPath, "clean");
    }

    private async Task<bool> RunGradleAsync(string projectPath, string arguments)
    {
        // Find gradlew
        var gradlewPath = Path.Combine(projectPath, "gradlew.bat");
        if (!File.Exists(gradlewPath))
        {
            gradlewPath = Path.Combine(projectPath, "gradlew");
            if (!File.Exists(gradlewPath))
            {
                ConsoleLogger.Error("Gradle wrapper not found (gradlew.bat or gradlew)");
                return false;
            }
        }

        // Set up environment variables
        var env = new Dictionary<string, string>();

        // Try to find Android SDK
        var androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");
        if (string.IsNullOrEmpty(androidHome))
        {
            androidHome = _sdkManager.AndroidSdkPath;
            if (Directory.Exists(androidHome))
            {
                env["ANDROID_HOME"] = androidHome;
                ConsoleLogger.Info($"Using SDK: {androidHome}");
            }
            else
            {
                ConsoleLogger.Warning("ANDROID_HOME not set and SDK not found");
                ConsoleLogger.Info("You may need to install Android SDK using: alternative.exe -sdk install <version>");
            }
        }

        ConsoleLogger.Info($"Running: gradlew {arguments}");
        Console.WriteLine();

        var result = await ProcessRunner.RunAsync(
            gradlewPath,
            arguments,
            projectPath,
            env,
            line => ConsoleLogger.Progress(line),
            line => ConsoleLogger.Warning(line)
        );

        Console.WriteLine();

        if (result.Success)
        {
            ConsoleLogger.Success("Build completed successfully!");

            // Try to find and display APK location
            FindAndDisplayApk(projectPath);
            return true;
        }
        else
        {
            ConsoleLogger.Error($"Build failed with exit code {result.ExitCode}");
            return false;
        }
    }

    private async Task EnsureRequiredSdksAsync(string projectPath)
    {
        ConsoleLogger.Info("Checking project requirements...");

        // Detect required compileSdk
        var compileSdk = ProjectInspector.GetRequiredCompileSdk(projectPath);
        if (compileSdk.HasValue)
        {
            ConsoleLogger.Info($"Project requires compileSdk: {compileSdk.Value}");

            // Check if SDK is installed
            var sdkPath = _sdkManager.GetSdkPath("android", $"cmdline-tools-{GetSdkVersionString(compileSdk.Value)}");

            if (sdkPath == null)
            {
                ConsoleLogger.Warning($"Android SDK {compileSdk.Value} not found");
                ConsoleLogger.Info($"Attempting to download Android SDK...");

                var sdkVersionString = GetSdkVersionString(compileSdk.Value);
                var success = await _sdkManager.InstallAndroidSdkAsync(sdkVersionString);

                if (!success)
                {
                    ConsoleLogger.Warning("Failed to auto-install SDK. Build may fail.");
                    ConsoleLogger.Info($"You can manually install with: alternative.exe -sdk install {sdkVersionString}");
                }
            }
            else
            {
                ConsoleLogger.Success($"Android SDK {compileSdk.Value} is installed");
            }
        }

        // Detect required NDK
        var ndkVersion = ProjectInspector.GetRequiredNdkVersion(projectPath);
        if (!string.IsNullOrEmpty(ndkVersion))
        {
            ConsoleLogger.Info($"Project requires NDK: {ndkVersion}");

            var ndkPath = _sdkManager.GetSdkPath("ndk", ndkVersion);

            if (ndkPath == null)
            {
                ConsoleLogger.Warning($"NDK {ndkVersion} not found");
                ConsoleLogger.Info($"Attempting to download NDK...");

                var success = await _sdkManager.InstallNdkAsync(ndkVersion);

                if (!success)
                {
                    ConsoleLogger.Warning("Failed to auto-install NDK. Build may fail if NDK is required.");
                    ConsoleLogger.Info($"You can manually install with: alternative.exe -ndk install {ndkVersion}");
                }
            }
            else
            {
                ConsoleLogger.Success($"NDK {ndkVersion} is installed");
            }
        }

        Console.WriteLine();
    }

    private string GetSdkVersionString(int compileSdk)
    {
        // Map compileSdk version to commandlinetools version
        // This is approximate - may need adjustment
        return compileSdk switch
        {
            >= 34 => "11076708", // Latest as of 2024
            >= 33 => "10406996",
            >= 31 => "9477386",
            _ => "9477386"
        };
    }

    private void FindAndDisplayApk(string projectPath)
    {
        try
        {
            var buildDir = Path.Combine(projectPath, "app", "build", "outputs", "apk");
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
