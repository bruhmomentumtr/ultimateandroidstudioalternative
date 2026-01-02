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

        if (args[0] == "install")
        {
            if (args.Length < 2)
            {
                ConsoleLogger.Error("Missing version. Usage: --flutter-sdk install <version>");
                return 1;
            }

            var success = await _sdkManager.InstallFlutterSdkAsync(args[1]);
            return success ? 0 : 1;
        }

        ConsoleLogger.Error($"Unknown Flutter SDK action: {args[0]}");
        ShowHelp();
        return 1;
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("FLUTTER SDK COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe --flutter-sdk list                List installed Flutter SDKs");
        Console.WriteLine("  alternative.exe --flutter-sdk install <version>   Install Flutter SDK");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe --flutter-sdk list");
        Console.WriteLine("  alternative.exe --flutter-sdk install 3.19.0");
        Console.WriteLine("  alternative.exe --flutter-sdk install 3.16.5");
        Console.WriteLine();
        Console.WriteLine("For available versions, visit:");
        Console.WriteLine("  https://flutter.dev/docs/development/tools/sdk/releases");
    }
}
