using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Models;

namespace WeatherApplication.Services
{
    public class SolarFlareService
    {
        private readonly HttpClient httpClient;

        public SolarFlareService(HttpClient httpClient)
        {
            httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<SolarFlareModel>> GetFlaresAsync(string startDate, string endDate, string apiKey, string solarFlareBaseUrl)
        {
            try
            {
                //var apiUrl = $"https://api.nasa.gov/DONKI/FLR?startDate={startDate}&endDate={endDate}&api_key={apiKey}";
                var solarFlareApiUrl = $"{solarFlareBaseUrl}{startDate}&endDate={endDate}&api_key={apiKey}";
                var response = await httpClient.GetAsync(solarFlareApiUrl);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var flares = JsonConvert.DeserializeObject<List<SolarFlareModel>>(jsonResponse);

                return flares;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from NASA API: {ex.Message}");
                return null;
            }
        }
    }
}
