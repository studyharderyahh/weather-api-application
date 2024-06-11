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
                WeatherApplicationView weatherView = new WeatherApplicationView();

                // Instantiate the controller with the view and model
                WeatherAPIController weatherController = new WeatherAPIController(weatherService, weatherView);

                Console.WriteLine("\n-----------------------------");
                Console.WriteLine("   Open Weather API Data: ");
                Console.WriteLine("-----------------------------\n");

                // Specify the city name for which you want to retrieve weather data
                string cityName = "Takanini";

                Console.WriteLine("Enter the City Name (default Takanini): ");
                var cityNameInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(cityNameInput))
                {
                    cityName = cityNameInput;
                }

                // Retrieve weather data and render the view
                await weatherController.RefreshWeatherData(actualWeatherAPIKey, cityName);
                weatherController.RefreshPanelView();

                Console.WriteLine("\n-----------------------------");
                Console.WriteLine("   NIWA - TIDE API Data: ");
                Console.WriteLine("-----------------------------\n");
                //Run through to tidal information
                double tideLat = -37.406;
                double tideLon = 175.947;

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
                    Console.WriteLine($"Downloading {currentDate:MMM} {year} into {filename}");

                    // Calculate the number of days in the current month
                    int numberOfDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                    string dateString = currentDate.ToString("yyyy-MM-dd");

                    string url = $"https://api.niwa.co.nz/tides/data?lat={tideLat}&long={tideLon}&datum=MSL&numberOfDays={numberOfDays}&apikey={actualTideAPIKey}&startDate={dateString}";

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

            string HuntingfilePath = "Config/hunting_season_data.txt";
            // Initialize the model, view, and controller
            var huntingModel = new HuntingModel();
            var huntingView = new HuntingView();
            var huntingController = new HuntingController(huntingModel, huntingView);

            // Load and display hunting season data
            huntingController.LoadAndDisplayHuntingSeasonData(HuntingfilePath);


            // UV Index API
            // Setup Dependency Injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<UVService>()
                .AddSingleton<UVController>()
                .AddSingleton<UVView>()
                .BuildServiceProvider();

            // Get services
            var uvController = serviceProvider.GetService<UVController>();
            var uvView = serviceProvider.GetService<UVView>();

            // Get latitude and longitude from user input or use default values
            double uvLat = -39.0;
            double uvLong = 174.0;

            Console.WriteLine("\n-----------------------------");
            Console.WriteLine("   NIWA - UV Index API Data: ");
            Console.WriteLine("-----------------------------\n");

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

            // Get UV data
            var uvModel = await uvController.GetUVDataAsync(uvLat, uvLong);

            // Display data
            uvView.DisplayUVData(uvModel);

        }
    }
}
