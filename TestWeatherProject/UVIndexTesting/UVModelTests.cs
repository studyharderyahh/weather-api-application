using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication;

namespace TestWeatherProject.UVIndexTesting
{
    public class UVModelTests
    {
        [Test]
        public void DeserializeUVModel_ValidJson_DeserializesCorrectly()
        {
            // Arrange
            string json = @"{
                ""products"": [
                    {
                        ""values"": [
                            {
                                ""time"": ""2024-06-22T12:00:00"",
                                ""value"": 7.5
                            },
                            {
                                ""time"": ""2024-06-22T13:00:00"",
                                ""value"": 8.1
                            }
                        ],
                        ""name"": ""cloudy_sky_uv_index""
                    }
                ],
                ""coord"": ""EPSG:4326,-35.0,174.0""
            }";

            // Deserialize JSON response to UVModel object
            UVModel vuModel = JsonConvert.DeserializeObject<UVModel>(json);

            // Assert
            Assert.That(vuModel, Is.Not.Null);
            Assert.That(vuModel.Coord, Is.EqualTo("EPSG:4326,-35.0,174.0"));
            Assert.That(vuModel.Products, Is.Not.Null & Has.Count.EqualTo(1));

            var product = vuModel.Products[0];
            Assert.That(product.Name, Is.EqualTo("cloudy_sky_uv_index"));
            Assert.That(product.Values, Is.Not.Null & Has.Count.EqualTo(2));

            var value1 = product.Values[0];
            Assert.That(value1.Time, Is.EqualTo(new DateTime(2024, 06, 22, 12, 0, 0)));
            Assert.That(value1.UVValue, Is.EqualTo(7.5));

            var value2 = product.Values[1];
            Assert.That(value2.Time, Is.EqualTo(new DateTime(2024, 06, 22, 13, 0, 0)));
            Assert.That(value2.UVValue, Is.EqualTo(8.1));
        }

    }
}
