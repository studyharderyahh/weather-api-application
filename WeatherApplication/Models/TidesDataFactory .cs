using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.Models
{
    /// <summary>
    /// Factory class for creating TidesData objects.
    /// </summary>
    public class TidesDataFactory : ITidesDataFactory
    {
        /// <summary>
        /// Creates a new instance of TidesData based on the provided parameters.
        /// </summary>
        /// <param name="lat">Latitude coordinate.</param>
        /// <param name="lon">Longitude coordinate.</param>
        /// <param name="startDate">Start date of the tides data.</param>
        /// <param name="endDate">End date of the tides data.</param>
        /// <returns>A new instance of TidesData with metadata initialized.</returns>
        public TidesModel.TidesData CreateTidesData(double lat, double lon, DateTime startDate, DateTime endDate)
        {
            try
            {
                // Calculate the number of days between startDate and endDate
                int days = (int)(endDate - startDate).TotalDays + 1;

                // Create a new TidesData object with initialized metadata
                return new TidesModel.TidesData
                {
                    Metadata = new TidesModel.Metadata
                    {
                        Latitude = lat,
                        Longitude = lon,
                        Datum = "MSL",        // Mean Sea Level
                        Start = startDate,
                        Days = days,
                        Interval = 0,
                        Height = "MSL = 0m"   // Mean Sea Level height
                    }
                };
            }
            catch (Exception ex)
            {
                // Log and handle any exceptions that might occur during data creation
                Console.WriteLine($"Error creating TidesData: {ex.Message}");
                throw; // Rethrow the exception for higher-level handling
            }
        }
    }
}
