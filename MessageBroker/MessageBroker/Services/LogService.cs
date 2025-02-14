namespace MessageBroker.Services;

public class LogService
{
    private static readonly string LogFilePath = "logs.txt";
    private static readonly object LockObj = new();

    public static void Log(string message, string level = "INFO")
    {
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";

        lock (LockObj)
        {

            if (level == "ERROR")
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (level == "WARNING")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }

            Console.WriteLine(logMessage);
            Console.ResetColor();

            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }
    }

    public static void Info(string message) => Log(message, "INFO");
    public static void Warning(string message) => Log(message, "WARNING");
    public static void Error(string message) => Log(message, "ERROR");
}
