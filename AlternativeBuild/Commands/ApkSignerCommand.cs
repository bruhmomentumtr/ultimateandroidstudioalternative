using AlternativeBuild.Tools;
using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

/// <summary>
/// Apksigner wrapper for APK signing
/// </summary>
public class ApkSignerCommand : ICommand
{
    public string Name => "apksigner";
    public string Description => "Sign and verify APK files";

    public async Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length == 0)
        {
            ShowHelp();
            return 0;
        }

        var apksignerJar = ToolsManager.GetApksignerPath();

        if (!File.Exists(apksignerJar))
        {
            ConsoleLogger.Error("Apksigner not found. Embedded tools may not be present.");
            return 1;
        }

        // Run apksigner via Java
        var javaPath = FindJava();
        if (string.IsNullOrEmpty(javaPath))
        {
            ConsoleLogger.Error("Java not found. Please install Java JDK.");
            return 1;
        }

        var apksignerArgs = $"-jar \"{apksignerJar}\" {string.Join(" ", args)}";
        ConsoleLogger.Info($"Running: java {apksignerArgs}");
        Console.WriteLine();

        var result = await ProcessRunner.RunAsync(
            javaPath,
            apksignerArgs,
            Directory.GetCurrentDirectory(),
            null,
            line => Console.WriteLine(line),
            line => ConsoleLogger.Warning(line)
        );

        return result.ExitCode;
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("APKSIGNER COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe -apksigner sign [options] <apk>");
        Console.WriteLine("  alternative.exe -apksigner verify [options] <apk>");
        Console.WriteLine();
        Console.WriteLine("SIGN OPTIONS:");
        Console.WriteLine("  --ks <keystore>           Keystore file");
        Console.WriteLine("  --ks-key-alias <alias>    Key alias");
        Console.WriteLine("  --ks-pass pass:<pass>     Keystore password");
        Console.WriteLine("  --key-pass pass:<pass>    Key password");
        Console.WriteLine("  --out <output-apk>        Output signed APK");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -apksigner sign --ks my.jks --ks-key-alias mykey --out signed.apk app.apk");
        Console.WriteLine("  alternative.exe -apksigner verify --verbose app.apk");
    }

    private string? FindJava()
    {
        // Check JAVA_HOME
        var javaHome = Environment.GetEnvironmentVariable("JAVA_HOME");
        if (!string.IsNullOrEmpty(javaHome))
        {
            var javaExe = Path.Combine(javaHome, "bin", "java.exe");
            if (File.Exists(javaExe)) return javaExe;
        }

        // Check PATH
        var pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (!string.IsNullOrEmpty(pathEnv))
        {
            foreach (var path in pathEnv.Split(';'))
            {
                var javaExe = Path.Combine(path, "java.exe");
                if (File.Exists(javaExe)) return javaExe;
            }
        }

        return null;
    }
}
