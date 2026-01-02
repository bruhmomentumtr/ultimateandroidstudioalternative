using AlternativeBuild.Tools;
using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

/// <summary>
/// ADB (Android Debug Bridge) command wrapper
/// </summary>
public class AdbCommand : ICommand
{
    public string Name => "adb";
    public string Description => "Android Debug Bridge - device management and debugging";

    public async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length == 0)
        {
            ShowHelp();
            return 0;
        }

        // Extract ADB if not already done
        var adbPath = ToolsManager.GetAdbPath();

        if (string.IsNullOrEmpty(adbPath) || !File.Exists(adbPath))
        {
            ConsoleLogger.Error("ADB not found. Embedded ADB tools may not be present.");
            ConsoleLogger.Info("Add ADB binaries to EmbeddedResources/tools/adb/");
            return 1;
        }

        // Run ADB with all arguments
        var adbArgs = string.Join(" ", args);
        ConsoleLogger.Info($"Running: adb {adbArgs}");
        Console.WriteLine();

        var result = await ProcessRunner.RunAsync(
            adbPath,
            adbArgs,
            Directory.GetCurrentDirectory(),
            null,
            line => Console.WriteLine(line),
            line => ConsoleLogger.Warning(line)
        );

        return result.ExitCode;
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("ADB COMMAND");
        Console.WriteLine();
        Console.WriteLine("DESCRIPTION:");
        Console.WriteLine("  Android Debug Bridge - communicate with Android devices/emulators");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe -adb <adb-command> [options]");
        Console.WriteLine();
        Console.WriteLine("COMMON COMMANDS:");
        Console.WriteLine("  devices                    List connected devices");
        Console.WriteLine("  install <apk>              Install APK to device");
        Console.WriteLine("  uninstall <package>        Uninstall app");
        Console.WriteLine("  logcat                     View device logs");
        Console.WriteLine("  shell <command>            Run shell command on device");
        Console.WriteLine("  push <local> <remote>      Copy file to device");
        Console.WriteLine("  pull <remote> <local>      Copy file from device");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -adb devices");
        Console.WriteLine("  alternative.exe -adb install app-release.apk");
        Console.WriteLine("  alternative.exe -adb logcat -s MyApp");
        Console.WriteLine("  alternative.exe -adb shell pm list packages");
        Console.WriteLine();
        Console.WriteLine("For full ADB documentation:");
        Console.WriteLine("  https://developer.android.com/studio/command-line/adb");
    }
}
