using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class TidesView
    {
        public void Render(TidesModel.TidesData tideData)
        {
            Console.WriteLine("Tides Data:");

            // Render metadata
            Console.WriteLine($"Latitude: {tideData.Metadata.Latitude}");
            Console.WriteLine($"Longitude: {tideData.Metadata.Longitude}");
            Console.WriteLine($"Datum: {tideData.Metadata.Datum}");
            Console.WriteLine($"Start Date: {tideData.Metadata.Start}");
            Console.WriteLine($"Number of Days: {tideData.Metadata.Days}");
            Console.WriteLine($"Interval: {tideData.Metadata.Interval}");
            Console.WriteLine($"Height: {tideData.Metadata.Height}");

            // Render tide values
            Console.WriteLine("\nTide Values:");
            foreach (var value in tideData.Values)
            {
                Console.WriteLine($"Time: {value.Time}, Value: {value.Value}");
            }
        }
    }
}
