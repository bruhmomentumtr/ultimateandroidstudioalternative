namespace AlternativeBuild.Utils;

/// <summary>
/// Colorful console output utility
/// </summary>
public static class ConsoleLogger
{
    public static void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[INFO] {message}");
        Console.ResetColor();
    }

    public static void Success(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[✓] {message}");
        Console.ResetColor();
    }

    public static void Warning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[!] {message}");
        Console.ResetColor();
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[✗] {message}");
        Console.ResetColor();
    }

    public static void Header(string message)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"\n═══ {message} ═══");
        Console.ResetColor();
    }

    public static void Progress(string message)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"    {message}");
        Console.ResetColor();
    }
}
