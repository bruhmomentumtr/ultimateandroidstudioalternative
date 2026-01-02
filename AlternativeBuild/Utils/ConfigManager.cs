using System.Text.Json;

namespace AlternativeBuild.Utils;

/// <summary>
/// Manages application configuration
/// </summary>
public class ConfigManager
{
    private static readonly string ConfigDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".alternative-build"
    );

    private static readonly string ConfigFile = Path.Combine(ConfigDir, "config.json");

    private Dictionary<string, string> _config = new();

    private static ConfigManager? _instance;
    public static ConfigManager Instance => _instance ??= new ConfigManager();

    private ConfigManager()
    {
        LoadConfig();
    }

    public string SdkPath
    {
        get => GetValue("sdk-path", Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".alternative-sdk"
        ));
        set => SetValue("sdk-path", value);
    }

    public string GetValue(string key, string defaultValue = "")
    {
        return _config.TryGetValue(key, out var value) ? value : defaultValue;
    }

    public void SetValue(string key, string value)
    {
        _config[key] = value;
        SaveConfig();
    }

    public Dictionary<string, string> GetAll()
    {
        return new Dictionary<string, string>(_config);
    }

    private void LoadConfig()
    {
        try
        {
            if (File.Exists(ConfigFile))
            {
                var json = File.ReadAllText(ConfigFile);
                _config = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
            }
        }
        catch
        {
            _config = new();
        }
    }

    private void SaveConfig()
    {
        try
        {
            Directory.CreateDirectory(ConfigDir);
            var json = JsonSerializer.Serialize(_config, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(ConfigFile, json);
        }
        catch (Exception ex)
        {
            ConsoleLogger.Warning($"Failed to save config: {ex.Message}");
        }
    }
}
