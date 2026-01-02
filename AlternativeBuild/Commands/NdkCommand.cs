using AlternativeBuild.SDK;
using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

public class NdkCommand : ICommand
{
    public string Name => "ndk";
    public string Description => "Manage Android NDK installations";

    private readonly SdkManager _sdkManager = new();

    public async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length == 0 || args[0] == "list")
        {
            _sdkManager.ListInstalled();
            return 0;
        }

        switch (args[0])
        {
            case "install":
                if (args.Length < 2)
                {
                    ConsoleLogger.Error("Missing version. Usage: -ndk install <version>");
                    return 1;
                }
                var success = await _sdkManager.InstallNdkAsync(args[1]);
                return success ? 0 : 1;

            case "browse":
                return await BrowseVersionsAsync();

            default:
                ConsoleLogger.Error($"Unknown NDK action: {args[0]}");
                ShowHelp();
                return 1;
        }
    }

    private async Task<int> BrowseVersionsAsync()
    {
        ConsoleLogger.Header("ANDROID NDK VERSIONS");
        Console.WriteLine();

        var versions = VersionCatalog.NdkVersions;

        for (int i = 0; i < versions.Count && i < 10; i++)
        {
            var version = versions[i];
            var prefix = version.IsRecommended ? "★" : " ";
            var color = version.IsRecommended ? ConsoleColor.Green : ConsoleColor.White;

            Console.ForegroundColor = color;
            Console.WriteLine($"{prefix} {i + 1}. NDK {version.Version} - {version.Description}");
            Console.ResetColor();
        }

        Console.WriteLine();
        Console.Write($"Select version (1-{Math.Min(versions.Count, 10)}, or 0 to cancel): ");

        var input = Console.ReadLine();
        if (!int.TryParse(input, out int selection) || selection < 0 || selection > Math.Min(versions.Count, 10))
        {
            ConsoleLogger.Error("Invalid selection");
            return 1;
        }

        if (selection == 0)
        {
            ConsoleLogger.Info("Cancelled");
            return 0;
        }

        var selectedVersion = versions[selection - 1];
        Console.WriteLine();
        ConsoleLogger.Info($"Installing Android NDK {selectedVersion.Version}...");

        var success = await _sdkManager.InstallNdkAsync(selectedVersion.Version);
        return success ? 0 : 1;
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("ANDROID NDK COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe -ndk list                 List installed NDKs");
        Console.WriteLine("  alternative.exe -ndk install <version>    Install Android NDK");
        Console.WriteLine("  alternative.exe -ndk browse               Browse and select NDK version");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -ndk list");
        Console.WriteLine("  alternative.exe -ndk install 26.1.10909125");
        Console.WriteLine("  alternative.exe -ndk browse");
    }
}
