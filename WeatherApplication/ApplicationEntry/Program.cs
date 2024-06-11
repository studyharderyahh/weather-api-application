using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WeatherApplication.Controllers;
using WeatherApplication.FileHandlers;
using WeatherApplication.Views;

namespace WeatherApplication.ApplicationEntry
{
    class Program
    {
        static async Task Main(string[] args)
        {

            try
            {
                // Initialize with the file path
                FileEncoder.Initialize("security.sys");

                // Get the singleton instance
                FileEncoder encoder = FileEncoder.Instance;
                string apiKeyFilePath = "Config/weatherAppAPIKeys.cfg";

                string weatherApiKey = "WeatherApiKey";
                string tideApiKey = "TideApiKey";
                

                // Added proper null checking before the initialization
                if (string.IsNullOrEmpty(apiKeyFilePath))
                {
                    throw new ArgumentNullException(nameof(apiKeyFilePath), "File path cannot be null or empty.");
                }

                ConfigFileReader configReader = new ConfigFileReader(apiKeyFilePath);
     
                try
                {
                    // Check if the file exists
                    if (!File.Exists(apiKeyFilePath))
                    {
                        Console.WriteLine($"The file '{apiKeyFilePath}' does not exist.");
                        return;
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine(configReader.GetAPIKey("appName"));
                Console.WriteLine();

                // Read the API key from the file
                // encoder.Write("ApiKey", "a173994356f879bb3e422754bfdde559");
                encoder.Write(weatherApiKey, configReader.GetAPIKey(weatherApiKey));

                string actualWeatherAPIKey = encoder.Read(weatherApiKey);

                // If API key is not found in the file, prompt the user to input it
                if (string.IsNullOrEmpty(actualWeatherAPIKey))
                {
                    Console.Write("Enter Weather API key: ");
                    actualWeatherAPIKey = Console.ReadLine();

                    // Write the API key to the file
                    encoder.Write("ApiKey", actualWeatherAPIKey);
                }

                if (string.IsNullOrWhiteSpace(actualWeatherAPIKey))
                {
                    Console.WriteLine("Register for an API key at https://developer.niwa.co.nz");
                    return;
                }

                // Create an instance of WeatherServiceModel with the API key
                WeatherModel weatherService = new WeatherModel(actualWeatherAPIKey);

                // Create an instance of WeatherAPIView
                WeatherApplicationView view = new WeatherApplicationView();

                // Instantiate the controller with the view and model
                WeatherAPIController controller = new WeatherAPIController(weatherService, view);

                // Specify the city name for which you want to retrieve weather data
                string cityName = "Chennai";

                // Retrieve weather data and render the view
                await controller.RefreshWeatherData(actualWeatherAPIKey, cityName);
                controller.RefreshPanelView();

                Console.WriteLine("\nTide API Data: ");
                Console.WriteLine("--------------------");
                //Run through to tidal information
                double lat = -37.406;
                double lon = 175.947;

                DateTime startDate = new DateTime(2023, 01, 01);
                DateTime endDate = new DateTime(2023, 12, 31);

                DateTime currentDate = startDate;

                // Remove later --- this is the tideAPIKey
                // actualAPIKey = "VtqRNuV5F79dsA8nPGCBhHaCeEJbocPd";
                string actualTideAPIKey = configReader.GetAPIKey(tideApiKey);

                while (currentDate <= endDate)
                {
                    string year = currentDate.Year.ToString();
                    string month = currentDate.Month.ToString("D2");
                    string filename = $"tides_{year}_{month}.json";
                    Console.WriteLine($"Downloading {currentDate:MMM} {year}");

                    // Calculate the number of days in the current month
                    int numberOfDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                    string dateString = currentDate.ToString("yyyy-MM-dd");

                    string url = $"https://api.niwa.co.nz/tides/data?lat={lat}&long={lon}&datum=MSL&numberOfDays={numberOfDays}&apikey={actualTideAPIKey}&startDate={dateString}";

                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            HttpResponseMessage response = await client.GetAsync(url);
                            response.EnsureSuccessStatusCode();

                            string result = await response.Content.ReadAsStringAsync();
                            File.WriteAllText(filename, result);
                        }
                        catch (HttpRequestException ex)
                        {
                            Console.WriteLine($"An error occurred while downloading tides data: {ex.Message}");
                        }
                    }

                    currentDate = currentDate.AddMonths(1);
                }

                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Below are the usage for hunting 

            string HuntingfilePath = "Config/hunting_season_data.txt";// Replace with your actual file path

            // Initialize the model, view, and controller
            var huntingModel = new HuntingModel();
            var huntingView = new HuntingView();
            var huntingController = new HuntingController(huntingModel, huntingView);

            // Load and display hunting season data
            huntingController.LoadAndDisplayHuntingSeasonData(HuntingfilePath);

        }
    }
}
