using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

/// <summary>
/// AVD (Android Virtual Device) management command
/// </summary>
public class AvdCommand : ICommand
{
    public string Name => "avd";
    public string Description => "Manage Android Virtual Devices (Emulators)";

    public async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length == 0 || args[0] == "list")
        {
            return await ListAvds();
        }

        switch (args[0])
        {
            case "create":
                if (args.Length < 3)
                {
                    ConsoleLogger.Error("Usage: -avd create <name> <api-level>");
                    return 1;
                }
                return await CreateAvd(args[1], args[2]);

            case "start":
                if (args.Length < 2)
                {
                    ConsoleLogger.Error("Usage: -avd start <name>");
                    return 1;
                }
                return await StartAvd(args[1]);

            case "stop":
                return await StopAvd();

            case "delete":
                if (args.Length < 2)
                {
                    ConsoleLogger.Error("Usage: -avd delete <name>");
                    return 1;
                }
                return await DeleteAvd(args[1]);

            case "setup":
                return await SetupAvdAutomaticAsync(args);

            default:
                ConsoleLogger.Error($"Unknown action: {args[0]}");
                ShowHelp();
                return 1;
        }
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("AVD COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative -avd list                      List all AVDs");
        Console.WriteLine("  alternative -avd create <name> <api>       Create new AVD");
        Console.WriteLine("  alternative -avd start <name>              Start AVD");
        Console.WriteLine("  alternative -avd stop                      Stop running emulator");
        Console.WriteLine("  alternative -avd delete <name>             Delete AVD");
        Console.WriteLine("  alternative -avd setup --api <api> --name <name>  Auto-setup complete AVD");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative -avd list");
        Console.WriteLine("  alternative -avd create Pixel_5 34");
        Console.WriteLine("  alternative -avd start Pixel_5");
        Console.WriteLine("  alternative -avd stop");
        Console.WriteLine("  alternative -avd delete Pixel_5");
        Console.WriteLine("  alternative -avd setup --api 34 --name Pixel_5");
        Console.WriteLine();
        Console.WriteLine("NOTE:");
        Console.WriteLine("  'setup' automatically installs system image and emulator if needed.");
        Console.WriteLine("  Use: alternative -sdk install <version>");
    }

    private async Task<int> ListAvds()
    {
        ConsoleLogger.Header("AVAILABLE AVDs");

        var avdmanager = FindAvdManager();
        if (string.IsNullOrEmpty(avdmanager))
        {
            ConsoleLogger.Error("avdmanager not found. Please install Android SDK.");
            ConsoleLogger.Info("Set ANDROID_HOME environment variable or install SDK using:");
            ConsoleLogger.Info("  alternative -sdk install <version>");
            return 1;
        }

        // List existing AVDs
        ConsoleLogger.Info("Installed AVDs:");
        Console.WriteLine();

        var result = await ProcessRunner.RunAsync(
            avdmanager,
            "list avd",
            Directory.GetCurrentDirectory(),
            null,
            line => Console.WriteLine(line),
            line => ConsoleLogger.Warning(line)
        );

        Console.WriteLine();

        // List available system images from ANDROID_HOME
        var androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");
        if (!string.IsNullOrEmpty(androidHome))
        {
            var systemImagesPath = Path.Combine(androidHome, "system-images");
            if (Directory.Exists(systemImagesPath))
            {
                ConsoleLogger.Info("Available System Images (for creating new AVDs):");
                Console.WriteLine();

                var apiLevels = Directory.GetDirectories(systemImagesPath);
                foreach (var apiDir in apiLevels)
                {
                    var apiName = Path.GetFileName(apiDir); // e.g., android-34
                    var variants = Directory.GetDirectories(apiDir);

                    foreach (var variantDir in variants)
                    {
                        var variantName = Path.GetFileName(variantDir); // e.g., google_apis
                        var archs = Directory.GetDirectories(variantDir);

                        foreach (var archDir in archs)
                        {
                            var archName = Path.GetFileName(archDir); // e.g., x86_64
                            ConsoleLogger.Progress($"  system-images;{apiName};{variantName};{archName}");
                        }
                    }
                }

                Console.WriteLine();
                ConsoleLogger.Info("To create AVD: alternative -avd create <name> <api-level>");
            }
        }

        return result.ExitCode;
    }

    private async Task<int> CreateAvd(string name, string apiLevel)
    {
        ConsoleLogger.Header($"Creating AVD: {name} (API {apiLevel})");

        var avdmanager = FindAvdManager();
        if (string.IsNullOrEmpty(avdmanager))
        {
            ConsoleLogger.Error("avdmanager not found.");
            return 1;
        }

        var systemImage = $"system-images;android-{apiLevel};google_apis;{GetArchitecture()}";

        ConsoleLogger.Info($"Creating AVD with system image: {systemImage}");

        // Create AVD
        var args = $"create avd -n {name} -k \"{systemImage}\" --force";

        var result = await ProcessRunner.RunAsync(
            avdmanager,
            args,
            Directory.GetCurrentDirectory(),
            null,
            line => Console.WriteLine(line),
            line => ConsoleLogger.Warning(line)
        );

        if (result.Success)
        {
            ConsoleLogger.Success($"AVD '{name}' created successfully");
        }

        return result.ExitCode;
    }

    private async Task<int> StartAvd(string name)
    {
        ConsoleLogger.Header($"Starting AVD: {name}");

        var emulator = FindEmulator();
        if (string.IsNullOrEmpty(emulator))
        {
            ConsoleLogger.Error("Emulator not found. Please install Android SDK emulator.");
            return 1;
        }

        ConsoleLogger.Info($"Launching emulator (this may take a while)...");

        var result = await ProcessRunner.RunAsync(
            emulator,
            $"-avd {name}",
            Directory.GetCurrentDirectory(),
            null,
            line => ConsoleLogger.Progress(line),
            line => ConsoleLogger.Warning(line)
        );

        return result.ExitCode;
    }

    private async Task<int> StopAvd()
    {
        ConsoleLogger.Header("Stopping Emulator");

        var adb = GetAdbPath();
        if (string.IsNullOrEmpty(adb))
        {
            ConsoleLogger.Error("ADB not found.");
            return 1;
        }

        var result = await ProcessRunner.RunAsync(
            adb,
            "emu kill",
            Directory.GetCurrentDirectory()
        );

        if (result.Success)
        {
            ConsoleLogger.Success("Emulator stopped");
        }

        return result.ExitCode;
    }

    private async Task<int> DeleteAvd(string name)
    {
        ConsoleLogger.Header($"Deleting AVD: {name}");

        var avdmanager = FindAvdManager();
        if (string.IsNullOrEmpty(avdmanager))
        {
            ConsoleLogger.Error("avdmanager not found.");
            return 1;
        }

        var result = await ProcessRunner.RunAsync(
            avdmanager,
            $"delete avd -n {name}",
            Directory.GetCurrentDirectory(),
            null,
            line => Console.WriteLine(line),
            line => ConsoleLogger.Warning(line)
        );

        if (result.Success)
        {
            ConsoleLogger.Success($"AVD '{name}' deleted");
        }

        return result.ExitCode;
    }

    private string? FindAvdManager()
    {
        var androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");

        if (!string.IsNullOrEmpty(androidHome))
        {
            var avdmanager = Path.Combine(androidHome, "cmdline-tools", "latest", "bin", $"avdmanager{PlatformHelper.ScriptExtension}");
            if (File.Exists(avdmanager))
            {
                PlatformHelper.MakeExecutable(avdmanager);
                return avdmanager;
            }
        }

        // Try default location
        var defaultPath = PlatformHelper.GetDefaultAndroidHome();
        if (!string.IsNullOrEmpty(defaultPath))
        {
            var avdmanager = Path.Combine(defaultPath, "cmdline-tools", "latest", "bin", $"avdmanager{PlatformHelper.ScriptExtension}");
            if (File.Exists(avdmanager))
            {
                PlatformHelper.MakeExecutable(avdmanager);
                return avdmanager;
            }
        }

        return null;
    }

    private string? FindEmulator()
    {
        var androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");

        if (!string.IsNullOrEmpty(androidHome))
        {
            var emulator = Path.Combine(androidHome, "emulator", $"emulator{PlatformHelper.ExecutableExtension}");
            if (File.Exists(emulator))
            {
                PlatformHelper.MakeExecutable(emulator);
                return emulator;
            }
        }

        return null;
    }

    private string? GetAdbPath()
    {
        var androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");

        if (!string.IsNullOrEmpty(androidHome))
        {
            var adb = Path.Combine(androidHome, "platform-tools", $"adb{PlatformHelper.ExecutableExtension}");
            if (File.Exists(adb))
            {
                PlatformHelper.MakeExecutable(adb);
                return adb;
            }
        }

        return null;
    }

    private string GetArchitecture()
    {
        var arch = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;
        return arch switch
        {
            System.Runtime.InteropServices.Architecture.X64 => "x86_64",
            System.Runtime.InteropServices.Architecture.Arm64 => "arm64-v8a",
            _ => "x86_64"
        };
    }

    private async Task<int> SetupAvdAutomaticAsync(string[] args)
    {
        ConsoleLogger.Header("✨ AVD AUTO-SETUP");
        Console.WriteLine();

        // Parse arguments
        string? apiLevel = null;
        string? name = null;

        for (int i = 1; i < args.Length; i++)
        {
            if (args[i] == "--api" && i + 1 < args.Length)
            {
                apiLevel = args[i + 1];
                i++;
            }
            else if (args[i] == "--name" && i + 1 < args.Length)
            {
                name = args[i + 1];
                i++;
            }
        }

        if (string.IsNullOrEmpty(apiLevel) || string.IsNullOrEmpty(name))
        {
            ConsoleLogger.Error("Missing required arguments");
            ConsoleLogger.Info("Usage: alternative -avd setup --api <api-level> --name <avd-name>");
            ConsoleLogger.Info("Example: alternative -avd setup --api 34 --name Pixel_5");
            return 1;
        }

        ConsoleLogger.Info($"Setting up AVD: {name} with API {apiLevel}");
        ConsoleLogger.Info("This will automatically install missing components...");
        Console.WriteLine();

        // 1. Check system image
        ConsoleLogger.Header("Step 1: Checking System Image...");
        if (!HasSystemImage(apiLevel))
        {
            ConsoleLogger.Warning($"System image for API {apiLevel} not found");
            ConsoleLogger.Info("Installing system image (this may take 10-20 minutes)...");

            var systemImageCmd = new SystemImageCommand();
            var result = await systemImageCmd.ExecuteAsync(new[] { "install", apiLevel });

            if (result != 0)
            {
                ConsoleLogger.Error("Failed to install system image");
                return result;
            }
        }
        else
        {
            ConsoleLogger.Success("✓ System image already installed");
        }
        Console.WriteLine();

        // 2. Check emulator
        ConsoleLogger.Header("Step 2: Checking Emulator...");
        if (!HasEmulator())
        {
            ConsoleLogger.Warning("Emulator not found");
            ConsoleLogger.Info("Installing emulator (this may take 5-10 minutes)...");

            var emulatorCmd = new EmulatorCommand();
            var result = await emulatorCmd.ExecuteAsync(new[] { "install" });

            if (result != 0)
            {
                ConsoleLogger.Error("Failed to install emulator");
                return result;
            }
        }
        else
        {
            ConsoleLogger.Success("✓ Emulator already installed");
        }
        Console.WriteLine();

        // 3. Create AVD
        ConsoleLogger.Header("Step 3: Creating AVD...");
        var createResult = await CreateAvd(name, apiLevel);
        if (createResult != 0)
        {
            ConsoleLogger.Error("Failed to create AVD");
            return createResult;
        }
        Console.WriteLine();

        // 4. Ask if user wants to start it
        ConsoleLogger.Success("✨ AVD setup complete!");
        Console.WriteLine();
        Console.Write("Start emulator now? (Y/n): ");
        var response = Console.ReadLine()?.ToLower();

        if (string.IsNullOrEmpty(response) || response == "y" || response == "yes")
        {
            Console.WriteLine();
            return await StartAvd(name);
        }

        ConsoleLogger.Info($"You can start it later with: alternative -avd start {name}");
        return 0;
    }

    private bool HasSystemImage(string apiLevel)
    {
        var androidHome = Environment.GetEnvironmentVariable("ANDROID_HOME");
        if (string.IsNullOrEmpty(androidHome))
            return false;

        var systemImagePath = Path.Combine(androidHome, "system-images", $"android-{apiLevel}", "google_apis", GetArchitecture());
        return Directory.Exists(systemImagePath);
    }

    private bool HasEmulator()
    {
        return !string.IsNullOrEmpty(FindEmulator());
    }
}
