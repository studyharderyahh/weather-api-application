using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.FileHandlers
{
    internal class ConfigFileReader
    {
        private readonly string filePath;

        private m_APIDictionary<string, string> m_API_Dictionary = new m_APIDictionary<string, string>();

        public ConfigFileReader(string filePath)
        {
            this.filePath = filePath;
            // as soon as you create this load the m_API_Dictionary
            ReadConfig();
        }

        public string GetAPIKey(string apiKey) {

            if (m_API_Dictionary.TryGetValue(apiKey, out string apiValue))
            {
                return apiValue;
            }
            return null;

        }


        public void ReadConfig()
        {
            try
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(this.filePath);
                //var configKeys = new Dictionary<string, string>();

                // Process each line
                foreach (var line in lines)
                {
                    // Split the line to get the key and value
                    string[] keyValue = line.Split('=');

                    if (keyValue.Length == 2)
                    {
                        //configKeys[keyValue[0].Trim()] = keyValue[1].Trim();
                        m_API_Dictionary.Add( keyValue[0].Trim(),keyValue[1].Trim());
                    }
                    else
                    {
                        throw new FormatException($"The line '{line}' is not in the expected format 'Key=Value'.");
                    }
                }

                //return configKeys;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while reading the config file: {ex.Message}", ex);
            }
        }
    }
}
