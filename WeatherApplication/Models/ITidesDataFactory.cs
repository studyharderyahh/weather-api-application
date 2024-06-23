using System;

namespace WeatherApplication.Models
{
    /// <summary>
    /// Represents a factory interface for creating TidesData objects.
    /// </summary>
    public interface ITidesDataFactory
    {
        /// <summary>
        /// Creates a new instance of TidesData based on the provided parameters.
        /// </summary>
        /// <param name="lat">Latitude coordinate for the location.</param>
        /// <param name="lon">Longitude coordinate for the location.</param>
        /// <param name="startDate">Start date for retrieving tide data.</param>
        /// <param name="endDate">End date for retrieving tide data.</param>
        /// <returns>A TidesData object populated with data based on the input parameters.</returns>
        TidesModel.TidesData CreateTidesData(double lat, double lon, DateTime startDate, DateTime endDate);
    }
}
