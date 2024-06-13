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
        private string UVIndexApiKey;
        private string UVIndexBaseUrl;
        private readonly HttpClient _httpClient;

        public UVService(string apiKey, string baseUrl)
        {
            UVIndexApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            UVIndexBaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            _httpClient = new HttpClient();
        }

        public async Task<UVModel> GetUVDataAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"{UVIndexBaseUrl}?lat={latitude}&long={longitude}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-apikey", UVIndexApiKey);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from API: {ex.Message}");
                return null;
            }
        }
    }
}
