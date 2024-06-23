using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherApplication.APIHelpers;

namespace WeatherApplication
{
    /// <summary>
    /// Represents a model for retrieving weather data from an API.
    /// </summary>
    public class WeatherModel
    {
        private readonly HttpClientWrapper m_httpClient = HttpClientWrapper.Instance;

        /// <summary>
        /// Initializes a new instance of the WeatherModel class with the specified API key.
        /// </summary>
        /// <param name="apiKey">The API key for accessing the weather data.</param>
        /// <exception cref="ArgumentNullException">Thrown when apiKey is null.</exception>
        public WeatherModel()
        {
            //m_apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        /// <summary>
        /// Retrieves weather data asynchronously for a specified city.
        /// </summary>
        /// <param name="cityName">The name of the city for which weather data is requested.</param>
        /// <param name="weatherBaseUrl">The base URL of the weather API.</param>
        /// <returns>A Task representing the asynchronous operation, returning WeatherData.</returns>
        /// <exception cref="ArgumentException">Thrown when cityName is null or empty.</exception>
        /// <exception cref="WeatherServiceException">Thrown for various weather service errors, including HTTP and JSON parsing errors.</exception>
        public async Task<WeatherData> GetWeatherAsync(string apiKey, string cityName, string weatherBaseUrl)
        {
            if (string.IsNullOrWhiteSpace(cityName))
            {
                throw new ArgumentException("City name cannot be null or empty.", nameof(cityName));
            }

            try
            {
                string encodedCityName = Uri.EscapeDataString(cityName);
                string encodedApiKey = Uri.EscapeDataString(apiKey);
                string url = $"{weatherBaseUrl}{encodedCityName}&appid={encodedApiKey}&units=metric";

                HttpResponseMessage response = await m_httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                WeatherDataDeserializer deserializer = new WeatherDataDeserializer();
                WeatherData weatherData = deserializer.DeserializeWeatherData(json) ?? throw new Exception("Weather data deserialization failed.");
                return weatherData;
            }
            catch (HttpRequestException ex)
            {
                throw new WeatherServiceException("Error occurred while sending the HTTP request.", ex);
            }
            catch (JsonException ex)
            {
                throw new WeatherServiceException("Error occurred while parsing JSON response.", ex);
            }
            catch (Exception ex)
            {
                throw new WeatherServiceException("An error occurred during the asynchronous operation.", ex);
            }
        }
    }

    /// <summary>
    /// Represents weather data retrieved from the weather API.
    /// </summary>
    public class WeatherData
    {
        /// <summary>
        /// Gets the coordinates of the location.
        /// </summary>
        public CoordInfo Coord { get; }

        /// <summary>
        /// Gets the weather information.
        /// </summary>
        public List<WeatherInfo> Weather { get; }

        /// <summary>
        /// Gets the base information.
        /// </summary>
        public string Base { get; }

        /// <summary>
        /// Gets the main weather information.
        /// </summary>
        public MainInfo Main { get; }

        /// <summary>
        /// Gets the visibility.
        /// </summary>
        public int Visibility { get; }

        /// <summary>
        /// Gets the wind information.
        /// </summary>
        public WindInfo Wind { get; }

        /// <summary>
        /// Gets the clouds information.
        /// </summary>
        public CloudsInfo Clouds { get; }

        /// <summary>
        /// Gets the date and time of data calculation.
        /// </summary>
        public long Dt { get; }

        /// <summary>
        /// Gets the sys information.
        /// </summary>
        public SysInfo Sys { get; }

        /// <summary>
        /// Gets the timezone offset.
        /// </summary>
        public int Timezone { get; }

        /// <summary>
        /// Gets the city ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the city name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the response code.
        /// </summary>
        public int Cod { get; }

