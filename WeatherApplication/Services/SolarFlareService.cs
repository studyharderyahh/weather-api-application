using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApplication.Models;

namespace WeatherApplication.Services
{
    /// <summary>
    /// Service class for fetching solar flare data from NASA API.
    /// </summary>
    public class SolarFlareService
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the SolarFlareService class.
        /// </summary>
        public SolarFlareService()
        {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// Fetches solar flare data asynchronously within the specified date range.
        /// </summary>
        /// <param name="startDate">Start date of the query (format: yyyy-MM-dd).</param>
        /// <param name="endDate">End date of the query (format: yyyy-MM-dd).</param>
        /// <param name="apiKey">API key required for accessing the solar flare API.</param>
        /// <param name="solarFlareBaseUrl">Base URL of the solar flare API.</param>
        /// <returns>A list of SolarFlareModel objects representing solar flare data.</returns>
        /// <exception cref="HttpRequestException">Thrown when an HTTP request fails.</exception>
        /// <exception cref="JsonException">Thrown when JSON deserialization fails.</exception>
        public async Task<List<SolarFlareModel>> GetFlaresAsync(string startDate, string endDate, string apiKey, string solarFlareBaseUrl)
        {
            try
            {
                // Construct the API URL
                var solarFlareApiUrl = $"{solarFlareBaseUrl}{startDate}&endDate={endDate}&api_key={apiKey}";

                // Send HTTP GET request
                var response = await httpClient.GetAsync(solarFlareApiUrl);
                response.EnsureSuccessStatusCode();

                // Read response content
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize JSON response into a list of SolarFlareModel objects
                var flares = JsonConvert.DeserializeObject<List<SolarFlareModel>>(jsonResponse);

                return flares;
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request failure
                Console.WriteLine($"HTTP request error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization error
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Error fetching data from NASA API: {ex.Message}");
                throw;
            }
        }
    }
}
