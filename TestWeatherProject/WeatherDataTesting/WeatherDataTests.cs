using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication;

namespace TestWeatherProject.WeatherDataTests
{
    public class WeatherDataTests
    {
        [Test]
        public void DeserializeWeatherData_ReturnsWeatherDataObject()
        {
            // Arrange
            string json = "{\"coord\":{\"lon\":174.8799,\"lat\":-36.9928},\"weather\":[{\"id\":802,\"main\":\"Clouds\",\"description\":\"scattered clouds\",\"icon\":\"03d\"}],\"base\":\"stations\",\"main\":{\"temp\":21.36,\"feels_like\":21.64,\"temp_min\":20.52,\"temp_max\":22.04,\"pressure\":1015,\"humidity\":80},\"visibility\":10000,\"wind\":{\"speed\":3.09,\"deg\":240},\"clouds\":{\"all\":40},\"dt\":1713488113,\"sys\":{\"type\":1,\"id\":7345,\"country\":\"NZ\",\"sunrise\":1713466159,\"sunset\":1713505771},\"timezone\":43200,\"id\":2187404,\"name\":\"Manukau\",\"cod\":200}";

            long unixTimeStamp1 = 1713466159;
            DateTimeOffset dateTimeOffset1 = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp1);
            long unixTimeStamp2 = 1713505771;
            DateTimeOffset dateTimeOffset2 = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp2);
            long unixTimeStamp3 = 1713488113;
            DateTimeOffset dateTimeOffset3 = DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp3);


            WeatherData expectedWeatherData = new WeatherData(
                new CoordInfo(174.8799, -36.9928),
                new List<WeatherInfo> { new WeatherInfo(802, "Clouds", "scattered clouds", "03d") },
                "stations",
                new MainInfo(21.36, 21.64, 20.52, 22.04, 1015, 80),
                10000,
                new WindInfo(3.09, 240),
                new CloudsInfo(40),
                1713488113,
                new SysInfo(1, 7345, "NZ", 1713466159, 1713505771),
                43200,
                802,
                "Manukau",
                200
            );

            WeatherDataDeserializer deserializer = new WeatherDataDeserializer();

            // Act
            WeatherData actualWeatherData = deserializer.DeserializeWeatherData(json);

            // Assert
            Assert.IsNotNull(actualWeatherData); // Check if actualWeatherData is not null
            Assert.IsNotNull(actualWeatherData.Coord); // Check if actualWeatherData.Coord is not null
            Assert.IsNotNull(actualWeatherData.Weather); // Check if actualWeatherData.Weather is not null
            Assert.That(actualWeatherData.Weather.Count, Is.EqualTo(1)); // Check if actualWeatherData.Weather has exactly one element
            Assert.IsNotNull(actualWeatherData.Main); // Check if actualWeatherData.Main is not null
            Assert.IsNotNull(actualWeatherData.Wind); // Check if actualWeatherData.Wind is not null
            Assert.IsNotNull(actualWeatherData.Clouds); // Check if actualWeatherData.Clouds is not null
            Assert.IsNotNull(actualWeatherData.Sys); // Check if actualWeatherData.Sys is not null

            Assert.That(actualWeatherData.Coord.Lon, Is.EqualTo(expectedWeatherData.Coord.Lon));
            Assert.That(actualWeatherData.Coord.Lat, Is.EqualTo(expectedWeatherData.Coord.Lat));
            Assert.That(actualWeatherData.Weather[0].Id, Is.EqualTo(expectedWeatherData.Weather[0].Id));
            Assert.That(actualWeatherData.Weather[0].Main, Is.EqualTo(expectedWeatherData.Weather[0].Main));
            Assert.That(actualWeatherData.Weather[0].Description, Is.EqualTo(expectedWeatherData.Weather[0].Description));
            Assert.That(actualWeatherData.Weather[0].Icon, Is.EqualTo(expectedWeatherData.Weather[0].Icon));
            Assert.That(actualWeatherData.Base, Is.EqualTo(expectedWeatherData.Base));
            Assert.That(actualWeatherData.Main.Temp, Is.EqualTo(expectedWeatherData.Main.Temp));
            Assert.That(actualWeatherData.Main.Pressure, Is.EqualTo(expectedWeatherData.Main.Pressure));
            Assert.That(actualWeatherData.Main.Humidity, Is.EqualTo(expectedWeatherData.Main.Humidity));
            Assert.That(actualWeatherData.Visibility, Is.EqualTo(expectedWeatherData.Visibility));
            Assert.That(actualWeatherData.Wind.Speed, Is.EqualTo(expectedWeatherData.Wind.Speed));
            Assert.That(actualWeatherData.Wind.Deg, Is.EqualTo(expectedWeatherData.Wind.Deg));
            Assert.That(actualWeatherData.Clouds.All, Is.EqualTo(expectedWeatherData.Clouds.All));
            Assert.That(actualWeatherData.Dt, Is.EqualTo(expectedWeatherData.Dt));
            Assert.That(actualWeatherData.Sys.Type, Is.EqualTo(expectedWeatherData.Sys.Type));
            Assert.That(actualWeatherData.Sys.Id, Is.EqualTo(expectedWeatherData.Sys.Id));
            Assert.That(actualWeatherData.Sys.Country, Is.EqualTo(expectedWeatherData.Sys.Country));
            Assert.That(actualWeatherData.Sys.Sunrise, Is.EqualTo(expectedWeatherData.Sys.Sunrise));
            Assert.That(actualWeatherData.Sys.Sunset, Is.EqualTo(expectedWeatherData.Sys.Sunset));
            Assert.That(actualWeatherData.Timezone, Is.EqualTo(expectedWeatherData.Timezone));
            Assert.That(actualWeatherData.Id, Is.EqualTo(expectedWeatherData.Id));
            Assert.That(actualWeatherData.Name, Is.EqualTo(expectedWeatherData.Name));
            Assert.That(actualWeatherData.Cod, Is.EqualTo(expectedWeatherData.Cod));
        }
    }
}