        /// <summary>
        /// Initializes a new instance of the WeatherData class with the provided parameters.
        /// </summary>
        /// <param name="coord">The coordinates of the location.</param>
        /// <param name="weather">The weather information.</param>
        /// <param name="base">The base information.</param>
        /// <param name="main">The main weather information.</param>
        /// <param name="visibility">The visibility.</param>
        /// <param name="wind">The wind information.</param>
        /// <param name="clouds">The clouds information.</param>
        /// <param name="dt">The date and time of data calculation.</param>
        /// <param name="sys">The sys information.</param>
        /// <param name="timezone">The timezone offset.</param>
        /// <param name="id">The city ID.</param>
        /// <param name="name">The city name.</param>
        /// <param name="cod">The response code.</param>
        public WeatherData(
            CoordInfo coord,
            List<WeatherInfo> weather,
            string @base,
            MainInfo main,
            int visibility,
            WindInfo wind,
            CloudsInfo clouds,
            long dt,
            SysInfo sys,
            int timezone,
            int id,
            string name,
            int cod)
        {
            Coord = coord;
            Weather = weather ?? throw new ArgumentNullException(nameof(weather));
            Base = @base ?? throw new ArgumentNullException(nameof(@base));
            Main = main ?? throw new ArgumentNullException(nameof(main));
            Visibility = visibility;
            Wind = wind ?? throw new ArgumentNullException(nameof(wind));
            Clouds = clouds ?? throw new ArgumentNullException(nameof(clouds));
            Dt = dt;
            Sys = sys ?? throw new ArgumentNullException(nameof(sys));
            Timezone = timezone;
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Cod = cod;
        }
    }

    /// <summary>
    /// Represents the coordinates of a location.
    /// </summary>
    public class CoordInfo
    {
        /// <summary>
        /// Gets the longitude.
        /// </summary>
        public double Lon { get; }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        public double Lat { get; }

        /// <summary>
        /// Initializes a new instance of the CoordInfo class with the provided longitude and latitude.
        /// </summary>
        /// <param name="lon">The longitude.</param>
        /// <param name="lat">The latitude.</param>
        public CoordInfo(double lon, double lat)
        {
            Lon = lon;
            Lat = lat;
        }
    }

    /// <summary>
    /// Represents weather information.
    /// </summary>
    public class WeatherInfo
    {
        /// <summary>
        /// Gets the weather condition ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the main weather description.
        /// </summary>
        public string Main { get; }

        /// <summary>
        /// Gets the detailed weather description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the weather icon ID.
        /// </summary>
        public string Icon { get; }

        /// <summary>
        /// Initializes a new instance of the WeatherInfo class with the provided parameters.
        /// </summary>
        /// <param name="id">The weather condition ID.</param>
        /// <param name="main">The main weather description.</param>
        /// <param name="description">The detailed weather description.</param>
        /// <param name="icon">The weather icon ID.</param>
        /// <exception cref="ArgumentNullException">Thrown when main, description, or icon is null.</exception>
        public WeatherInfo(int id, string main, string description, string icon)
        {
            Id = id;
            Main = main ?? throw new ArgumentNullException(nameof(main));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Icon = icon ?? throw new ArgumentNullException(nameof(icon));
        }
    }

    /// <summary>
    /// Represents main weather information.
    /// </summary>
    public class MainInfo
    {
        /// <summary>
        /// Gets the temperature.
        /// </summary>
        public double Temp { get; }

        /// <summary>
        /// Gets the "feels like" temperature.
        /// </summary>
        public double Feels_like { get; }

        /// <summary>
        /// Gets the minimum temperature.
        /// </summary>
        public double Temp_min { get; }

        /// <summary>
        /// Gets the maximum temperature.
        /// </summary>
        public double Temp_max { get; }

        /// <summary>
        /// Gets the atmospheric pressure.
        /// </summary>
        public int Pressure { get; }

        /// <summary>
        /// Gets the humidity.
        /// </summary>
        public int Humidity { get; }

