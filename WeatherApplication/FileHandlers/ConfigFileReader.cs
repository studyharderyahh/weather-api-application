using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WeatherApplication.FileHandlers
{
    /// <summary>
    /// Reads configuration settings from a JSON file.
    /// </summary>
    public class ConfigFileReader
    {
        private readonly string filePath;
        private Dictionary<string, string> m_API_Dictionary = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigFileReader"/> class.
        /// </summary>
        /// <param name="filePath">The path to the configuration file.</param>
        public ConfigFileReader(string filePath)
        {
            this.filePath = filePath;
            ReadConfig();
        }

        /// <summary>
        /// Retrieves the value associated with the specified key from the configuration.
        /// </summary>
        /// <param name="key">The key whose value to retrieve.</param>
        /// <returns>The value associated with the key, or null if the key is not found.</returns>
        public string GetKeyValue(string key)
        {
            if (m_API_Dictionary.TryGetValue(key, out string value))
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// Reads the configuration file and populates the internal dictionary.
        /// </summary>
        private void ReadConfig()
        {
            try
            {
                // Read the entire file as a string
                string jsonContent = File.ReadAllText(this.filePath);

                // Deserialize JSON into dictionary
                m_API_Dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
            }
            catch (FileNotFoundException ex)
            {
                // Handle file not found exception
                throw new FileNotFoundException($"Config file not found at '{this.filePath}'.", ex);
            }
            catch (JsonException ex)
            {
                // Handle JSON parsing exception
                throw new ApplicationException($"Error parsing JSON in config file '{this.filePath}': {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                throw new ApplicationException($"An error occurred while reading the config file '{this.filePath}': {ex.Message}", ex);
            }
        }
    }
}
