using AlternativeBuild.SDK;
using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

public class SystemImageCommand : ICommand
{
    public string Name => "system-image";
    public string Description => "Manage Android System Images for AVD/Emulator";

    public async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length == 0 || args[0] == "list")
        {
            return ListInstalledImages();
        }

        switch (args[0])
        {
            case "install":
                if (args.Length < 2)
                {
                    ConsoleLogger.Error("Missing API level. Usage: -system-image install <api-level>");
                    return 1;
                }
                return await InstallSystemImageAsync(args[1]);

            case "browse":
                return await BrowseSystemImagesAsync();

            default:
                ConsoleLogger.Error($"Unknown action: {args[0]}");
                ShowHelp();
                return 1;
        }
    }

    private int ListInstalledImages()
    {
        ConsoleLogger.Header("INSTALLED SYSTEM IMAGES");
        Console.WriteLine();

        var androidHome = GetAndroidHome();
        if (string.IsNullOrEmpty(androidHome))
        {
            ConsoleLogger.Warning("ANDROID_HOME not set. No system images installed.");
            return 0;
        }

        var systemImagesPath = Path.Combine(androidHome, "system-images");
        if (!Directory.Exists(systemImagesPath))
        {
            ConsoleLogger.Info("No system images installed yet.");
            return 0;
        }

        var found = false;
        foreach (var apiDir in Directory.GetDirectories(systemImagesPath))
        {
            var apiName = Path.GetFileName(apiDir);
            foreach (var variantDir in Directory.GetDirectories(apiDir))
            {
                var variantName = Path.GetFileName(variantDir);
                foreach (var archDir in Directory.GetDirectories(variantDir))
                {
                    var archName = Path.GetFileName(archDir);
                    ConsoleLogger.Success($"  system-images;{apiName};{variantName};{archName}");
                    found = true;
                }
            }
        }

        if (!found)
        {
            ConsoleLogger.Info("No system images installed yet.");
        }

        return 0;
    }

    private async Task<int> BrowseSystemImagesAsync()
    {
        ConsoleLogger.Header("ANDROID SYSTEM IMAGES");
        Console.WriteLine();

        var images = VersionCatalog.SystemImages;

        for (int i = 0; i < images.Count && i < 10; i++)
        {
            var image = images[i];
            var prefix = image.IsRecommended ? "★" : " ";
            var color = image.IsRecommended ? ConsoleColor.Green : ConsoleColor.White;

            Console.ForegroundColor = color;
            Console.WriteLine($"{prefix} {i + 1}. {image.Description} ({image.Architecture})");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"      Package: {image.PackageName}");
            Console.ResetColor();
        }

        Console.WriteLine();
        Console.Write($"Select image (1-{Math.Min(images.Count, 10)}, or 0 to cancel): ");

        var input = Console.ReadLine();
        if (!int.TryParse(input, out int selection) || selection < 0 || selection > Math.Min(images.Count, 10))
        {
            ConsoleLogger.Error("Invalid selection");
            return 1;
        }

        if (selection == 0)
        {
            ConsoleLogger.Info("Cancelled");
            return 0;
        }

        var selectedImage = images[selection - 1];
        Console.WriteLine();
        ConsoleLogger.Info($"Installing {selectedImage.Description}...");
        ConsoleLogger.Warning("Download size: ~1-1.5 GB");

        return await InstallSystemImageAsync(selectedImage.ApiLevel, selectedImage.Variant, selectedImage.Architecture);
    }

    private async Task<int> InstallSystemImageAsync(string apiLevel, string variant = "google_apis", string arch = "x86_64")
    {
        var packageName = $"system-images;android-{apiLevel};{variant};{arch}";

        ConsoleLogger.Info($"Installing system image: {packageName}");
        Console.WriteLine();

        var androidHome = GetAndroidHome();
        if (string.IsNullOrEmpty(androidHome))
        {
            ConsoleLogger.Error("ANDROID_HOME not set. Please install Android SDK first:");
            ConsoleLogger.Info("  alternative -sdk browse");
            return 1;
        }

        var sdkManagerPath = FindSdkManager(androidHome);
        if (string.IsNullOrEmpty(sdkManagerPath))
        {
            ConsoleLogger.Error("sdkmanager not found. Please install Android SDK first.");
            return 1;
        }

        ConsoleLogger.Info("This may take 10-20 minutes depending on your connection...");
        Console.WriteLine();

        var result = await ProcessRunner.RunAsync(
            sdkManagerPath,
            $"\"{packageName}\"",
            Directory.GetCurrentDirectory(),
            null,
            line => Console.WriteLine(line),
            line => ConsoleLogger.Warning(line)
        );

        if (result.ExitCode == 0)
        {
            ConsoleLogger.Success($"System image installed successfully!");
            ConsoleLogger.Info($"Location: {Path.Combine(androidHome, "system-images", $"android-{apiLevel}", variant, arch)}");
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
        ConsoleLogger.Header("SYSTEM IMAGE COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe -system-image list                  List installed system images");
        Console.WriteLine("  alternative.exe -system-image install <api-level>   Install system image");
        Console.WriteLine("  alternative.exe -system-image browse                Browse and select image");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -system-image list");
        Console.WriteLine("  alternative.exe -system-image install 34");
        Console.WriteLine("  alternative.exe -system-image browse");
        Console.WriteLine();
        Console.WriteLine("NOTE: Requires ANDROID_HOME to be set (install SDK first)");
    }
}
