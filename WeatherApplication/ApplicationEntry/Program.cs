using WeatherApplication.Controllers;
using WeatherApplication.FileHandlers;
using WeatherApplication.Views;
using Microsoft.Extensions.DependencyInjection;
using WeatherApplication.Services;


namespace WeatherApplication.ApplicationEntry
{
    class Program
    {
        public static Logger logger = Logger.Instance("Logs/application.log");

        static async Task Main(string[] args)
        {

            logger.LogInfo("Application started.");

            // Define the file path for API keys configuration file
            string weatherAppConfigFile = "Config/weatherAppConfigFile.json";

            logger.LogInfo($"Reading config file: {weatherAppConfigFile}");

            // Define keys for accessing different APIs
            string weatherApiKey = "WeatherApiKey";
            string tideApiKey = "TideApiKey";
            string uvApiKey = "UVIndexApiKey";
            string solarFlareApiKey = "SolarFlareApiKey";
            string weatherBaseUrl = "WatherBaseUrl";
            string tidesBaseUrl = "TidesBaseUrl";
            string uvBaseUrl = "UVIndexBaseUrl";
            string solarFlareBaseUrl = "SolarFlareBaseUrl";


            // Check if the API key configuration file exists
            if (!File.Exists(weatherAppConfigFile))
            {
                logger.LogError($"The file '{weatherAppConfigFile}' does not exist.");
                Console.WriteLine($"The file '{weatherAppConfigFile}' does not exist.");
                // Exit if the file does not exist
                return;
            }
            try
            {
                // Create an instance of ConfigFileReader to read API keys from the config file
                ConfigFileReader configReader = new ConfigFileReader(weatherAppConfigFile);

                // Initialize FileEncoder with the file path for encryption keys
                FileEncoder.Initialize("security.sys", configReader.GetKeyValue("EncryptionKey"));
                FileEncoder encoder = FileEncoder.Instance;

                logger.LogInfo($"Application Name: {configReader.GetKeyValue("appName")}");
                Console.WriteLine(configReader.GetKeyValue("appName"));

                // Write the Weather API key to the encoder and read it back
                encoder.Write(weatherApiKey, configReader.GetKeyValue(weatherApiKey));

                // Read the API key from the file
                // encoder.Write("ApiKey", "a173994356f879bb3e422754bfdde559");
                string actualWeatherAPIKey = encoder.Read(weatherApiKey);

                // Prompt the user for the Weather API key if it is not found or empty
                if (string.IsNullOrEmpty(actualWeatherAPIKey))
                {
                    Console.Write("Enter Weather API key ");
                    actualWeatherAPIKey = Console.ReadLine();
                    // Write the API key to the file
                    encoder.Write(weatherApiKey, actualWeatherAPIKey);
                }

                // Check if the API key is still empty or whitespace
                if (string.IsNullOrWhiteSpace(actualWeatherAPIKey))
                {
                    logger.LogWarning("No valid API key is provided. Register for API Key at https://developer.niwa.co.nz");
                    Console.WriteLine("Register for an API key at https://developer.niwa.co.nz");
                    return;
                }

                // Display weather data using the Weather API key
                await DisplayWeatherData(actualWeatherAPIKey, configReader.GetKeyValue(weatherBaseUrl));

                // Display tide data using the Tide API key
                // Remove later --- this is the tideAPIKey
                // actualAPIKey = "VtqRNuV5F79dsA8nPGCBhHaCeEJbocPd";
                await DownloadTideData(configReader.GetKeyValue(tideApiKey), configReader.GetKeyValue(tidesBaseUrl));

                // Load and display hunting season data
                DisplayHuntingSeasonData();

                // Display UV Index data using the UV API
                await DisplayUVIndexData(configReader.GetKeyValue(uvApiKey), configReader.GetKeyValue(uvBaseUrl));

                // Display Solar Flare data using the Solar Flare API key
                await DisplaySolarFlareData(configReader.GetKeyValue(solarFlareApiKey), configReader.GetKeyValue(solarFlareBaseUrl));

                Console.ReadKey();

            }
            catch (Exception ex)
            {   
                logger.LogError($"An error occurred: {ex.Message}");
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            logger.LogInfo("Application finished.");
        }

        // Method to display weather data
        private static async Task DisplayWeatherData(string apiKey, string weatherBaseUrl)
        {

            logger.LogInfo("Fetching Open Weather API Data");
            Console.WriteLine("\n-----------------------------");
            Console.WriteLine("   Open Weather API Data ");
            Console.WriteLine("-----------------------------\n");

            // Create instances of the weather model, view, and controller
            var weatherService = new WeatherModel(apiKey);
            var weatherView = new WeatherApplicationView();
            var weatherController = new WeatherAPIController(weatherService, weatherView);



            // Prompt the user for the city name, with a default value of "Takanini"
            string cityName = "Takanini";
            Console.WriteLine("Enter the City Name (default Takanini): ");
            string cityNameInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(cityNameInput))
            {
                cityName = cityNameInput;
            }

            // Retrieve and display weather data for the specified city
            await weatherController.RefreshWeatherData(apiKey, cityName, weatherBaseUrl);
            weatherController.RefreshPanelView();
        }

