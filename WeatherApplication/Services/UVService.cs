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
    public class UVService
    {
        private readonly string uvIndexApiKey;
        private readonly string uvIndexBaseUrl;
        private readonly HttpClient httpClient;

        private const string ApiKeyHeader = "x-apikey";
        private const string ApiKeyNullMessage = "API key cannot be null";
        private const string BaseUrlNullMessage = "Base URL cannot be null";
        private const string ErrorMessage = "Error fetching data from API";

        public UVService(string apiKey, string baseUrl)
        {
            uvIndexApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey), ApiKeyNullMessage);
            uvIndexBaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl), BaseUrlNullMessage);
            httpClient = new HttpClient();
        }

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

                var uvModel = JsonConvert.DeserializeObject<UVModel>(jsonResponse);
                return uvModel;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ErrorMessage}: {ex.Message}");
                return null;
            }
        }
    }
}
