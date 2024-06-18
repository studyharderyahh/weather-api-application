using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.Models
{
    public interface ITidesDataFactory
    {
        TidesModel.TidesData CreateTidesData(double lat, double lon, DateTime startDate, DateTime endDate);
    }
}
