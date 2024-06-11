using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Models;

namespace WeatherApplication.Views
{
    public class SolarFlareView
    {
        public void DisplayFlares(List<SolarFlareModel> flares)
        {
            if (flares != null && flares.Count > 0)
            {
                Console.WriteLine("Solar Flares Data:");
                foreach (var flare in flares)
                {
                    Console.WriteLine($"Flare ID: {flare.FlareId}");
                    Console.WriteLine($"Begin Time: {flare.BeginTime}");
                    Console.WriteLine($"Peak Time: {flare.PeakTime}");
                    Console.WriteLine($"End Time: {flare.EndTime}");
                    Console.WriteLine($"Class Type: {flare.ClassType}");
                    Console.WriteLine($"Source Location: {flare.SourceLocation}");
                    Console.WriteLine($"Active Region Number: {flare.ActiveRegionNumber}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No solar flares data available.");
            }
        }
    }
}
