using System;
using System.Threading.Tasks;
using WeatherApplication.FileHandlers;
using WeatherApplication.Services;
using WeatherApplication.Views;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherApplication.Controllers
{
    public class UVController
    {
        private readonly UVService _uvService;
        private readonly Logger logger = Logger.Instance();

        public UVController(UVService uvService)
        {
            _uvService = uvService ?? throw new ArgumentNullException(nameof(uvService));
        }

        public async Task<UVModel> GetUVDataAsync(double latitude, double longitude)
        {
            try
            {
                return await _uvService.GetUVDataAsync(latitude, longitude);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception
                Console.WriteLine($"Failed to retrieve UV data. Please check your network connection and try again. {ex.Message}");
                logger.LogError($"Failed to retrieve UV data: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An unexpected error occurred while retrieving UV data: {ex.Message}");
                logger.LogError($"An unexpected error occurred while retrieving UV data: {ex.Message}");
                return null;
            }
        }
    }
}
