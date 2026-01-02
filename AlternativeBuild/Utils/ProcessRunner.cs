using System.Diagnostics;
using System.Text;

namespace AlternativeBuild.Utils;

/// <summary>
/// Utility for running external processes (gradle, flutter, etc.)
/// </summary>
public class ProcessRunner
{
    public static async Task<ProcessResult> RunAsync(
        string executable,
        string arguments,
        string workingDirectory,
        Dictionary<string, string>? environmentVariables = null,
        Action<string>? onOutputLine = null,
        Action<string>? onErrorLine = null)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = executable,
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        // Add environment variables
        if (environmentVariables != null)
        {
            foreach (var kvp in environmentVariables)
            {
                processInfo.Environment[kvp.Key] = kvp.Value;
            }
        }

        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        using var process = new Process { StartInfo = processInfo };

        process.OutputDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                outputBuilder.AppendLine(e.Data);
                onOutputLine?.Invoke(e.Data);
            }
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (e.Data != null)
            {
                errorBuilder.AppendLine(e.Data);
                onErrorLine?.Invoke(e.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        return new ProcessResult
        {
            ExitCode = process.ExitCode,
            Output = outputBuilder.ToString(),
            Error = errorBuilder.ToString()
        };
    }
}

public class ProcessResult
{
    public int ExitCode { get; init; }
    public string Output { get; init; } = "";
    public string Error { get; init; } = "";
    public bool Success => ExitCode == 0;
}
