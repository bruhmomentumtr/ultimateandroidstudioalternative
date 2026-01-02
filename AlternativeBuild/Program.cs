using AlternativeBuild.Commands;
using AlternativeBuild.Utils;

namespace AlternativeBuild;

class Program
{
    private static readonly Dictionary<string, ICommand> Commands = new();

    static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Register commands
        RegisterCommands();

        // Show banner
        ShowBanner();

        // Parse and execute command
        if (args.Length == 0 || args[0] == "-help" || args[0] == "--help" || args[0] == "-h")
        {
            ShowHelp();
            return 0;
        }

        // Get command name (remove leading dashes)
        string commandName = args[0].TrimStart('-');

        // Check for command-specific help
        if (args.Length > 1 && (args[1] == "-help" || args[1] == "--help"))
        {
            if (Commands.TryGetValue(commandName, out var helpCommand))
            {
                helpCommand.ShowHelp();
                return 0;
            }
        }

        // Execute command
        if (Commands.TryGetValue(commandName, out var command))
        {
            try
            {
                var commandArgs = args.Skip(1).ToArray();
                return await command.ExecuteAsync(commandArgs);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Error($"Command execution failed: {ex.Message}");
                return 1;
            }
        }
        else
        {
            ConsoleLogger.Error($"Unknown command: {commandName}");
            ConsoleLogger.Info("Use -help to see available commands");
            return 1;
        }
    }

    private static void RegisterCommands()
    {
        Commands.Add("flutter", new FlutterBuildCommand());
        Commands.Add("kotlin", new KotlinBuildCommand());
        Commands.Add("sdk", new SdkCommand());
        Commands.Add("ndk", new NdkCommand());
        Commands.Add("flutter-sdk", new FlutterSdkCommand());
        Commands.Add("config", new ConfigCommand());
        Commands.Add("adb", new AdbCommand());
        Commands.Add("apksigner", new ApkSignerCommand());
        Commands.Add("avd", new AvdCommand());
    }

    private static void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
╔══════════════════════════════════════════════════╗
║   Alternative Build Tool v1.0                    ║
║   Android Studio Alternative CLI Builder         ║
╚══════════════════════════════════════════════════╝
");
        Console.ResetColor();
    }

    private static void ShowHelp()
    {
        ConsoleLogger.Header("ALTERNATIVE BUILD TOOL");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe <command> [options]");
        Console.WriteLine();
        Console.WriteLine("BUILD COMMANDS:");
        Console.WriteLine("  -kotlin <action>        Build Kotlin/Java Android projects");
        Console.WriteLine("  -flutter <action>       Build Flutter Android projects");
        Console.WriteLine();
        Console.WriteLine("SDK MANAGEMENT:");
        Console.WriteLine("  -sdk <action>           Manage Android SDK versions");
        Console.WriteLine("  -ndk <action>           Manage Android NDK versions");
        Console.WriteLine("  --flutter-sdk <action>  Manage Flutter SDK versions");
        Console.WriteLine();
        Console.WriteLine("ANDROID TOOLS (Embedded):");
        Console.WriteLine("  -adb <command>          Android Debug Bridge - device management");
        Console.WriteLine("  -apksigner <command>    Sign and verify APK files");
        Console.WriteLine();
        Console.WriteLine("CONFIGURATION:");
        Console.WriteLine("  -config <key> [value]   Configure tool settings");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -flutter build");
        Console.WriteLine("  alternative.exe -kotlin build-signed -keystore my.keystore");
        Console.WriteLine("  alternative.exe -sdk install android-34");
        Console.WriteLine("  alternative.exe --flutter-sdk install 3.19.0");
        Console.WriteLine("  alternative.exe -ndk install 26.1.10909125");
        Console.WriteLine("  alternative.exe -adb devices");
        Console.WriteLine("  alternative.exe -adb install app.apk");
        Console.WriteLine("  alternative.exe -apksigner sign --ks my.jks --out signed.apk app.apk");
        Console.WriteLine("  alternative.exe -config sdk-path \"C:\\CustomSDK\"");
        Console.WriteLine();
        Console.WriteLine("For command-specific help:");
        Console.WriteLine("  alternative.exe <command> -help");
    }
}
