using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApplication.APIHelpers;
using WeatherApplication.FileHandlers;

namespace WeatherApplication.Models
{
    /// <summary>
    /// Model class responsible for fetching and processing tides data.
    /// </summary>
    public class TidesModel
    {
        private readonly HttpClientWrapper m_httpClient = HttpClientWrapper.Instance;
        private readonly Logger logger = Logger.Instance();
        private readonly ITidesDataFactory tidesFactory;

        /// <summary>
        /// Event raised when progress is updated during data retrieval.
        /// </summary>
        public event EventHandler<string> ProgressUpdated;

        /// <summary>
        /// Initializes a new instance of the TidesModel class with a specified data factory.
        /// </summary>
        /// <param name="factory">Factory responsible for creating TidesData objects.</param>
        public TidesModel(ITidesDataFactory factory)
        {
            tidesFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Nested class representing tides data structure.
        /// </summary>
        public class TidesData
        {
            public Metadata Metadata { get; set; }
            public List<TideValue> Values { get; set; } = new List<TideValue>();
        }

        /// <summary>
        /// Nested class representing metadata about tides data.
        /// </summary>
        public class Metadata
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public string Datum { get; set; }
            public DateTime Start { get; set; }
            public int Days { get; set; }
            public int Interval { get; set; }
            public string Height { get; set; }
        }

        /// <summary>
        /// Nested class representing a single tide value.
        /// </summary>
        public class TideValue
        {
            public DateTime Time { get; set; }
            public double Value { get; set; }
        }

        /// <summary>
        /// Asynchronously fetches tides data from an API based on the specified parameters.
        /// </summary>
        /// <param name="lat">Latitude coordinate.</param>
        /// <param name="lon">Longitude coordinate.</param>
        /// <param name="apiKey">API key for accessing the tides data API.</param>
        /// <param name="startDate">Start date for retrieving tides data.</param>
        /// <param name="endDate">End date for retrieving tides data.</param>
        /// <param name="tidesBaseUrl">Base URL of the tides API.</param>
        /// <returns>Task representing the asynchronous operation with the fetched TidesData.</returns>
        public async Task<TidesData> GetTidesDataAsync(double lat, double lon, string apiKey, DateTime startDate, DateTime endDate, string tidesBaseUrl)
        {
            // Validate API key
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
            }

            // Create initial TidesData using the factory
            TidesData tidesData = tidesFactory.CreateTidesData(lat, lon, startDate, endDate);
            DateTime currentDate = startDate;
            string resultMessage = "";

            try
            {
                while (currentDate <= endDate)
                {
                    // Prepare information for API request
                    string year = currentDate.Year.ToString();
                    string month = currentDate.Month.ToString("D2");
                    string filename = $"tides_{year}_{month}.json";
                    ProgressUpdated?.Invoke(this, $"Downloading {currentDate:MMM} {year} into {filename}");

                    // Calculate the number of days in the current month
                    int numberOfDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                    string dateString = currentDate.ToString("yyyy-MM-dd");

                    // Construct URL for API request
                    string url = $"{tidesBaseUrl}{lat}&long={lon}&datum=MSL&numberOfDays={numberOfDays}&apikey={apiKey}&startDate={dateString}";

                    // Make the HTTP request to fetch tides data
                    HttpResponseMessage response = await m_httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    // Read the response content as JSON
                    string result = await response.Content.ReadAsStringAsync();

                    // Write the JSON response to a file
                    File.WriteAllText(filename, result);

                    // Deserialize the JSON data and add it to the Values list
                    TidesData monthlyData = JsonConvert.DeserializeObject<TidesData>(result);
                    tidesData.Values.AddRange(monthlyData.Values);

                    currentDate = currentDate.AddMonths(1);
                }

                resultMessage += "Done";
            }
            catch (HttpRequestException ex)
            {
                logger.LogError($"An error occurred while downloading tides data: {ex.Message}");
                throw new WeatherServiceException("Failed to retrieve tides data. Check your network connection and try again.", ex);
            }
            catch (JsonException ex)
            {
                logger.LogError($"Error parsing JSON response: {ex.Message}");
                throw new WeatherServiceException("Error parsing tides data response.", ex);
            }
            catch (Exception ex)
            {
                logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw;
            }

            return tidesData;
        }
    }
}
