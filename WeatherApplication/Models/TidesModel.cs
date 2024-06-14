using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApplication.APIHelpers;
using WeatherApplication.FileHandlers;

namespace WeatherApplication
{
    public class TidesModel
    {

        private readonly HttpClientWrapper m_httpClient = HttpClientWrapper.Instance;

        // Nested class to represent tides data structure
        public class TidesData
        {
            public Metadata Metadata { get; set; }  // Metadata about tides data
            public List<TideValue> Values { get; set; } // List of tide values
        }
        // Nested class to represent metadata about tides data
        public class Metadata
        {
            public double Latitude { get; set; }  // Latitude of the location
            public double Longitude { get; set; }// Longitude of the location
            public string Datum { get; set; }  // Datum reference for measurements
            public DateTime Start { get; set; } // Start date for data retrieval
            public int Days { get; set; } // Number of days of data
            public int Interval { get; set; }  // Interval between tide measurements
            public string Height { get; set; } // Height reference for measurements
        }
        // Nested class to represent a single tide value
        public class TideValue
        {
            public DateTime Time { get; set; } // Timestamp of the tide value
            public double Value { get; set; } // Actual tide value
           
        }
        // Method to fetch tides data asynchronously
        public async Task<TidesData> GetTidesData(double lat, double lon, string apiKey, DateTime startDate, DateTime endDate, string tidesBaseUrl)
        {
            // Validate API key
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty.");
            }
            // Initialize tides data object
            TidesData tidesData = new TidesData
            {
                Metadata = new Metadata
                {
                    Latitude = lat,
                    Longitude = lon,
                    Datum = "MSL", // Assuming default datum is MSL (Mean Sea Level)
                    Start = startDate,
                    Days = (int)(endDate - startDate).TotalDays + 1, // Total number of days including start and end dates
                    Interval = 0, // Assuming no interval between tide measurements
                    Height = "MSL = 0m" // Assuming default height is at Mean Sea Level
                },
                Values = new List<TideValue>()  // Initialize empty list of tide values
            
            };
            // Iterate through each month to fetch tides data
            DateTime currentDate = startDate;
            string resultMessage = "";

            while (currentDate <= endDate)
            {
                // Prepare information for API request
                string year = currentDate.Year.ToString();
                string month = currentDate.Month.ToString("D2");
                string filename = $"tides_{year}_{month}.json";
                resultMessage += $"Downloading {currentDate:MMM} {year} into {filename}\n";

                // Calculate the number of days in the current month
                int numberOfDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                string dateString = currentDate.ToString("yyyy-MM-dd");
                // Construct URL for API request
                // string url = $"https://api.niwa.co.nz/tides/data?lat={lat}&long={lon}&datum=MSL&numberOfDays={numberOfDays}&apikey={apiKey}&startDate={dateString}";
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
                    ErrorLogger.Instance.LogError($"An error occurred while downloading tides data: {ex.Message}\n");
                }

                currentDate = currentDate.AddMonths(1);
            }
            resultMessage += "Done";
            return tidesData;
        }
    }
}
