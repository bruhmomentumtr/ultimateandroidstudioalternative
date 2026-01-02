using AlternativeBuild.Utils;

namespace AlternativeBuild.Commands;

public class ConfigCommand : ICommand
{
    public string Name => "config";
    public string Description => "Manage configuration settings";

    public Task<int> ExecuteAsync(string[] args)
    {
        if (args.Length == 0 || args[0] == "list")
        {
            ListConfig();
            return Task.FromResult(0);
        }

        if (args[0] == "get")
        {
            if (args.Length < 2)
            {
                ConsoleLogger.Error("Missing key. Usage: -config get <key>");
                return Task.FromResult(1);
            }
            GetConfig(args[1]);
            return Task.FromResult(0);
        }

        // Set config: -config <key> <value>
        if (args.Length < 2)
        {
            ConsoleLogger.Error("Missing value. Usage: -config <key> <value>");
            return Task.FromResult(1);
        }

        SetConfig(args[0], args[1]);
        return Task.FromResult(0);
    }

    public void ShowHelp()
    {
        ConsoleLogger.Header("CONFIG COMMAND");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  alternative.exe -config <key> <value>   Set configuration value");
        Console.WriteLine("  alternative.exe -config get <key>       Get configuration value");
        Console.WriteLine("  alternative.exe -config list            List all configurations");
        Console.WriteLine();
        Console.WriteLine("KEYS:");
        Console.WriteLine("  sdk-path        SDK installation directory (default: %USERPROFILE%\\.alternative-sdk)");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  alternative.exe -config sdk-path \"C:\\Android\\SDKs\"");
        Console.WriteLine("  alternative.exe -config get sdk-path");
        Console.WriteLine("  alternative.exe -config list");
    }

    private void ListConfig()
    {
        ConsoleLogger.Header("CURRENT CONFIGURATION");
        var config = ConfigManager.Instance.GetAll();

        if (config.Count == 0)
        {
            ConsoleLogger.Info("No configuration set (using defaults)");
            Console.WriteLine();
            Console.WriteLine($"  sdk-path = {ConfigManager.Instance.SdkPath} (default)");
            return;
        }

        foreach (var kvp in config)
        {
            Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
        }
    }

    private void GetConfig(string key)
    {
        var value = ConfigManager.Instance.GetValue(key);
        if (string.IsNullOrEmpty(value))
        {
            ConsoleLogger.Warning($"Key '{key}' not found");
            return;
        }
        Console.WriteLine(value);
    }

    private void SetConfig(string key, string value)
    {
        ConfigManager.Instance.SetValue(key, value);
        ConsoleLogger.Success($"Configuration updated: {key} = {value}");
    }
}
