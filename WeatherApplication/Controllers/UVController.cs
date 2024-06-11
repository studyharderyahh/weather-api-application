using System;
using System.Threading.Tasks;
using WeatherApplication.FileHandlers;
using WeatherApplication.Services;
using WeatherApplication.Views;

namespace WeatherApplication.Controllers
{
    public class UVController
    {
        private readonly UVService _uvService;

        public UVController(UVService uvService)
        {
            _uvService = uvService;
        }

        public async Task<UVModel> GetUVDataAsync(double latitude, double longitude)
        {
            return await _uvService.GetUVDataAsync(latitude, longitude);
        }
    }
}
