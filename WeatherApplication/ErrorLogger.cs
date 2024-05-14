using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public sealed class ErrorLogger
    {
        private static readonly ErrorLogger instance = new ErrorLogger();
        private static readonly string logFilePath = "error.log";

        public static ErrorLogger Instance
        {
            get { return instance; }
        }

        public void LogError(string errorMessage)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                // Log to console if writing to file fails
                Console.WriteLine($"Failed to log error: {ex.Message}");
            }
        }
    }
}
