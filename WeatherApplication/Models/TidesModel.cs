using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLog.Config;
using WeatherApplication.APIHelpers;
using WeatherApplication.FileHandlers;

namespace WeatherApplication.Models
{
    public class TidesModel
    {
        private readonly HttpClientWrapper m_httpClient = HttpClientWrapper.Instance;
        private readonly Logger logger = Logger.Instance();
        private readonly ITidesDataFactory tidesFactory;
        public event EventHandler<string> ProgressUpdated;

        public TidesModel(ITidesDataFactory factory)
        {
            tidesFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        // Nested class to represent tides data structure
        public class TidesData
        {
            public Metadata Metadata { get; set; }
            // Initialize empty list of tide values
            public List<TideValue> Values { get; set; } = new List<TideValue>();
        }

        // Nested class to represent metadata about tides data
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

        // Nested class to represent a single tide value
        public class TideValue
        {
            public DateTime Time { get; set; }
            public double Value { get; set; }

        }

        // Method to fetch tides data asynchronously
        public async Task<TidesData> GetTidesDataAsync(double lat, double lon, string apiKey, DateTime startDate, DateTime endDate, string tidesBaseUrl)
        {
            // Validate API key				   
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty.");
            }

            TidesData tidesData = tidesFactory.CreateTidesData(lat, lon, startDate, endDate);
            DateTime currentDate = startDate;
            string resultMessage = "";

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

                try
                {
                    HttpResponseMessage response = await m_httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    File.WriteAllText(filename, result);

                    // Deserialize the JSON data and add it to the Values list														  
                    TidesData monthlyData = JsonConvert.DeserializeObject<TidesData>(result);
                    tidesData.Values.AddRange(monthlyData.Values);
                }
                catch (HttpRequestException ex)
                {
                    logger.LogError($"An error occurred while downloading tides data: {ex.Message}\n");
                }

                currentDate = currentDate.AddMonths(1);
            }

            resultMessage += "Done";
            return tidesData;
        }
    }
}
