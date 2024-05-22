using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    internal class ConfigFileReader
    {
        private readonly string filePath;

        public ConfigFileReader(string filePath)
        {
            this.filePath = filePath;
        }

        public Dictionary<string, string> ReadConfig()
        {
            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(this.filePath);
                var configKeys = new Dictionary<string, string>();

                // Process each line
                foreach (var line in lines)
                {
                    // Split the line to get the key and value
                    string[] keyValue = line.Split('=');

                    if (keyValue.Length == 2)
                    {
                        configKeys[keyValue[0].Trim()] = keyValue[1].Trim();
                    }
                    else
                    {
                        throw new FormatException($"The line '{line}' is not in the expected format 'Key=Value'.");
                    }
                }

                return configKeys;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while reading the config file: {ex.Message}", ex);
            }
        }
    }
}
