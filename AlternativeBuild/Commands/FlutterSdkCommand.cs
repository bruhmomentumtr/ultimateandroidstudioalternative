using AlternativeBuild.SDK;
using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

public class FlutterSdkCommand : ICommand
{
    public string Name => "flutter-sdk";
    public string Description => "Manage Flutter SDK installations";

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
                    ConsoleLogger.Error("Missing version. Usage: --flutter-sdk install <version>");
                    return 1;
                }
                var success = await _sdkManager.InstallFlutterSdkAsync(args[1]);
                return success ? 0 : 1;

            case "browse":
                return await BrowseVersionsAsync();

            default:
                ConsoleLogger.Error($"Unknown Flutter SDK action: {args[0]}");
                ShowHelp();
                return 1;
        }
    }

    private async Task<int> BrowseVersionsAsync()
    {
        ConsoleLogger.Header("FLUTTER SDK VERSIONS");
        Console.WriteLine();

        var versions = VersionCatalog.FlutterVersions;

        for (int i = 0; i < versions.Count && i < 10; i++)
        {
            var version = versions[i];
            var prefix = version.IsRecommended ? "★" : " ";
            var color = version.IsRecommended ? ConsoleColor.Green : ConsoleColor.White;

            Console.ForegroundColor = color;
            Console.WriteLine($"{prefix} {i + 1}. Flutter {version.Version} - {version.Description}");
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
        ConsoleLogger.Info($"Installing Flutter SDK {selectedVersion.Version}...");

        var success = await _sdkManager.InstallFlutterSdkAsync(selectedVersion.Version);
        return success ? 0 : 1;
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("FLUTTER SDK COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe --flutter-sdk list                List installed Flutter SDKs");
        Console.WriteLine("  alternative.exe --flutter-sdk install <version>   Install Flutter SDK");
        Console.WriteLine("  alternative.exe --flutter-sdk browse              Browse and select Flutter version");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe --flutter-sdk list");
        Console.WriteLine("  alternative.exe --flutter-sdk install 3.19.0");
        Console.WriteLine("  alternative.exe --flutter-sdk browse");
    }
}
