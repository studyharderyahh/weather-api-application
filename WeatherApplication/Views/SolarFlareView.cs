using System;
using System.Collections.Generic;
using WeatherApplication.Models;

namespace WeatherApplication.Views
{
    /// <summary>
    /// View class responsible for displaying solar flare data.
    /// </summary>
    public class SolarFlareView
    {
        /// <summary>
        /// Displays information about solar flares.
        /// </summary>
        /// <param name="flares">List of SolarFlareModel objects representing solar flares.</param>
        public void DisplayFlares(List<SolarFlareModel> flares)
        {
            if (flares == null || flares.Count == 0)
            {
                PrintNoDataMessage();
                return;
            }

            PrintFlaresHeader();
            foreach (var flare in flares)
            {
                DisplayFlareDetails(flare);
            }
        }

        /// <summary>
        /// Prints a message when no solar flares data is available.
        /// </summary>
        private void PrintNoDataMessage()
        {
            Console.WriteLine("No solar flares data available.");
        }

        /// <summary>
        /// Prints the header for the solar flares data section.
        /// </summary>
        private void PrintFlaresHeader()
        {
            Console.WriteLine("Solar Flares Data:");
        }

        /// <summary>
        /// Displays details of a single solar flare.
        /// </summary>
        /// <param name="flare">SolarFlareModel object representing a single solar flare.</param>
        private void DisplayFlareDetails(SolarFlareModel flare)
        {
            Console.WriteLine($"Flare ID: {flare.FlareId}");
            Console.WriteLine($"Begin Time: {flare.BeginTime}");
            Console.WriteLine($"Peak Time: {flare.PeakTime}");
            Console.WriteLine($"End Time: {flare.EndTime}");
            Console.WriteLine($"Class Type: {flare.ClassType}");
            Console.WriteLine($"Source Location: {flare.SourceLocation}");
            Console.WriteLine($"Active Region Number: {flare.ActiveRegionNumber}");
            Console.WriteLine(); // Add a blank line for readability
        }
    }
}
