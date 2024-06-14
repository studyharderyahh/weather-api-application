using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WeatherApplication.FileHandlers
{
    public class ConfigFileReader
    {
        private readonly string filePath;
        private Dictionary<string, string> m_API_Dictionary = new Dictionary<string, string>();

        public ConfigFileReader(string filePath)
        {
            this.filePath = filePath;
            ReadConfig();
        }

        public string GetKeyValue(string key)
        {
            if (m_API_Dictionary.TryGetValue(key, out string value))
            {
                return value;
            }
            return null;
        }

        private void ReadConfig()
        {
            try
            {
                // Read the entire file as a string
                string jsonContent = File.ReadAllText(this.filePath);

                // Deserialize JSON into dictionary
                m_API_Dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while reading the config file: {ex.Message}", ex);
            }
        }
    }
}
