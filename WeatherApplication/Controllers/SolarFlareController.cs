using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Services;
using WeatherApplication.Views;

namespace WeatherApplication.Controllers
{
    public class SolarFlareController
    {
        private readonly SolarFlareService solarFlaresService;
        private readonly SolarFlareView solarFlaresView;

        public SolarFlareController(SolarFlareService flaresService, SolarFlareView flaresView)
        {
            solarFlaresService = flaresService;
            solarFlaresView = flaresView;
        }

        public async Task GetFlaresAndDisplayAsync(string startDate, string endDate, string apiKey, string solarFlareBaseUrl)
        {
            var flares = await solarFlaresService.GetFlaresAsync(startDate, endDate, apiKey, solarFlareBaseUrl);
            solarFlaresView.DisplayFlares(flares);
        }
    }
}
