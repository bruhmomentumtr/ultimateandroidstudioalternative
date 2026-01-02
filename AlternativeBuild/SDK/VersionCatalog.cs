namespace AlternativeBuild.SDK;

/// <summary>
/// Available SDK/NDK/Flutter versions for browsing
/// </summary>
public static class VersionCatalog
{
    /// <summary>
    /// Android SDK Command-line Tools versions
    /// </summary>
    public static readonly List<SdkVersion> AndroidSdkVersions = new()
    {
        new("11076708", "Latest Stable (2024)", true),
        new("10406996", "11.0 (2023)", false),
        new("9477386", "9.0 (2022)", false),
        new("9123335", "8.0", false),
        new("8512546", "7.0", false),
        new("7583922", "6.0", false),
        new("6858069", "5.0", false),
        new("6609375", "4.0", false),
        new("6514223", "3.0", false),
        new("6200805", "2.0", false),
    };

    /// <summary>
    /// Android NDK versions
    /// </summary>
    public static readonly List<SdkVersion> NdkVersions = new()
    {
        new("27.0.12077973", "NDK r27 LTS (Latest)", true),
        new("26.3.11579264", "NDK r26d", false),
        new("26.2.11394342", "NDK r26c", false),
        new("26.1.10909125", "NDK r26b", false),
        new("25.2.9519653", "NDK r25c", false),
        new("25.1.8937393", "NDK r25b", false),
        new("24.0.8215888", "NDK r24", false),
        new("23.2.8568313", "NDK r23c", false),
        new("23.1.7779620", "NDK r23b", false),
        new("22.1.7171670", "NDK r22b", false),
    };

    /// <summary>
    /// Flutter SDK versions
    /// </summary>
    public static readonly List<SdkVersion> FlutterVersions = new()
    {
        new("3.27.1", "Latest Stable", true),
        new("3.27.0", "Stable", false),
        new("3.24.5", "LTS (Long Term Support)", false),
        new("3.24.4", "Stable", false),
        new("3.22.3", "Stable", false),
        new("3.19.6", "Stable", false),
        new("3.16.9", "Stable", false),
        new("3.13.9", "Stable", false),
        new("3.10.6", "Stable", false),
        new("3.7.12", "Stable", false),
    };

    /// <summary>
    /// Android System Images for AVD/Emulator
    /// </summary>
    public static readonly List<SystemImageInfo> SystemImages = new()
    {
        new("34", "Android 14 (API 34)", "google_apis", "x86_64", true),
        new("33", "Android 13 (API 33)", "google_apis", "x86_64", false),
        new("32", "Android 12L (API 32)", "google_apis", "x86_64", false),
        new("31", "Android 12 (API 31)", "google_apis", "x86_64", false),
        new("30", "Android 11 (API 30)", "google_apis", "x86_64", false),
        new("29", "Android 10 (API 29)", "google_apis", "x86_64", false),
        new("28", "Android 9 (API 28)", "google_apis", "x86_64", false),
        new("27", "Android 8.1 (API 27)", "google_apis", "x86_64", false),
        new("26", "Android 8.0 (API 26)", "google_apis", "x86_64", false),
        new("25", "Android 7.1 (API 25)", "google_apis", "x86_64", false),
    };
}

/// <summary>
/// SDK version info
/// </summary>
public record SdkVersion(string Version, string Description, bool IsRecommended);

/// <summary>
/// System image info for AVD
/// </summary>
public record SystemImageInfo(
    string ApiLevel,
    string Description,
    string Variant,      // google_apis, default, google_apis_playstore
    string Architecture, // x86_64, arm64-v8a
    bool IsRecommended
)
{
    public string PackageName => $"system-images;android-{ApiLevel};{Variant};{Architecture}";
}
