using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using WeatherApplication;


namespace TestWeatherProject.HuntingSeasonsTesting
{
    [TestFixture]
    public class HuntingModelTests
    {
        private const string TestDataFilePath = "TestData.txt";

        [OneTimeSetUp]
        public void Setup()
        {
            // Initialize or prepare any setup needed for the tests, such as creating the test data file
            CreateTestDataFile();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            // Clean up any resources after all tests are finished
            if (File.Exists(TestDataFilePath))
            {
                File.Delete(TestDataFilePath);
            }
        }

        [Test]
        public void ParseHuntingSeasonData_ReturnsCorrectSeasonCount()
        {
            // Arrange
            // Ensure test data is available
            CreateTestDataFile();

            // Act
            var seasons = HuntingModel.ParseHuntingSeasonData(TestDataFilePath);

            // Assert
            Assert.That(seasons.Count, Is.EqualTo(12)); // Based on the provided sample data
        }

        [Test]
        public void ParseHuntingSeasonData_ReturnHuntingDatesCorrectly()
        {
            // Arrange
            CreateTestDataFile();

            // Act
            var seasons = HuntingModel.ParseHuntingSeasonData(TestDataFilePath);

            // Assert
            Assert.That(seasons.Count, Is.EqualTo(12));

            // Verify specific dates parsing
            var redStagSeason = seasons.Single(s => s.Species == "Red Stag");
            Assert.That(redStagSeason.HuntingDates,Is.EqualTo("February to August (Rut March to April)"));
        }



        private void CreateTestDataFile()
        {
            // Example sample data as provided
            string[] lines = new[]
            {
                "SPECIES,HUNTING DATES,Notes",
                "Waterfowl,May to June (North Island),Mallard; Canada Geese; Australian Shoveler; Paradise Shelduck; Pacific Black Duck; Black Swan.",
                "Waterfowl,May to July (South Island),Mallard; Canada Geese; Australian Shoveler; Paradise Shelduck; Pacific Black Duck; Black Swan.",
                "Turkey,All Year,August through December spring turkey hunting with no competition; pairs well with trout fishing.  North Island.",
                "Upland Gamebirds,May to July,Pheasant; Quail; Pukeko; Peacock.  North Island.",
                "Red Stag,February to August (Rut March to April),Combines easily with waterfowl; fallow deer and other big game hunts.  North Island & South Island.",
                "Fallow Deer,March to September (Rut in April),Commonly taken during Red Deer hunts.  North Island.",
                "Himalayan Tahr,April to August (Rut in April and May),May be hunted all year but designated dates are when capes are prime.  Commonly paired with Chamois hunting.  North Island & South Island.",
                "European Chamois,April to August (Rut in April and May),Dates indicate prime capes.  Commonly hunted in conjunction with Tahr.  North Island & South Island.",
                "Sika Stag,Late February through May (Rut in April),Winter hunting through September can be rewarding; but snow is likely.  North Island.",
                "Sambar Stag,Mid August through September,For 6 weekends only.  Average 50% on trophy stags. North Island.",
                "Rusa Stag,Rut July and August,Popularly hunted in conjunction with winter hunt for Tahr; Sambar; or Sika.  North Island.",
                "Trout Fishing,All Year,October through April peak trophy fishing."
            };

            File.WriteAllLines(TestDataFilePath, lines);
        }
    }
}
