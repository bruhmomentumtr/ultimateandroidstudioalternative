using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

public class EmulatorCommand : ICommand
{
    public string Name => "emulator";
    public string Description => "Manage Android Emulator binary";

    public async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length == 0 || args[0] == "check")
        {
            return CheckEmulator();
        }

        switch (args[0])
        {
            case "install":
                return await InstallEmulatorAsync();

            default:
                ConsoleLogger.Error($"Unknown action: {args[0]}");
                ShowHelp();
                return 1;
        }
    }

    private int CheckEmulator()
    {
        ConsoleLogger.Header("EMULATOR STATUS");
        Console.WriteLine();

        var androidHome = GetAndroidHome();
        if (string.IsNullOrEmpty(androidHome))
        {
            ConsoleLogger.Warning("ANDROID_HOME not set");
            ConsoleLogger.Info("Install Android SDK first: alternative -sdk browse");
            return 1;
        }

        var emulatorPath = GetEmulatorPath(androidHome);
        if (!string.IsNullOrEmpty(emulatorPath) && File.Exists(emulatorPath))
        {
            ConsoleLogger.Success($"✓ Emulator installed");
            ConsoleLogger.Info($"  Location: {emulatorPath}");

            // Try to get version
            try
            {
                var result = ProcessRunner.RunAsync(
                    emulatorPath,
                    "-version",
                    Directory.GetCurrentDirectory(),
                    null,
                    line => Console.WriteLine($"  {line}"),
                    line => { }
                ).Result;
            }
            catch
            {
                // Version check failed, but emulator exists
            }

            return 0;
        }
        else
        {
            ConsoleLogger.Warning("✗ Emulator not installed");
            ConsoleLogger.Info("Install with: alternative -emulator install");
            return 1;
        }
    }

    private async Task<int> InstallEmulatorAsync()
    {
        ConsoleLogger.Header("INSTALLING EMULATOR");
        Console.WriteLine();

        var androidHome = GetAndroidHome();
        if (string.IsNullOrEmpty(androidHome))
        {
            ConsoleLogger.Error("ANDROID_HOME not set. Please install Android SDK first:");
            ConsoleLogger.Info("  alternative -sdk browse");
            return 1;
        }

        // Check if already installed
        var emulatorPath = GetEmulatorPath(androidHome);
        if (!string.IsNullOrEmpty(emulatorPath) && File.Exists(emulatorPath))
        {
            ConsoleLogger.Warning("Emulator already installed");
            ConsoleLogger.Info($"Location: {emulatorPath}");
            Console.WriteLine();
            Console.Write("Reinstall? (y/N): ");
            var response = Console.ReadLine()?.ToLower();
            if (response != "y" && response != "yes")
            {
                ConsoleLogger.Info("Cancelled");
                return 0;
            }
        }

        var sdkManagerPath = FindSdkManager(androidHome);
        if (string.IsNullOrEmpty(sdkManagerPath))
        {
            ConsoleLogger.Error("sdkmanager not found");
            return 1;
        }

        ConsoleLogger.Info("Downloading emulator (~150-200 MB)...");
        ConsoleLogger.Info("This may take 5-10 minutes...");
        Console.WriteLine();

        var result = await ProcessRunner.RunAsync(
            sdkManagerPath,
            "\"emulator\"",
            Directory.GetCurrentDirectory(),
            null,
            line => Console.WriteLine(line),
            line => ConsoleLogger.Warning(line)
        );

        if (result.ExitCode == 0)
        {
            ConsoleLogger.Success("Emulator installed successfully!");

            emulatorPath = GetEmulatorPath(androidHome);
            if (!string.IsNullOrEmpty(emulatorPath))
            {
                ConsoleLogger.Info($"Location: {emulatorPath}");
            }
        }
        else
        {
            ConsoleLogger.Error("Installation failed");
        }

        return result.ExitCode;
    }

    private string? GetAndroidHome()
    {
        return Environment.GetEnvironmentVariable("ANDROID_HOME")
            ?? Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
    }

    private string? GetEmulatorPath(string androidHome)
    {
        var emulatorBinary = PlatformHelper.IsWindows ? "emulator.exe" : "emulator";
        var emulatorPath = Path.Combine(androidHome, "emulator", emulatorBinary);

        if (File.Exists(emulatorPath))
            return emulatorPath;

        return null;
    }

    private string? FindSdkManager(string androidHome)
    {
        var cmdlineTools = Path.Combine(androidHome, "cmdline-tools", "latest", "bin",
            PlatformHelper.IsWindows ? "sdkmanager.bat" : "sdkmanager");

        if (File.Exists(cmdlineTools))
            return cmdlineTools;

        // Try finding any version
        var cmdlineToolsRoot = Path.Combine(androidHome, "cmdline-tools");
        if (Directory.Exists(cmdlineToolsRoot))
        {
            foreach (var versionDir in Directory.GetDirectories(cmdlineToolsRoot))
            {
                var sdkManager = Path.Combine(versionDir, "bin",
                    PlatformHelper.IsWindows ? "sdkmanager.bat" : "sdkmanager");
                if (File.Exists(sdkManager))
                    return sdkManager;
            }
        }

        return null;
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("EMULATOR COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe -emulator check      Check if emulator is installed");
        Console.WriteLine("  alternative.exe -emulator install    Install Android Emulator");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -emulator check");
        Console.WriteLine("  alternative.exe -emulator install");
        Console.WriteLine();
        Console.WriteLine("NOTE: Requires ANDROID_HOME to be set (install SDK first)");
        Console.WriteLine("      Download size: ~150-200 MB");
    }
}
