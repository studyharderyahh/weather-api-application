using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using WeatherApplication;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using static WeatherApplication.HuntingModel;
using static WeatherApplication.UVModel;
using Newtonsoft.Json;

namespace WeatherApplication.Tests
{
    [TestFixture]
    public class WeatherDataDeserializerTests
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

    [TestFixture]
    public class UVDataDeserializerTests
    {
        [Test]
        public void DeserializeUVData_ReturnsUVDataObject()
        {
            // Arrange
            string json = @"
        {
          ""products"": [
            {
              ""values"": [
                {
                  ""time"": ""2024-04-22T18:00:00.000Z"",
                  ""value"": 0
                },
                {
                  ""time"": ""2024-04-22T19:00:00.000Z"",
                  ""value"": 0
                },
                {
                  ""time"": ""2024-04-22T20:00:00.000Z"",
                  ""value"": 0.261
                }
              ],
              ""name"": ""cloudy_sky_uv_index""
            },
            {
              ""values"": [
                {
                  ""time"": ""2024-04-22T18:00:00.000Z"",
                  ""value"": 0
                },
                {
                  ""time"": ""2024-04-22T19:00:00.000Z"",
                  ""value"": 0
                },
                {
                  ""time"": ""2024-04-22T20:00:00.000Z"",
                  ""value"": 0.382
                }
              ],
              ""name"": ""clear_sky_uv_index""
            }
          ],
          ""coord"": ""EPSG:4326,-36.993,174.88""
        }";

            // Act
            var uvDataDeserializer = new UVModel.UVDataDeserializer();
            UVModel.UVData uvData = uvDataDeserializer.Deserialize(json);

            // Assert
            Assert.That(uvData, Is.Not.Null);
            Assert.That(uvData.Products, Has.Count.EqualTo(2));

            // Assert product 1
            Assert.That(uvData.Products[0].Name, Is.EqualTo("cloudy_sky_uv_index"));
            Assert.That(uvData.Products[0].Values, Has.Count.EqualTo(3));
            AssertValuesInRange(uvData.Products[0].Values);

            // Assert product 2
            Assert.That(uvData.Products[1].Name, Is.EqualTo("clear_sky_uv_index"));
            Assert.That(uvData.Products[1].Values, Has.Count.EqualTo(3));
            AssertValuesInRange(uvData.Products[1].Values);
        }

        private void AssertValuesInRange(List<UVModel.UVDataEntry> values)
        {
            AssertValuesInRange(values, v => v.Value, v => v.Time);
        }

        private void AssertValuesInRange(List<UVModel.UVDataEntry> values, Func<UVModel.UVDataEntry, double> valueAccessor, Func<UVModel.UVDataEntry, DateTime> timeAccessor)
        {
            foreach (var value in values)
            {
                Assert.That(valueAccessor(value), Is.GreaterThanOrEqualTo(0));
                Assert.That(valueAccessor(value), Is.LessThanOrEqualTo(double.MaxValue));
                //              Assert.That(timeAccessor(value), Is.Not.Null);
            }
        }
    }
    [TestFixture]
    public class TideDataDeserializerTests
    {
        public class TideModelTests
        {
            [Test]
            public void DeserializeTideData_ReturnsTideDataObject()
            {
                // Arrange
                string json = "{\"metadata\":{\"latitude\":-37.045,\"longitude\":174.846,\"datum\":\"LAT\",\"start\":\"2024-04-28T12:00:00.000Z\",\"days\":7,\"interval\":0,\"height\":\"LAT = 2.241m below MSL\"},\"values\":[{\"time\":\"2024-04-28T13:30:00Z\",\"value\":3.6},{\"time\":\"2024-04-28T19:38:00Z\",\"value\":1},{\"time\":\"2024-04-29T01:46:00Z\",\"value\":3.54},{\"time\":\"2024-04-29T08:03:00Z\",\"value\":0.87},{\"time\":\"2024-04-29T14:21:00Z\",\"value\":3.51},{\"time\":\"2024-04-29T20:32:00Z\",\"value\":1.1},{\"time\":\"2024-04-30T02:41:00Z\",\"value\":3.42},{\"time\":\"2024-04-30T09:01:00Z\",\"value\":0.97},{\"time\":\"2024-04-30T15:23:00Z\",\"value\":3.43},{\"time\":\"2024-04-30T21:38:00Z\",\"value\":1.18},{\"time\":\"2024-05-01T03:49:00Z\",\"value\":3.34},{\"time\":\"2024-05-01T10:10:00Z\",\"value\":1.03},{\"time\":\"2024-05-01T16:35:00Z\",\"value\":3.42},{\"time\":\"2024-05-01T22:52:00Z\",\"value\":1.17},{\"time\":\"2024-05-02T05:05:00Z\",\"value\":3.35},{\"time\":\"2024-05-02T11:25:00Z\",\"value\":1},{\"time\":\"2024-05-02T17:49:00Z\",\"value\":3.51},{\"time\":\"2024-05-03T00:07:00Z\",\"value\":1.04},{\"time\":\"2024-05-03T06:20:00Z\",\"value\":3.47},{\"time\":\"2024-05-03T12:36:00Z\",\"value\":0.86},{\"time\":\"2024-05-03T18:57:00Z\",\"value\":3.69},{\"time\":\"2024-05-04T01:14:00Z\",\"value\":0.83},{\"time\":\"2024-05-04T07:26:00Z\",\"value\":3.67},{\"time\":\"2024-05-04T13:40:00Z\",\"value\":0.68},{\"time\":\"2024-05-04T19:57:00Z\",\"value\":3.89},{\"time\":\"2024-05-05T02:12:00Z\",\"value\":0.6},{\"time\":\"2024-05-05T08:24:00Z\",\"value\":3.89}]}";

                // Act
                var tideData = JsonConvert.DeserializeObject<TidesModel.TidesData>(json);

                // Assert
                Assert.IsNotNull(tideData);
                Assert.IsNotNull(tideData.Metadata);
                Assert.IsNotNull(tideData.Values);
                Assert.That(tideData.Metadata.Latitude, Is.EqualTo(-37.045));
                Assert.That(tideData.Metadata.Longitude, Is.EqualTo(174.846));
                // Add more assertions as needed
            }
        }

    }

