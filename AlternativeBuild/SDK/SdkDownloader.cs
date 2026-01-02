using AlternativeBuild.Utils;
using System.IO.Compression;

namespace AlternativeBuild.SDK;

/// <summary>
/// Handles SDK downloading with progress reporting
/// </summary>
public class SdkDownloader
{
    private static readonly HttpClient _httpClient = new();

    public static async Task<string> DownloadAndExtractAsync(
        string url,
        string targetDirectory,
        string packageName,
        Action<int>? onProgress = null)
    {
        ConsoleLogger.Info($"Downloading {packageName}...");

        var tempZip = Path.Combine(Path.GetTempPath(), $"{packageName}.zip");

        try
        {
            // Download with progress
            using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength ?? 0;
                using var contentStream = await response.Content.ReadAsStreamAsync();
                using var fileStream = new FileStream(tempZip, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

                var buffer = new byte[8192];
                long totalRead = 0;
                int percentComplete = 0;

                while (true)
                {
                    var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                    if (read == 0) break;

                    await fileStream.WriteAsync(buffer, 0, read);
                    totalRead += read;

                    if (totalBytes > 0)
                    {
                        var newPercent = (int)((totalRead * 100) / totalBytes);
                        if (newPercent != percentComplete)
                        {
                            percentComplete = newPercent;
                            onProgress?.Invoke(percentComplete);
                            Console.Write($"\r  Progress: {percentComplete}%");
                        }
                    }
                }
                Console.WriteLine();
            }

            ConsoleLogger.Info($"Extracting {packageName}...");
            Directory.CreateDirectory(targetDirectory);
            ZipFile.ExtractToDirectory(tempZip, targetDirectory, true);

            ConsoleLogger.Success($"{packageName} installed successfully");
            return targetDirectory;
        }
        finally
        {
            if (File.Exists(tempZip))
            {
                File.Delete(tempZip);
            }
        }
    }

    public static async Task<string> DownloadFileAsync(string url, string targetPath, Action<int>? onProgress = null)
    {
        using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var totalBytes = response.Content.Headers.ContentLength ?? 0;
        using var contentStream = await response.Content.ReadAsStreamAsync();

        Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
        using var fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

        var buffer = new byte[8192];
        long totalRead = 0;
        int percentComplete = 0;

        while (true)
        {
            var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
            if (read == 0) break;

            await fileStream.WriteAsync(buffer, 0, read);
            totalRead += read;

            if (totalBytes > 0)
            {
                var newPercent = (int)((totalRead * 100) / totalBytes);
                if (newPercent != percentComplete)
                {
                    percentComplete = newPercent;
                    onProgress?.Invoke(percentComplete);
                }
            }
        }

        return targetPath;
    }
}
