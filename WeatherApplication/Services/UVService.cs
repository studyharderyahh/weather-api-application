using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
// using WeatherApplication.Models;


namespace WeatherApplication.Services
{
    /// <summary>
    /// Service class for fetching UV index data from an API.
    /// </summary>
    public class UVService
    {
        private readonly string uvIndexApiKey;
        private readonly string uvIndexBaseUrl;
        private readonly HttpClient httpClient;

        private const string ApiKeyHeader = "x-apikey";
        private const string ApiKeyNullMessage = "API key cannot be null";
        private const string BaseUrlNullMessage = "Base URL cannot be null";
        private const string ErrorMessage = "Error fetching UV data from API";

        /// <summary>
        /// Initializes a new instance of the UVService class.
        /// </summary>
        /// <param name="apiKey">API key required for accessing the UV index API.</param>
        /// <param name="baseUrl">Base URL of the UV index API.</param>
        /// <param name="httpClient">HTTP client instance used for making API requests.</param>
        /// <exception cref="ArgumentNullException">Thrown when apiKey, baseUrl, or httpClient is null.</exception>
        public UVService(string apiKey, string baseUrl, HttpClient httpClient)
        {
            uvIndexApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey), ApiKeyNullMessage);
            uvIndexBaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl), BaseUrlNullMessage);
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Retrieves UV index data asynchronously based on the provided latitude and longitude.
        /// </summary>
        /// <param name="latitude">Latitude coordinate for the location.</param>
        /// <param name="longitude">Longitude coordinate for the location.</param>
        /// <returns>An instance of UVModel containing the UV index data.</returns>
        /// <exception cref="HttpRequestException">Thrown when an HTTP request fails.</exception>
        /// <exception cref="JsonException">Thrown when JSON deserialization fails.</exception>
        public async Task<UVModel> GetUVDataAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"{uvIndexBaseUrl}?lat={latitude}&long={longitude}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add(ApiKeyHeader, uvIndexApiKey);

                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                /*
                // Log the response content and headers
                Console.WriteLine("Response Content:");
                Console.WriteLine(jsonResponse);
                Console.WriteLine("Response Headers:");
                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(",", header.Value)}");
                } */

                // Deserialize JSON response into UVModel object
                var uvModel = JsonConvert.DeserializeObject<UVModel>(jsonResponse);
                return uvModel;
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request failure
                Console.WriteLine($"{ErrorMessage}: {ex.Message}");
                throw new HttpRequestException(ErrorMessage, ex);
            }
            catch (JsonException ex)
            {
                // Handle JSON deserialization error
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
                throw new JsonException("Error parsing UV data JSON response", ex);
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"{ErrorMessage}: {ex.Message}");
                throw;
            }
        }
    }
}
