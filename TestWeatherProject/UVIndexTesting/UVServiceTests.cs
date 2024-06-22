using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Services;
using WeatherApplication;
using Moq;
using Moq.Protected;
using NUnit.Framework;


namespace TestWeatherProject.UVIndexTesting
{
    public class UVServiceTests
    {
        private UVService CreateServiceWithMockedHttpClient(string apiKey, string baseUrl, HttpResponseMessage responseMessage)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri(baseUrl)
            };

            return new UVService(apiKey, baseUrl, httpClient);
        }

        [Test]
        public void Constructor_NullApiKey_ThrowsArgumentNullException()
        {
            // Arrange
            string nullApiKey = null;
            string baseUrl = "https://test.com";
            HttpClient httpClient = new HttpClient();

            // Act & Assert
            Assert.That(() => new UVService(nullApiKey, baseUrl, httpClient),
                Throws.ArgumentNullException.With.Message.Contains("API key cannot be null"));
        }

        [Test]
        public void Constructor_NullBaseUrl_ThrowsArgumentNullException()
        {
            // Arrange
            string apiKey = "testApiKey";
            string nullBaseUrl = null;
            HttpClient httpClient = new HttpClient();

            // Act & Assert
            Assert.That(() => new UVService(apiKey, nullBaseUrl, httpClient),
                Throws.ArgumentNullException.With.Message.Contains("Base URL cannot be null"));
        }

        [Test]
        public void Constructor_NullHttpClient_ThrowsArgumentNullException()
        {
            // Arrange
            string apiKey = "testApiKey";
            string baseUrl = "https://test.com";
            HttpClient nullHttpClient = null;

            // Act & Assert
            Assert.That(() => new UVService(apiKey, baseUrl, nullHttpClient),
                Throws.ArgumentNullException.With.Message.Contains("Value cannot be null."));
        }

        [Test]
        public async Task GetUVDataAsync_ValidCoordinates_ReturnsUVModel()
        {
            // Arrange
            string apiKey = "testApiKey";
            string baseUrl = "https://test.com";
            double latitude = 37.7749;
            double longitude = -122.4194;

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

            var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };

            var service = CreateServiceWithMockedHttpClient(apiKey, baseUrl, responseMessage);

            // Act
            var result = await service.GetUVDataAsync(latitude, longitude);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Coord, Is.EqualTo("EPSG:4326,-35.0,174.0"));
            Assert.That(result.Products, Is.Not.Null);
            Assert.That(result.Products.Count, Is.EqualTo(1));

            var product = result.Products[0];
            Assert.That(product.Name, Is.EqualTo("cloudy_sky_uv_index"));
            Assert.That(product.Values, Is.Not.Null);
            Assert.That(product.Values.Count, Is.EqualTo(2));

            var value1 = product.Values[0];
            Assert.That(value1.Time, Is.EqualTo(new DateTime(2024, 06, 22, 12, 0, 0)));
            Assert.That(value1.UVValue, Is.EqualTo(7.5));

            var value2 = product.Values[1];
            Assert.That(value2.Time, Is.EqualTo(new DateTime(2024, 06, 22, 13, 0, 0)));
            Assert.That(value2.UVValue, Is.EqualTo(8.1));
        }

    }
}
