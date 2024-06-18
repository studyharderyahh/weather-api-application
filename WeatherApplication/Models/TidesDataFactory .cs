using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.Models
{
    public class TidesDataFactory : ITidesDataFactory
    {
        public TidesModel.TidesData CreateTidesData(double lat, double lon, DateTime startDate, DateTime endDate)
        {
            return new TidesModel.TidesData
            {
                Metadata = new TidesModel.Metadata
                {
                    Latitude = lat,
                    Longitude = lon,
                    Datum = "MSL",
                    Start = startDate,
                    Days = (int)(endDate - startDate).TotalDays + 1,
                    Interval = 0,
                    Height = "MSL = 0m"
                }
            };
        }
    }
}
