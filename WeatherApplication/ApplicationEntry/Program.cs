using WeatherApplication.Controllers;
using WeatherApplication.FileHandlers;
using WeatherApplication.Views;
using Microsoft.Extensions.DependencyInjection;
using WeatherApplication.Services;


namespace WeatherApplication.ApplicationEntry
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // Initialize FileEncoder with the file path for encryption keys
            FileEncoder.Initialize("security.sys");
            FileEncoder encoder = FileEncoder.Instance;

            // Define the file path for API keys configuration file
            string apiKeyFilePath = "Config/weatherAppAPIKeys.cfg";

            // Define keys for accessing different APIs
            string weatherApiKey = "WeatherApiKey";
            string tideApiKey = "TideApiKey";

            // Create an instance of ConfigFileReader to read API keys from the config file
            ConfigFileReader configReader = new ConfigFileReader(apiKeyFilePath);

            Console.WriteLine(configReader.GetAPIKey("appName"));

            // Check if the API key configuration file exists
            if (!File.Exists(apiKeyFilePath))
            {
                Console.WriteLine($"The file '{apiKeyFilePath}' does not exist.");
                // Exit if the file does not exist
                return;
            }
            try
            {
                // Write the Weather API key to the encoder and read it back
                encoder.Write(weatherApiKey, configReader.GetAPIKey(weatherApiKey));
                // Read the API key from the file
                // encoder.Write("ApiKey", "a173994356f879bb3e422754bfdde559");
                string actualWeatherAPIKey = encoder.Read(weatherApiKey);

                // Prompt the user for the Weather API key if it is not found or empty
                if (string.IsNullOrEmpty(actualWeatherAPIKey))
                {
                    Console.Write("Enter Weather API key: ");
                    actualWeatherAPIKey = Console.ReadLine();
                    // Write the API key to the file
                    encoder.Write(weatherApiKey, actualWeatherAPIKey);
                }

                // Check if the API key is still empty or whitespace
                if (string.IsNullOrWhiteSpace(actualWeatherAPIKey))
                {
                    Console.WriteLine("Register for an API key at https://developer.niwa.co.nz");
                    // Exit if no valid API key is provided
                    return;
                }

                // Display weather data using the Weather API key
                await DisplayWeatherData(actualWeatherAPIKey);

                // Display tide data using the Tide API key
                // Remove later --- this is the tideAPIKey
                // actualAPIKey = "VtqRNuV5F79dsA8nPGCBhHaCeEJbocPd";
                await DownloadTideData(configReader.GetAPIKey(tideApiKey));

                // Load and display hunting season data
                DisplayHuntingSeasonData();

                // Display UV Index data using the UV API
                await DisplayUVIndexData();

                // Display Solar Flare data using the Solar Flare API key
                await DisplaySolarFlareData(configReader);

                Console.ReadKey();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            // Method to display weather data
            static async Task DisplayWeatherData(string apiKey)
            {
                // Create instances of the weather model, view, and controller
                var weatherService = new WeatherModel(apiKey);
                var weatherView = new WeatherApplicationView();
                var weatherController = new WeatherAPIController(weatherService, weatherView);

                Console.WriteLine("\n-----------------------------");
                Console.WriteLine("   Open Weather API Data: ");
                Console.WriteLine("-----------------------------\n");

                // Prompt the user for the city name, with a default value of "Takanini"
                string cityName = "Takanini";
                Console.WriteLine("Enter the City Name (default Takanini): ");
                string cityNameInput = Console.ReadLine();

                if (!string.IsNullOrEmpty(cityNameInput))
                {
                    cityName = cityNameInput;
                }
                // Retrieve and display weather data for the specified city
                await weatherController.RefreshWeatherData(apiKey, cityName);
                weatherController.RefreshPanelView();
            }

            // Method to download tide data
            static async Task DownloadTideData(string tideApiKey)
            {
                Console.WriteLine("\n-----------------------------");
                Console.WriteLine("   NIWA - TIDE API Data: ");
                Console.WriteLine("-----------------------------\n");

                // Define the latitude, longitude, start and end dates for the tide data request
                double tideLat = -37.406;
                double tideLon = 175.947;
                DateTime tideStartDate = new DateTime(2023, 01, 01);
                DateTime tideEndDate = new DateTime(2023, 12, 31);
                DateTime currentDate = tideStartDate;

                // Loop through each month in the date range to download tide data
                while (currentDate <= tideEndDate)
                {
                    string year = currentDate.Year.ToString();
                    string month = currentDate.Month.ToString("D2");
                    string filename = $"tides_{year}_{month}.json";
                    Console.WriteLine($"Downloading {currentDate:MMM} {year} into {filename}");

                    int numberOfDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                    string dateString = currentDate.ToString("yyyy-MM-dd");

                    // Construct the API URL with query parameters
                    string url = $"https://api.niwa.co.nz/tides/data?lat={tideLat}&long={tideLon}&datum=MSL&numberOfDays={numberOfDays}&apikey={tideApiKey}&startDate={dateString}";

                    // Use HttpClient to make the API request and save the response to a file
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

                    currentDate = currentDate.AddMonths(1); // Move to the next month
                }

                Console.WriteLine("Done");
            }

            // Method to load and display hunting season data
            static void DisplayHuntingSeasonData()
            {
                // Define the file path for hunting season data
                string huntingFilePath = "Config/hunting_season_data.txt";

                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("   NIWA - Hunting Season API Data: ");
                Console.WriteLine("-----------------------------------\n");

                // Create instances of the hunting model, view, and controller
                var huntingModel = new HuntingModel();
                var huntingView = new HuntingView();
                var huntingController = new HuntingController(huntingModel, huntingView);

                // Load and display hunting season data
                huntingController.LoadAndDisplayHuntingSeasonData(huntingFilePath);
            }

            // Method to display UV Index data
            static async Task DisplayUVIndexData()
            {
                // Set up dependency injection for UV service components
                var uvServiceProvider = new ServiceCollection()
                    .AddSingleton<UVService>()
                    .AddSingleton<UVController>()
                    .AddSingleton<UVView>()
                    .BuildServiceProvider();

                // Get instances of UV controller and view
                var uvController = uvServiceProvider.GetService<UVController>();
                var uvView = uvServiceProvider.GetService<UVView>();

                //  Get latitude and longitude from user input or use default values
                double uvLat = -39.0;
                double uvLong = 174.0;

                Console.WriteLine("\n-----------------------------");
                Console.WriteLine("   NIWA - UV Index API Data: ");
                Console.WriteLine("-----------------------------\n");

                // Prompt the user for latitude and longitude values
                Console.WriteLine("Enter latitude (default -39.0): ");
                var latInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(latInput) && double.TryParse(latInput, out var lat))
                {
                    uvLat = lat;
                }

                Console.WriteLine("Enter longitude (default 174.0): ");
                var longInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(longInput) && double.TryParse(longInput, out var lon))
                {
                    uvLong = lon;
                }

                // Retrieve and display UV data for the specified location
                var uvModel = await uvController.GetUVDataAsync(uvLat, uvLong);
                uvView.DisplayUVData(uvModel);
            }
        }

        // Method to display solar flare data
        private static async Task DisplaySolarFlareData(ConfigFileReader configReader)
        {
            // Set up dependency injection for solar flare service components
            var solarFlareServiceProvider = new ServiceCollection()
                .AddSingleton<SolarFlareService>()
                .AddSingleton<SolarFlareController>()
                .AddSingleton<SolarFlareView>()
                .BuildServiceProvider();

            // Get instance of the solar flare controller
            var solarFlareController = solarFlareServiceProvider.GetService<SolarFlareController>();

            // Define the start and end dates for the solar flare data request
            string solarFlareStartDate = "2024-05-01";
            string solarFlareEndDate = "2024-05-02";

            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine("   NASA - Solar Flare Index API Data: ");
            Console.WriteLine("----------------------------------------\n");

            // Retrieve and display solar flare data for the specified date range
            await solarFlareController.GetFlaresAndDisplayAsync(solarFlareStartDate, solarFlareEndDate, configReader.GetAPIKey("SolarFlareApiKey"));
        }
    }
}
