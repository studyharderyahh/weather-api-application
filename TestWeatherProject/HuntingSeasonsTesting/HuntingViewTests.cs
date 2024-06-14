using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using WeatherApplication.Views;

namespace WeatherApplication.Tests.Views
{
    [TestFixture]
    public class HuntingViewTests
    {
        private string _testFilePath; // File path for test data

        [SetUp]
        public void Setup()
        {
            // Setup test file path
            _testFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "HuntingSeasons.txt");
        }

        [Test]
        public void Render_NoSeasonsProvided_PrintsNoDataMessage()
        {
            // Arrange
            var view = new HuntingView();
            List<HuntingModel.HuntingSeason> seasons = null;
            var expectedOutput = "No hunting season data available.";

            // Act
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                view.Render(seasons);
                var output = sw.ToString().Trim();

                // Assert
                Assert.AreEqual(expectedOutput, output);
            }
        }

        [Test]
        public void Render_EmptySeasonsList_PrintsNoDataMessage()
        {
            // Arrange
            var view = new HuntingView();
            var seasons = new List<HuntingModel.HuntingSeason>();
            var expectedOutput = "No hunting season data available.";

            // Act
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                view.Render(seasons);
                var output = sw.ToString().Trim();

                // Assert
                Assert.AreEqual(expectedOutput, output);
            }
        }

        [Test]
        public void Render_SingleSeason_PrintsSeasonDetails()
        {
            // Arrange
            var view = new HuntingView();
            var seasons = new List<HuntingModel.HuntingSeason>
            {
                new HuntingModel.HuntingSeason("Deer", "October - November", "Bucks only", new DateTime(2024, 10, 1), new DateTime(2024, 11, 30))
            };

            // Expected output lines
            var expectedLines = new List<string>
            {
                "Hunting Season Data:",
                "--------------------",
                "Species: Deer",
                "Hunting Dates: October - November",
                "Notes: Bucks only"
            };

            // Act
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                view.Render(seasons);
                var output = sw.ToString().Trim().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                // Assert
                Assert.AreEqual(expectedLines.Count, output.Length);
                for (int i = 0; i < expectedLines.Count; i++)
                {
                    Assert.IsTrue(output[i].Contains(expectedLines[i]));
                }
            }
        }

        [Test]
        public void Render_MultipleSeasons_PrintsAllSeasonsDetails()
        {
            // Arrange
            var view = new HuntingView();
            var seasons = new List<HuntingModel.HuntingSeason>
            {
                new HuntingModel.HuntingSeason("Deer", "October - November", "Bucks only", new DateTime(2024, 10, 1), new DateTime(2024, 11, 30)),
                new HuntingModel.HuntingSeason("Duck", "September - December", "No limit", new DateTime(2024, 9, 1), new DateTime(2024, 12, 31))
            };

            // Expected output lines
            var expectedOutput = string.Join(Environment.NewLine, new List<string>
            {
                "Hunting Season Data:",
                "--------------------",
                "Species: Deer",
                "Hunting Dates: October - November",
                "Notes: Bucks only",
                "",
                "Species: Duck",
                "Hunting Dates: September - December",
                "Notes: No limit"
            });

            // Act
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                view.Render(seasons);
                var output = sw.ToString().Trim();

                // Assert
                Assert.AreEqual(expectedOutput, output);
            }
        }

    }
}