        /// <summary>
        /// Initializes a new instance of the MainInfo class with the provided parameters.
        /// </summary>
        /// <param name="temp">The temperature.</param>
        /// <param name="feels_like">The "feels like" temperature.</param>
        /// <param name="temp_min">The minimum temperature.</param>
        /// <param name="temp_max">The maximum temperature.</param>
        /// <param name="pressure">The atmospheric pressure.</param>
        /// <param name="humidity">The humidity.</param>
        public MainInfo(double temp, double feels_like, double temp_min, double temp_max, int pressure, int humidity)
        {
            Temp = temp;
            Feels_like = feels_like;
            Temp_min = temp_min;
            Temp_max = temp_max;
            Pressure = pressure;
            Humidity = humidity;
        }
    }

    /// <summary>
    /// Represents wind information.
    /// </summary>
    public class WindInfo
    {
        /// <summary>
        /// Gets the wind speed.
        /// </summary>
        public double Speed { get; }

        /// <summary>
        /// Gets the wind direction in degrees.
        /// </summary>
        public int Deg { get; }

        /// <summary>
        /// Initializes a new instance of the WindInfo class with the provided parameters.
        /// </summary>
        /// <param name="speed">The wind speed.</param>
        /// <param name="deg">The wind direction in degrees.</param>
        public WindInfo(double speed, int deg)
        {
            Speed = speed;
            Deg = deg;
        }
    }

    /// <summary>
    /// Represents cloud information.
    /// </summary>
    public class CloudsInfo
    {
        /// <summary>
        /// Gets the cloudiness percentage.
        /// </summary>
        public int All { get; }

        /// <summary>
        /// Initializes a new instance of the CloudsInfo class with the provided cloudiness percentage.
        /// </summary>
        /// <param name="all">The cloudiness percentage.</param>
        public CloudsInfo(int all)
        {
            All = all;
        }
    }

    /// <summary>
    /// Represents sys information.
    /// </summary>
    public class SysInfo
    {
        /// <summary>
        /// Gets the type of sys information.
        /// </summary>
        public int Type { get; }

        /// <summary>
        /// Gets the city ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the country code.
        /// </summary>
        public string Country { get; }

        /// <summary>
        /// Gets the sunrise time (Unix timestamp).
        /// </summary>
        public long Sunrise { get; }

        /// <summary>
        /// Gets the sunset time (Unix timestamp).
        /// </summary>
        public long Sunset { get; }

        /// <summary>
        /// Initializes a new instance of the SysInfo class with the provided parameters.
        /// </summary>
        /// <param name="type">The type of sys information.</param>
        /// <param name="id">The city ID.</param>
        /// <param name="country">The country code.</param>
        /// <param name="sunrise">The sunrise time (Unix timestamp).</param>
        /// <param name="sunset">The sunset time (Unix timestamp).</param>
        /// <exception cref="ArgumentNullException">Thrown when country is null.</exception>
        public SysInfo(int type, int id, string country, long sunrise, long sunset)
        {
            Type = type;
            Id = id;
            Country = country ?? throw new ArgumentNullException(nameof(country));
            Sunrise = sunrise;
            Sunset = sunset;
        }
    }

