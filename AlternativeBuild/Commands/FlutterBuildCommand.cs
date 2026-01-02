using AlternativeBuild.Build;
using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

public class FlutterBuildCommand : ICommand
{
    public string Name => "flutter";
    public string Description => "Build Flutter Android projects";

    public async Task<int> ExecuteAsync(string[] args)
    {
        var action = args.Length > 0 ? args[0] : "build";
        var projectPath = Directory.GetCurrentDirectory();

        // Check if it's a valid Flutter project
        if (!File.Exists(Path.Combine(projectPath, "pubspec.yaml")))
        {
            ConsoleLogger.Error("Not a Flutter project (no pubspec.yaml found)");
            return 1;
        }

        var builder = new FlutterBuilder();

        switch (action)
        {
            case "build":
            case "build-debug":
                return await builder.BuildDebugAsync(projectPath) ? 0 : 1;

            case "build-release":
                return await builder.BuildReleaseAsync(projectPath) ? 0 : 1;

            case "clean":
                return await builder.CleanAsync(projectPath) ? 0 : 1;

            default:
                ConsoleLogger.Error($"Unknown action: {action}");
                ShowHelp();
                return 1;
        }
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("FLUTTER BUILD COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe -flutter build          Build debug APK");
        Console.WriteLine("  alternative.exe -flutter build-debug    Build debug APK");
        Console.WriteLine("  alternative.exe -flutter build-release  Build release APK");
        Console.WriteLine("  alternative.exe -flutter clean          Clean build");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -flutter build");
        Console.WriteLine("  alternative.exe -flutter build-release");
    }
}
