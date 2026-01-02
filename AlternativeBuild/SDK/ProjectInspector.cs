using System.Text.RegularExpressions;

namespace AlternativeBuild.SDK;

/// <summary>
/// Inspects Android projects to determine required SDK/NDK versions
/// </summary>
public class ProjectInspector
{
    /// <summary>
    /// Get required Android SDK version from project
    /// </summary>
    public static int? GetRequiredCompileSdk(string projectPath)
    {
        // Check build.gradle.kts (Kotlin DSL)
        var gradleKts = Path.Combine(projectPath, "app", "build.gradle.kts");
        if (File.Exists(gradleKts))
        {
            var content = File.ReadAllText(gradleKts);
            var match = Regex.Match(content, @"compileSdk\s*=\s*(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out var sdk))
            {
                return sdk;
            }
        }

        // Check build.gradle (Groovy DSL)
        var gradle = Path.Combine(projectPath, "app", "build.gradle");
        if (File.Exists(gradle))
        {
            var content = File.ReadAllText(gradle);

            // compileSdkVersion 34 or compileSdk 34
            var match = Regex.Match(content, @"compileSdk(?:Version)?\s+(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out var sdk))
            {
                return sdk;
            }
        }

        return null;
    }

    /// <summary>
    /// Get required NDK version from project
    /// </summary>
    public static string? GetRequiredNdkVersion(string projectPath)
    {
        // Check build.gradle.kts
        var gradleKts = Path.Combine(projectPath, "app", "build.gradle.kts");
        if (File.Exists(gradleKts))
        {
            var content = File.ReadAllText(gradleKts);
            var match = Regex.Match(content, @"ndkVersion\s*=\s*""([^""]+)""");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }

        // Check build.gradle
        var gradle = Path.Combine(projectPath, "app", "build.gradle");
        if (File.Exists(gradle))
        {
            var content = File.ReadAllText(gradle);
            var match = Regex.Match(content, @"ndkVersion\s+['""]([^'""]+)['""]");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }

        return null;
    }

    /// <summary>
    /// Get build tools version (if specified)
    /// </summary>
    public static string? GetRequiredBuildToolsVersion(string projectPath)
    {
        // Check build.gradle.kts
        var gradleKts = Path.Combine(projectPath, "app", "build.gradle.kts");
        if (File.Exists(gradleKts))
        {
            var content = File.ReadAllText(gradleKts);
            var match = Regex.Match(content, @"buildToolsVersion\s*=\s*""([^""]+)""");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }

        // Check build.gradle
        var gradle = Path.Combine(projectPath, "app", "build.gradle");
        if (File.Exists(gradle))
        {
            var content = File.ReadAllText(gradle);
            var match = Regex.Match(content, @"buildToolsVersion\s+['""]([^'""]+)['""]");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }

        return null;
    }

    /// <summary>
    /// Get target SDK version
    /// </summary>
    public static int? GetTargetSdk(string projectPath)
    {
        // Check build.gradle.kts
        var gradleKts = Path.Combine(projectPath, "app", "build.gradle.kts");
        if (File.Exists(gradleKts))
        {
            var content = File.ReadAllText(gradleKts);
            var match = Regex.Match(content, @"targetSdk\s*=\s*(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out var sdk))
            {
                return sdk;
            }
        }

        // Check build.gradle
        var gradle = Path.Combine(projectPath, "app", "build.gradle");
        if (File.Exists(gradle))
        {
            var content = File.ReadAllText(gradle);
            var match = Regex.Match(content, @"targetSdk(?:Version)?\s+(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out var sdk))
            {
                return sdk;
            }
        }

        return null;
    }

    /// <summary>
    /// Get min SDK version
    /// </summary>
    public static int? GetMinSdk(string projectPath)
    {
        // Check build.gradle.kts
        var gradleKts = Path.Combine(projectPath, "app", "build.gradle.kts");
        if (File.Exists(gradleKts))
        {
            var content = File.ReadAllText(gradleKts);
            var match = Regex.Match(content, @"minSdk\s*=\s*(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out var sdk))
            {
                return sdk;
            }
        }

        // Check build.gradle
        var gradle = Path.Combine(projectPath, "app", "build.gradle");
        if (File.Exists(gradle))
        {
            var content = File.ReadAllText(gradle);
            var match = Regex.Match(content, @"minSdk(?:Version)?\s+(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out var sdk))
            {
                return sdk;
            }
        }

        return null;
    }
}