        // Method to download tide data
        private static async Task DownloadTideData(string tideApiKey, string tidesBaseUrl)
        {

            logger.LogInfo("Fetching NIWA Tide API Data");
            Console.WriteLine("\n-----------------------------");
            Console.WriteLine("   NIWA - TIDE API Data ");
            Console.WriteLine("-----------------------------\n");
            
            // Create instances of the weather model, view, and controller
            var tidesService = new TidesModel();
            var tidesView = new TidesView();
            var tidesController = new TidesController(tidesService, tidesView);

            // Define the latitude, longitude, start and end dates for the tide data request
            double tideLat = -37.406;
            double tideLon = 175.947;
            DateTime tideStartDate = new DateTime(2023, 01, 01);
            DateTime tideEndDate = new DateTime(2023, 12, 31);
            DateTime currentDate = tideStartDate;

            // RefreshTidesData(double lat, double lon, string apiKey, DateTime startDate, DateTime endDate)
            await tidesController.RefreshTidesData(tideLat,tideLon, tideApiKey,tideStartDate,tideEndDate, tidesBaseUrl);

        }

        // Method to load and display hunting season data
        private static void DisplayHuntingSeasonData()
        {
            logger.LogInfo("Fetching NIWA Hunting Seasons API Data");
            Console.WriteLine("\n-----------------------------------");
            Console.WriteLine("   NIWA - Hunting Season API Data ");
            Console.WriteLine("-----------------------------------\n");

            // Define the file path for hunting season data
            string huntingFilePath = "Config/hunting_season_data.txt";


            // Create instances of the hunting model, view, and controller
            var huntingModel = new HuntingModel();
            var huntingView = new HuntingView();
            var huntingController = new HuntingController(huntingModel, huntingView);

            // Load and display hunting season data
            huntingController.LoadAndDisplayHuntingSeasonData(huntingFilePath);
        }

        // Method to display UV Index data
        private static async Task DisplayUVIndexData(string uvApiKey, string uvBaseUrl)
        {

            logger.LogInfo("Fetching NIWA - UV Index API Data");
            Console.WriteLine("\n-----------------------------");
            Console.WriteLine("   NIWA - UV Index API Data ");
            Console.WriteLine("-----------------------------\n");

            // Set up dependency injection for UV service components
            var uvServiceProvider = new ServiceCollection()
                .AddSingleton<UVService>(provider => new UVService(uvApiKey, uvBaseUrl))
                .AddSingleton<UVController>()
                .AddSingleton<UVView>()
                .BuildServiceProvider();

            // Get instances of UV controller and view
            var uvController = uvServiceProvider.GetService<UVController>();
            var uvView = uvServiceProvider.GetService<UVView>();

            //  Get latitude and longitude from user input or use default values
            double uvLat = -39.0;
            double uvLong = 174.0;

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

        // Method to display solar flare data
        private static async Task DisplaySolarFlareData(string solarFlareApiKey, string solarFlareBaseUrl)
        {
            logger.LogInfo("Fetching NASA - Solar Flare Index API Data");
            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine("   NASA - Solar Flare Index API Data ");
            Console.WriteLine("----------------------------------------\n");

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


            // Retrieve and display solar flare data for the specified date range
            await solarFlareController.GetFlaresAndDisplayAsync(solarFlareStartDate, solarFlareEndDate, solarFlareApiKey, solarFlareBaseUrl);
        }


    }
}
