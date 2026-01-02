namespace AlternativeBuild.Commands;

/// <summary>
/// Base interface for all CLI commands
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Command name (e.g., "flutter", "kotlin", "sdk")
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Command description for help text
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Execute the command with given arguments
    /// </summary>
    /// <param name="args">Command arguments</param>
    /// <returns>Exit code (0 for success)</returns>
    Task<int> ExecuteAsync(string[] args);

    /// <summary>
    /// Show help for this command
    /// </summary>
    void ShowHelp();
}
