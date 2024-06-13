using System;
using System.IO;

namespace WeatherApplication.FileHandlers
{
    public enum LogLevel
    {
        INFO,
        WARNING,
        ERROR
    }

    public class Logger
    {
        private static readonly Lazy<Logger> lazyInstance = new Lazy<Logger>(() => new Logger());
        private readonly string logFilePath;
        private readonly object lockObject = new object();

        private Logger()
        {
            // Default log file path
            logFilePath = "Logs/application.log";
            InitializeLogFile();
        }

        private Logger(string logFilePath)
        {
            this.logFilePath = logFilePath;
            InitializeLogFile();
        }

        public static Logger Instance(string logFilePath = null)
        {
            if (logFilePath != null)
            {
                // If a log file path is provided, use it for initialization
                lazyInstance.Value.InitializeLogFile(logFilePath);
            }
            return lazyInstance.Value;
        }

        private void InitializeLogFile(string customLogFilePath = null)
        {
            string logFile = customLogFilePath ?? logFilePath;
            var logDirectory = Path.GetDirectoryName(logFile);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        public void Log(LogLevel logLevel, string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}";
            WriteLog(logEntry);
        }

        public void LogInfo(string message)
        {
            Log(LogLevel.INFO, message);
        }

        public void LogWarning(string message)
        {
            Log(LogLevel.WARNING, message);
        }

        public void LogError(string message)
        {
            Log(LogLevel.ERROR, message);
        }

        private void WriteLog(string logEntry)
        {
            lock (lockObject) // Ensure thread-safety
            {
                try
                {
                    using (var writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine(logEntry);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing to log file: {ex.Message}");
                }
            }
        }
    }
}
