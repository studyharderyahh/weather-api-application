using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Models;

namespace TestWeatherProject.SolarFlareTesting
{
    [TestFixture]
    public class SolarFlareModelTests
    {
        [Test]
        public void SolarFlareModel_Serialization()
        {
            // Arrange
            var flare = new SolarFlareModel
            {
                FlareId = "AR1234",
                BeginTime = new DateTime(2024, 6, 20, 10, 30, 0),
                PeakTime = new DateTime(2024, 6, 20, 11, 0, 0),
                EndTime = new DateTime(2024, 6, 20, 11, 30, 0),
                ClassType = "X1.5",
                SourceLocation = "S24W36",
                ActiveRegionNumber = 12345
            };

            // Act
            string json = JsonConvert.SerializeObject(flare);

            // Assert
            Assert.That(json, Does.Contain("\"flrID\":\"AR1234\""));
            Assert.That(json, Does.Contain("\"beginTime\":\"2024-06-20T10:30:00\""));
            Assert.That(json, Does.Contain("\"peakTime\":\"2024-06-20T11:00:00\""));
            Assert.That(json, Does.Contain("\"endTime\":\"2024-06-20T11:30:00\""));
            Assert.That(json, Does.Contain("\"classType\":\"X1.5\""));
            Assert.That(json, Does.Contain("\"sourceLocation\":\"S24W36\""));
            Assert.That(json, Does.Contain("\"activeRegionNum\":12345"));
        }

        [Test]
        public void SolarFlareModel_Deserialization()
        {
            // Arrange
            string json = @"{
                ""flrID"": ""AR5678"",
                ""beginTime"": ""2023-05-15T08:00:00"",
                ""peakTime"": ""2023-05-15T08:30:00"",
                ""endTime"": ""2023-05-15T09:00:00"",
                ""classType"": ""M3.2"",
                ""sourceLocation"": ""N08E15"",
                ""activeRegionNum"": 54321
            }";

            // Act
            var flare = JsonConvert.DeserializeObject<SolarFlareModel>(json);

            // Assert
            Assert.That(flare, Is.Not.Null);
            Assert.That(flare.FlareId, Is.EqualTo("AR5678"));
            Assert.That(flare.BeginTime, Is.EqualTo(new DateTime(2023, 5, 15, 8, 0, 0)));
            Assert.That(flare.PeakTime, Is.EqualTo(new DateTime(2023, 5, 15, 8, 30, 0)));
            Assert.That(flare.EndTime, Is.EqualTo(new DateTime(2023, 5, 15, 9, 0, 0)));
            Assert.That(flare.ClassType, Is.EqualTo("M3.2"));
            Assert.That(flare.SourceLocation, Is.EqualTo("N08E15"));
            Assert.That(flare.ActiveRegionNumber, Is.EqualTo(54321));
        }
    }
}
