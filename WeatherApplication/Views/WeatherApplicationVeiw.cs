using System;

namespace WeatherApplication.Views
{
    /// <summary>
    /// View class responsible for rendering weather data to the console.
    /// </summary>
    public class WeatherApplicationView
    {
        /// <summary>
        /// Renders weather data to the console.
        /// </summary>
        /// <param name="weatherData">WeatherData object containing weather information.</param>
        public void Render(WeatherData weatherData)
        {
            if (weatherData == null)
            {
                Console.WriteLine("Weather data is null.");
                return;
            }

            Console.WriteLine($"Weather for {weatherData.Name}:");
            Console.WriteLine($"Weather Description: {weatherData.Weather?[0]?.Description}");
            Console.WriteLine($"Coordinates: Lon: {weatherData.Coord?.Lon}, Lat: {weatherData.Coord?.Lat}");
            Console.WriteLine($"Base: {weatherData.Base}");
            Console.WriteLine($"Temperature: {FormatTemperature(weatherData.Main?.Temp)}");
            Console.WriteLine($"Feels Like: {FormatTemperature(weatherData.Main?.Feels_like)}");
            Console.WriteLine($"Minimum Temperature: {FormatTemperature(weatherData.Main?.Temp_min)}");
            Console.WriteLine($"Maximum Temperature: {FormatTemperature(weatherData.Main?.Temp_max)}");
            Console.WriteLine($"Pressure: {weatherData.Main?.Pressure}hPa");
            Console.WriteLine($"Humidity: {weatherData.Main?.Humidity}%");
            Console.WriteLine($"Visibility: {weatherData.Visibility}");
            Console.WriteLine($"Wind Speed: {weatherData.Wind?.Speed}m/s");
            Console.WriteLine($"Wind Degree: {weatherData.Wind?.Deg}°");
            Console.WriteLine($"Cloudiness: {weatherData.Clouds?.All}%");
            Console.WriteLine($"Date and Time (UTC): {UnixTimeStampToDateTime(weatherData.Dt)}");
            Console.WriteLine($"Sys Info: Type: {weatherData.Sys?.Type}, ID: {weatherData.Sys?.Id}, Country: {weatherData.Sys?.Country}, Sunrise: {UnixTimeStampToDateTime(weatherData.Sys?.Sunrise)}, Sunset: {UnixTimeStampToDateTime(weatherData.Sys?.Sunset)}");
            Console.WriteLine($"Timezone Offset (Sec): {weatherData.Timezone}");
            Console.WriteLine($"City ID: {weatherData.Id}");
            Console.WriteLine($"City Name: {weatherData.Name}");
            Console.WriteLine($"Cod: {weatherData.Cod}");
        }

        /// <summary>
        /// Helper method to format temperature values.
        /// </summary>
        /// <param name="temperature">Temperature value in Celsius.</param>
        /// <returns>Formatted temperature string, or "N/A" if temperature is null.</returns>
        private string FormatTemperature(double? temperature)
        {
            return temperature.HasValue ? $"{temperature.Value}°C" : "N/A";
        }

        /// <summary>
        /// Helper method to convert Unix timestamp to DateTime.
        /// </summary>
        /// <param name="unixTimeStamp">Nullable long Unix timestamp value.</param>
        /// <returns>DateTime representation of the Unix timestamp in UTC, or null if input is null.</returns>
        private DateTime? UnixTimeStampToDateTime(long? unixTimeStamp)
        {
            if (unixTimeStamp.HasValue)
            {
                return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp.Value).UtcDateTime;
            }
            return null;
        }
    }
}
