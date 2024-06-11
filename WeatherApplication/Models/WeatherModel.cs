using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherApplication.APIHelpers;

namespace WeatherApplication
{
    public class WeatherModel
    {
        private readonly HttpClientWrapper m_httpClient = HttpClientWrapper.Instance; 
        private readonly string m_apiKey;

        public WeatherModel(string apiKey)
        {
            m_apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public async Task<WeatherData> GetWeatherAsync(string apiKey, string cityName)
        {
            if (string.IsNullOrWhiteSpace(cityName))
            {
                throw new ArgumentException("City name cannot be null or empty.", nameof(cityName));
            }
            try
            {
                string encodedCityName = Uri.EscapeDataString(cityName);
                string encodedApiKey = Uri.EscapeDataString(m_apiKey);
                string url = $"https://api.openweathermap.org/data/2.5/weather?q={encodedCityName}&appid={encodedApiKey}&units=metric";
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

    public class WeatherData
    {
        public CoordInfo Coord { get; }
        public List<WeatherInfo> Weather { get; }
        public string Base { get; }
        public MainInfo Main { get; }
        public int Visibility { get; }
        public WindInfo Wind { get; }
        public CloudsInfo Clouds { get; }
        public long Dt { get; }
        public SysInfo Sys { get; }
        public int Timezone { get; }
        public int Id { get; }
        public string Name { get; }
        public int Cod { get; }

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
            Weather = weather;
            Base = @base;
            Main = main;
            Visibility = visibility;
            Wind = wind;
            Clouds = clouds;
            Dt = dt;
            Sys = sys;
            Timezone = timezone;
            Id = id;
            Name = name;
            Cod = cod;
        }
    }

    public class CoordInfo
    {
        public double Lon { get; }
        public double Lat { get; }

        public CoordInfo(double lon, double lat)
        {
            Lon = lon;
            Lat = lat;
        }
    }

    public class WeatherInfo
    {
        public int Id { get; }
        public string Main { get; }
        public string Description { get; }
        public string Icon { get; }

        public WeatherInfo(int id, string main, string description, string icon)
        {
            Id = id;
            Main = main ?? throw new ArgumentNullException(nameof(main));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Icon = icon ?? throw new ArgumentNullException(nameof(icon));
        }
    }

    public class MainInfo
    {
        public double Temp { get; }
        public double Feels_like { get; }
        public double Temp_min { get; }
        public double Temp_max { get; }
        public int Pressure { get; }
        public int Humidity { get; }

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

    public class WindInfo
    {
        public double Speed { get; }
        public int Deg { get; }

        public WindInfo(double speed, int deg)
        {
            Speed = speed;
            Deg = deg;
        }
    }

    public class CloudsInfo
    {
        public int All { get; }

        public CloudsInfo(int all)
        {
            All = all;
        }
    }

    public class SysInfo
    {
        public int Type { get; }
        public int Id { get; }
        public string Country { get; }
        public long Sunrise { get; }
        public long Sunset { get; }

        public SysInfo(int type, int id, string country, long sunrise, long sunset)
        {
            Type = type;
            Id = id;
            Country = country ?? throw new ArgumentNullException(nameof(country));
            Sunrise = sunrise;
            Sunset = sunset;
        }
    }

    public class WeatherDataDeserializer
    {
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

    public class WeatherServiceException : Exception
    {
        public WeatherServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
