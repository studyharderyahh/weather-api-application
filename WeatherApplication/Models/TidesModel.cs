using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApplication.APIHelpers;

namespace WeatherApplication
{
    public class TidesModel
    {
        public class TidesData
        {
            public Metadata Metadata { get; set; }
            public List<TideValue> Values { get; set; }
        }

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

        public class TideValue
        {
            public DateTime Time { get; set; }
            public double Value { get; set; }
        }

        public async Task<TidesData> GetTidesData(double lat, double lon, string apiKey, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty.");
            }

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
                Values = new List<TideValue>()
            };

            DateTime currentDate = startDate;
            string resultMessage = "";

            while (currentDate <= endDate)
            {
                string year = currentDate.Year.ToString();
                string month = currentDate.Month.ToString("D2");
                string filename = $"tides_{year}_{month}.json";
                resultMessage += $"Downloading {currentDate:MMM} {year}\n";

                // Calculate the number of days in the current month
                int numberOfDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                string dateString = currentDate.ToString("yyyy-MM-dd");

                string url = $"https://api.niwa.co.nz/tides/data?lat={lat}&long={lon}&datum=MSL&numberOfDays={numberOfDays}&apikey={apiKey}&startDate={dateString}";

                using (HttpClientWrapper client = HttpClientWrapper.Instance)
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string result = await response.Content.ReadAsStringAsync();
                        File.WriteAllText(filename, result);

                        // Deserialize the JSON data and add it to the Values list
                        TidesData monthlyData = JsonConvert.DeserializeObject<TidesData>(result);
                        tidesData.Values.AddRange(monthlyData.Values);
                    }
                    catch (HttpRequestException ex)
                    {
                        resultMessage += $"An error occurred while downloading tides data: {ex.Message}\n";
                    }
                }

                currentDate = currentDate.AddMonths(1);
            }
            resultMessage += "Done";
            return tidesData;
        }
    }
}