    [TestFixture]
    public class HuntingDataLoadingTests
    {
        [Test]
        public void DeserializeHuntingData_ReturnsHuntingDataObject()
        {
            // Arrange
            List<HuntingSeason> expectedHuntingSeasons = new List<HuntingSeason>
            {
                new HuntingSeason("Waterfowl", "May to June (North Island)", "Mallard; Canada Geese; Australian Shoveler; Paradise Shelduck; Pacific Black Duck; Black Swan."),
                new HuntingSeason("Waterfowl", "May to July (South Island)", "Mallard; Canada Geese; Australian Shoveler; Paradise Shelduck; Pacific Black Duck; Black Swan."),
                new HuntingSeason("Turkey", "All Year", "August through December spring turkey hunting with no competition; pairs well with trout fishing.  North Island."),
                new HuntingSeason("Upland Gamebirds", "May to July", "Pheasant; Quail; Pukeko; Peacock.  North Island."),
                new HuntingSeason("Red Stag", "February to August (Rut March to April)", "Combines easily with waterfowl; fallow deer and other big game hunts.  North Island & South Island."),
                new HuntingSeason("Fallow Deer", "March to September (Rut in April)", "Commonly taken during Red Deer hunts.  North Island."),
                new HuntingSeason("Himalayan Tahr", "April to August (Rut in April and May)", "May be hunted all year but designated dates are when capes are prime.  Commonly paired with Chamois hunting.  North Island & South Island."),
                new HuntingSeason("European Chamois", "April to August (Rut in April and May)", "Dates indicate prime capes.  Commonly hunted in conjunction with Tahr.  North Island & South Island."),
                new HuntingSeason("Sika Stag", "Late February through May (Rut in April)", "Winter hunting through September can be rewarding; but snow is likely.  North Island."),
                new HuntingSeason("Sambar Stag", "Mid August through September", "For 6 weekends only.  Average 50% on trophy stags. North Island."),
                new HuntingSeason("Rusa Stag", "Rut July and August", "Popularly hunted in conjunction with winter hunt for Tahr; Sambar; or Sika.  North Island."),
                new HuntingSeason("Trout Fishing", "All Year", "October through April peak trophy fishing.")
            };


            // Act
            List<HuntingSeason> actualHuntingSeasons = HuntingModel.ParseHuntingSeasonData("hunting_season_data.txt");

            // Assert
            Assert.That(actualHuntingSeasons, Is.Not.Null);
            Assert.That(actualHuntingSeasons.Count, Is.EqualTo(expectedHuntingSeasons.Count));

            for (int i = 0; i < expectedHuntingSeasons.Count; i++)
            {
                Assert.That(actualHuntingSeasons[i].Species, Is.EqualTo(expectedHuntingSeasons[i].Species));
                Assert.That(actualHuntingSeasons[i].HuntingDates, Is.EqualTo(expectedHuntingSeasons[i].HuntingDates));
                Assert.That(actualHuntingSeasons[i].Notes, Is.EqualTo(expectedHuntingSeasons[i].Notes));
            }
        }
    }
   
    [TestFixture]
    public class FileEncoderTests
    {
        private string testFilePath = "application.sys";

        [SetUp]
        public void Setup()
        {
            // Clear the contents of the test file before each test
            File.WriteAllText(testFilePath, string.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            // Remove the test file after each test
            File.Delete(testFilePath);
        }

        [Test]
        public void FileEncoder_AddNameValuePair()
        {
            // Get an instance of FileEncoder for testing
            FileEncoder encoder = FileEncoder.GetInstance(testFilePath);
            // Write key-value pairs for testing
            encoder.Write("ApiKey", "a173994356f879bb3e422754bfdde559");
            // Read values by key for testing
            string actualAPIKey = encoder.Read("ApiKey");
            Assert.That(actualAPIKey, Is.EqualTo("a173994356f879bb3e422754bfdde559"));
        }
    }
}

