using System;
using System.IO;

namespace WeatherApplication.FileHandlers
{
    /// <summary>
    /// Defines the severity level of a log message.
    /// </summary>
    public enum LogLevel
    {
        INFO,
        WARNING,
        ERROR
    }

    /// <summary>
    /// Provides logging functionalities to record messages to a log file.
    /// </summary>
    public class Logger
    {
        private static readonly Lazy<Logger> lazyInstance = new Lazy<Logger>(() => new Logger());
        private readonly string logFilePath;
        private readonly object lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class with the default log file path "Logs/application.log".
        /// </summary>
        private Logger()
        {
            logFilePath = "Logs/application.log";
            InitializeLogFile();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class with a custom log file path.
        /// </summary>
        /// <param name="logFilePath">The path of the log file.</param>
        private Logger(string logFilePath)
        {
            this.logFilePath = logFilePath;
            InitializeLogFile();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="Logger"/> class.
        /// </summary>
        /// <param name="logFilePath">Optional: The path of the log file. If provided, initializes the logger with the custom log file path.</param>
        /// <returns>The singleton instance of the <see cref="Logger"/> class.</returns>
        public static Logger Instance(string logFilePath = null)
        {
            if (logFilePath != null)
            {
                lazyInstance.Value.InitializeLogFile(logFilePath);
            }
            return lazyInstance.Value;
        }

        /// <summary>
        /// Initializes the log file if it does not exist.
        /// </summary>
        /// <param name="customLogFilePath">Optional: A custom log file path to initialize.</param>
        private void InitializeLogFile(string customLogFilePath = null)
        {
            string logFile = customLogFilePath ?? logFilePath;
            var logDirectory = Path.GetDirectoryName(logFile);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        /// <summary>
        /// Logs a message with the specified severity level.
        /// </summary>
        /// <param name="logLevel">The severity level of the log message.</param>
        /// <param name="message">The message to log.</param>
        public void Log(LogLevel logLevel, string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {message}";
            WriteLog(logEntry);
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        public void LogInfo(string message)
        {
            Log(LogLevel.INFO, message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void LogWarning(string message)
        {
            Log(LogLevel.WARNING, message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public void LogError(string message)
        {
            Log(LogLevel.ERROR, message);
        }

        /// <summary>
        /// Writes the log entry to the log file in a thread-safe manner.
        /// </summary>
        /// <param name="logEntry">The log entry to write.</param>
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
