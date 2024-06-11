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
        private readonly HttpClient _httpClient;

        public SolarFlareService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<SolarFlareModel>> GetFlaresAsync(string startDate, string endDate, string apiKey)
        {
            try
            {
                var apiUrl = $"https://api.nasa.gov/DONKI/FLR?startDate={startDate}&endDate={endDate}&api_key={apiKey}";
                var response = await _httpClient.GetAsync(apiUrl);
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
