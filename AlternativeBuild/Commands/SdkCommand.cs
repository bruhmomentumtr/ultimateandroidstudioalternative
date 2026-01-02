using AlternativeBuild.SDK;
using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

public class SdkCommand : ICommand
{
    public string Name => "sdk";
    public string Description => "Manage Android SDK installations";

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
                ConsoleLogger.Error("Missing version. Usage: -sdk install <version>");
                return 1;
            }

            var success = await _sdkManager.InstallAndroidSdkAsync(args[1]);
            return success ? 0 : 1;
        }

        ConsoleLogger.Error($"Unknown SDK action: {args[0]}");
        ShowHelp();
        return 1;
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("ANDROID SDK COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe -sdk list                 List installed Android SDKs");
        Console.WriteLine("  alternative.exe -sdk install <version>    Install Android SDK");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -sdk list");
        Console.WriteLine("  alternative.exe -sdk install 10406996");
        Console.WriteLine("  alternative.exe -sdk install android-34");
        Console.WriteLine();
        Console.WriteLine("For available versions, visit:");
        Console.WriteLine("  https://developer.android.com/studio#command-tools");
    }
}
