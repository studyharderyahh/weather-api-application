using System;

namespace WeatherApplication.Views
{
    public class WeatherApplicationView
    {
        // Method to render weather data to the console.(gpt)
        public void Render(WeatherData weatherData)
        {
            if (null == weatherData)
            {
                Console.WriteLine("Weather data is null.");

            }
            else
            {
                // Display weather data details.
                Console.WriteLine($"Weather for {weatherData.Name}:");
                // Safe navigation with null-conditional operators.
                Console.WriteLine($"Weather Description: {weatherData.Weather?[0]?.Description}");
                Console.WriteLine($"Coordinates: Lon: {weatherData.Coord?.Lon}, Lat: {weatherData.Coord?.Lat}"); // Added null-conditional operator
                Console.WriteLine($"Weather Description: {weatherData.Weather?[0]?.Description}"); // Added null-conditional operator
                Console.WriteLine($"Base: {weatherData.Base}");
                Console.WriteLine($"Temperature: {weatherData.Main?.Temp}°C"); // Added null-conditional operator
                Console.WriteLine($"Feels Like: {weatherData.Main?.Feels_like}°C"); // Added null-conditional operator
                Console.WriteLine($"Minimum Temperature: {weatherData.Main?.Temp_min}°C"); // Added null-conditional operator
                Console.WriteLine($"Maximum Temperature: {weatherData.Main?.Temp_max}°C"); // Added null-conditional operator
                Console.WriteLine($"Pressure: {weatherData.Main?.Pressure}hPa"); // Added null-conditional operator
                Console.WriteLine($"Humidity: {weatherData.Main?.Humidity}%"); // Added null-conditional operator
                Console.WriteLine($"Visibility: {weatherData.Visibility}");
                Console.WriteLine($"Wind Speed: {weatherData.Wind?.Speed}m/s"); // Added null-conditional operator
                Console.WriteLine($"Wind Degree: {weatherData.Wind?.Deg}°"); // Added null-conditional operator
                Console.WriteLine($"Cloudiness: {weatherData.Clouds?.All}%"); // Added null-conditional operator
                Console.WriteLine($"Date and Time (UTC): {UnixTimeStampToDateTime(weatherData.Dt)}"); // Convert UNIX timestamp to DateTime
                Console.WriteLine($"Sys Info: Type: {weatherData.Sys?.Type}, ID: {weatherData.Sys?.Id}, Country: {weatherData.Sys?.Country}, Sunrise: {UnixTimeStampToDateTime(weatherData.Sys.Sunrise)}, Sunset: {UnixTimeStampToDateTime(weatherData.Sys.Sunset)}"); // Added null-conditional operator and converted UNIX timestamps
                Console.WriteLine($"Timezone Offset (Sec): {weatherData.Timezone}");
                Console.WriteLine($"City ID: {weatherData.Id}");
                Console.WriteLine($"City Name: {weatherData.Name}");
                Console.WriteLine($"Cod: {weatherData.Cod}");
            }
        }

            // Helper method to convert Unix timestamp to DateTime.
        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
        }
    }
}