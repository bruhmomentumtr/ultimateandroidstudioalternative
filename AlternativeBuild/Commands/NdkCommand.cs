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

        if (args[0] == "install")
        {
            if (args.Length < 2)
            {
                ConsoleLogger.Error("Missing version. Usage: -ndk install <version>");
                return 1;
            }

            var success = await _sdkManager.InstallNdkAsync(args[1]);
            return success ? 0 : 1;
        }

        ConsoleLogger.Error($"Unknown NDK action: {args[0]}");
        ShowHelp();
        return 1;
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("ANDROID NDK COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe -ndk list                 List installed NDKs");
        Console.WriteLine("  alternative.exe -ndk install <version>    Install Android NDK");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -ndk list");
        Console.WriteLine("  alternative.exe -ndk install 26.1.10909125");
        Console.WriteLine("  alternative.exe -ndk install 25c");
        Console.WriteLine();
        Console.WriteLine("For available versions, visit:");
        Console.WriteLine("  https://developer.android.com/ndk/downloads");
    }
}
