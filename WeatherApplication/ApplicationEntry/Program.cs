using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WeatherApplication.Controllers;
using WeatherApplication.FileHandlers;
using WeatherApplication.Services;
using WeatherApplication.Views;

namespace WeatherApplication.ApplicationEntry
{
    class Program
    {
        public static Logger logger = Logger.Instance("Logs/application.log");

        static async Task Main(string[] args)
        {
            logger.LogInfo("Application started.");

            string weatherAppConfigFile = "Config/weatherAppConfigFile.json";

            if (!CheckConfigFileExists(weatherAppConfigFile))
            {
                return;
            }

            var configReader = InitializeConfigReader(weatherAppConfigFile);
            if (configReader == null)
            {
                return;
            }

            var encoder = InitializeFileEncoder(configReader);
            if (encoder == null)
            {
                return;
            }

            await ProcessAPIs(configReader, encoder);

            logger.LogInfo("Application finished.");
        }

        private static bool CheckConfigFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                logger.LogError($"The file '{filePath}' does not exist.");
                Console.WriteLine($"The file '{filePath}' does not exist.");
                return false;
            }
            return true;
        }

        private static ConfigFileReader InitializeConfigReader(string configFilePath)
        {
            try
            {
                logger.LogInfo($"Reading config file: {configFilePath}");
                return new ConfigFileReader(configFilePath);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error reading config file: {ex.Message}");
                Console.WriteLine($"Error reading config file: {ex.Message}");
                return null;
            }
        }

        private static FileEncoder InitializeFileEncoder(ConfigFileReader configReader)
        {
            try
            {
                FileEncoder.Initialize("security.sys", configReader.GetKeyValue("EncryptionKey"));
                return FileEncoder.Instance;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error initializing FileEncoder: {ex.Message}");
                Console.WriteLine($"Error initializing FileEncoder: {ex.Message}");
                return null;
            }
        }

        private static async Task ProcessAPIs(ConfigFileReader configReader, FileEncoder encoder)
        {
            string weatherApiKey = GetApiKey("WeatherApiKey", configReader, encoder);
            if (string.IsNullOrWhiteSpace(weatherApiKey))
            {
                PromptForApiKey("Weather API", weatherApiKey, encoder);
            }

            string tideApiKey = configReader.GetKeyValue("TideApiKey");
            string uvApiKey = configReader.GetKeyValue("UVIndexApiKey");
            string solarFlareApiKey = configReader.GetKeyValue("SolarFlareApiKey");

            await DisplayWeatherData(weatherApiKey, configReader.GetKeyValue("WeatherBaseUrl"));
            await DownloadTideData(tideApiKey, configReader.GetKeyValue("TidesBaseUrl"));
            DisplayHuntingSeasonData();
            await DisplayUVIndexData(uvApiKey, configReader.GetKeyValue("UVIndexBaseUrl"));
            await DisplaySolarFlareData(solarFlareApiKey, configReader.GetKeyValue("SolarFlareBaseUrl"));
        }

        private static string GetApiKey(string keyName, ConfigFileReader configReader, FileEncoder encoder)
        {
            encoder.Write(keyName, configReader.GetKeyValue(keyName));
            return encoder.Read(keyName);
        }

        private static void PromptForApiKey(string apiName, string apiKey, FileEncoder encoder)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                Console.Write($"Enter {apiName} key: ");
                apiKey = Console.ReadLine();
                encoder.Write(apiName, apiKey);
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                logger.LogWarning($"No valid {apiName} key is provided. Register for API Key at the appropriate site.");
                Console.WriteLine($"Register for an API key at the appropriate site.");
            }
        }

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
            await tidesController.RefreshTidesData(tideLat, tideLon, tideApiKey, tideStartDate, tideEndDate, tidesBaseUrl);

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
