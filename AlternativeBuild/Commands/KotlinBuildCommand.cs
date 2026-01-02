using AlternativeBuild.Build;
using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

public class KotlinBuildCommand : ICommand
{
    public string Name => "kotlin";
    public string Description => "Build Kotlin/Java Android projects";

    public async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length == 0)
        {
            ConsoleLogger.Error("Missing action. Usage: -kotlin <action>");
            ShowHelp();
            return 1;
        }

        var action = args[0];
        var projectPath = Directory.GetCurrentDirectory();

        // Check if it's a valid Android project
        if (!File.Exists(Path.Combine(projectPath, "build.gradle")) &&
            !File.Exists(Path.Combine(projectPath, "build.gradle.kts")))
        {
            ConsoleLogger.Error("Not an Android project (no build.gradle found)");
            return 1;
        }

        var builder = new GradleBuilder();

        switch (action)
        {
            case "build":
                return await builder.BuildDebugAsync(projectPath) ? 0 : 1;

            case "build-release":
                return await builder.BuildReleaseAsync(projectPath) ? 0 : 1;

            case "build-signed":
                var keystore = GetArgument(args, "-keystore");
                var keystorePass = GetArgument(args, "-storepass");
                var keyPass = GetArgument(args, "-keypass");
                var alias = GetArgument(args, "-alias");

                if (string.IsNullOrEmpty(keystore))
                {
                    ConsoleLogger.Error("Missing -keystore argument");
                    return 1;
                }

                return await builder.BuildSignedAsync(projectPath, keystore, keystorePass, keyPass, alias) ? 0 : 1;

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
        ConsoleLogger.Header("KOTLIN/JAVA BUILD COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe -kotlin build                Build debug APK");
        Console.WriteLine("  alternative.exe -kotlin build-release        Build release APK");
        Console.WriteLine("  alternative.exe -kotlin build-signed [opts]  Build signed APK");
        Console.WriteLine("  alternative.exe -kotlin clean                Clean build");
        Console.WriteLine();
        Console.WriteLine("SIGNED BUILD OPTIONS:");
        Console.WriteLine("  -keystore <path>      Path to keystore file (required)");
        Console.WriteLine("  -storepass <pass>     Keystore password");
        Console.WriteLine("  -keypass <pass>       Key password");
        Console.WriteLine("  -alias <name>         Key alias");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -kotlin build");
        Console.WriteLine("  alternative.exe -kotlin build-signed -keystore my.jks -alias mykey");
    }

    private string? GetArgument(string[] args, string flag)
    {
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] == flag)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}
