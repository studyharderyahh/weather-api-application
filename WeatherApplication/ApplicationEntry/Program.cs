﻿using System;
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
                // Get an instance of FileEncoder for handling file operations
                FileEncoder encoder = FileEncoder.GetInstance("security.sys");
                string weatherApiKey = "WeatherApiKey";
                ConfigFileReader configReader = null;

                try
                {
                    // Specify the file path
                    string filePath = "Config/weatherAppAPIKeys.cfg";


                    // Check if the file exists
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"The file '{filePath}' does not exist.");
                        return;
                    }

                    // Create an instance of configReader
                    configReader  = new ConfigFileReader(filePath);

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

                string actualAPIKey = encoder.Read(weatherApiKey);

                // If API key is not found in the file, prompt the user to input it
                if (string.IsNullOrEmpty(actualAPIKey))
                {
                    Console.Write("Enter Weather API key: ");
                    actualAPIKey = Console.ReadLine();

                    // Write the API key to the file
                    encoder.Write("ApiKey", actualAPIKey);
                }

                if (string.IsNullOrWhiteSpace(actualAPIKey))
                {
                    Console.WriteLine("Register for an API key at https://developer.niwa.co.nz");
                    return;
                }

                // Create an instance of WeatherServiceModel with the API key
                WeatherModel weatherService = new WeatherModel(actualAPIKey);

                // Create an instance of WeatherAPIView
                WeatherApplicationView view = new WeatherApplicationView();

                // Instantiate the controller with the view and model
                WeatherAPIController controller = new WeatherAPIController(weatherService, view);

                // Specify the city name for which you want to retrieve weather data
                string cityName = "Pokeno";

                // Retrieve weather data and render the view
                await controller.RefreshWeatherData(actualAPIKey, cityName);
                controller.RefreshPanelView();
                //Run through to tidal information
                double lat = -37.406;
                double lon = 175.947;

                DateTime startDate = new DateTime(2023, 01, 01);
                DateTime endDate = new DateTime(2023, 12, 31);

                DateTime currentDate = startDate;

                actualAPIKey = "VtqRNuV5F79dsA8nPGCBhHaCeEJbocPd";

                while (currentDate <= endDate)
                {
                    string year = currentDate.Year.ToString();
                    string month = currentDate.Month.ToString("D2");
                    string filename = $"tides_{year}_{month}.json";
                    Console.WriteLine($"Downloading {currentDate:MMM} {year}");

                    // Calculate the number of days in the current month
                    int numberOfDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                    string dateString = currentDate.ToString("yyyy-MM-dd");

                    string url = $"https://api.niwa.co.nz/tides/data?lat={lat}&long={lon}&datum=MSL&numberOfDays={numberOfDays}&apikey={actualAPIKey}&startDate={dateString}";

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



        }
    }
}