    /// <summary>
    /// Deserializes JSON data into a WeatherData object.
    /// </summary>
    public class WeatherDataDeserializer
    {
        /// <summary>
        /// Deserializes JSON data into a WeatherData object.
        /// </summary>
        /// <param name="json">The JSON string containing weather data.</param>
        /// <returns>A WeatherData object representing the deserialized data.</returns>
        /// <exception cref="Exception">Thrown when required JSON properties are missing.</exception>
        public WeatherData DeserializeWeatherData(string json)
        {
            var jsonObject = JObject.Parse(json);

            var coordObject = jsonObject["coord"] ?? throw new Exception("Coord object is missing in JSON.");
            var weatherArray = jsonObject["weather"] ?? throw new Exception("Weather array is missing in JSON.");
            var mainObject = jsonObject["main"] ?? throw new Exception("Main object is missing in JSON.");
            var windObject = jsonObject["wind"] ?? throw new Exception("Wind object is missing in JSON.");
            var cloudsObject = jsonObject["clouds"] ?? throw new Exception("Clouds object is missing in JSON.");
            var sysObject = jsonObject["sys"] ?? throw new Exception("Sys object is missing in JSON.");

            var weatherArrayFirstElement = weatherArray[0] ?? throw new Exception("First element missing from weather array in JSON.");

            long sunriseDateTime = sysObject.Value<long>("sunrise");
            long sunsetDateTime = sysObject.Value<long>("sunset");

            // Coordinate information
            var lon = coordObject.Value<double>("lon");
            var lat = coordObject.Value<double>("lat");
            var coordInfo = new CoordInfo(lon, lat);

            // Weather information
            var id = weatherArrayFirstElement.Value<int>("id");
            var main = weatherArrayFirstElement.Value<string>("main") ?? throw new Exception("Main is missing in weather info.");
            var description = weatherArrayFirstElement.Value<string>("description") ?? throw new Exception("Description is missing in weather info.");
            var icon = weatherArrayFirstElement.Value<string>("icon") ?? throw new Exception("Icon is missing in weather info.");
            var weatherInfo = new WeatherInfo(id, main, description, icon);
            var weatherList = new List<WeatherInfo> { weatherInfo };

            // Base
            var baseInfo = jsonObject.Value<string>("base") ?? throw new Exception("Base is missing in JSON.");

            // Main information
            var temp = mainObject.Value<double>("temp");
            var feelsLike = mainObject.Value<double>("feels_like");
            var tempMin = mainObject.Value<double>("temp_min");
            var tempMax = mainObject.Value<double>("temp_max");
            var pressure = mainObject.Value<int>("pressure");
            var humidity = mainObject.Value<int>("humidity");
            var mainInfo = new MainInfo(temp, feelsLike, tempMin, tempMax, pressure, humidity);

            // Visibility
            var visibility = jsonObject.Value<int>("visibility");

            // Wind information
            var windSpeed = windObject.Value<double>("speed");
            var windDeg = windObject.Value<int>("deg");
            var windInfo = new WindInfo(windSpeed, windDeg);

            // Clouds information
            var cloudsAll = cloudsObject.Value<int>("all");
            var cloudsInfo = new CloudsInfo(cloudsAll);

            // Date and time
            var dt = jsonObject.Value<long>("dt");

            // Sys information
            var sysType = sysObject.Value<int>("type");
            var sysId = sysObject.Value<int>("id");
            var sysCountry = sysObject.Value<string>("country") ?? throw new Exception("Country is missing in sys info.");
            var sysSunrise = sysObject.Value<long>("sunrise");
            var sysSunset = sysObject.Value<long>("sunset");
            var sysInfo = new SysInfo(sysType, sysId, sysCountry, sysSunrise, sysSunset);

            // Timezone, ID, Name, and Cod
            var timezone = jsonObject.Value<int>("timezone");
            var name = jsonObject.Value<string>("name") ?? throw new Exception("Name is missing in JSON.");
            var cod = jsonObject.Value<int>("cod");

            // Create WeatherData object
            var weatherData = new WeatherData(coordInfo,
                                                    weatherList,
                                                    baseInfo,
                                                    mainInfo,
                                                    visibility,
                                                    windInfo,
                                                    cloudsInfo,
                                                    dt,
                                                    sysInfo,
                                                    timezone,
                                                    id,
                                                    name,
                                                    cod);
            return weatherData;
        }
    }

    /// <summary>
    /// Represents an exception thrown by the weather service.
    /// </summary>
    public class WeatherServiceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the WeatherServiceException class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that caused the current exception.</param>
        public WeatherServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
