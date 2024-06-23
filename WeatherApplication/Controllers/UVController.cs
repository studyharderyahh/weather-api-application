using System;
using System.Threading.Tasks;
using WeatherApplication.Services;
using WeatherApplication.Views;
using WeatherApplication.FileHandlers;

namespace WeatherApplication.Controllers
{
    /// <summary>
    /// Controller class for handling UV data.
    /// </summary>
    public class UVController
    {
        private readonly UVService _uvService;
        private readonly Logger logger = Logger.Instance();

        /// <summary>
        /// Initializes a new instance of the <see cref="UVController"/> class.
        /// </summary>
        /// <param name="uvService">The UV service for retrieving UV data.</param>
        /// <exception cref="ArgumentNullException">Thrown when uvService is null.</exception>
        public UVController(UVService uvService)
        {
            _uvService = uvService ?? throw new ArgumentNullException(nameof(uvService));
        }

        /// <summary>
        /// Retrieves UV data asynchronously.
        /// </summary>
        /// <param name="latitude">The latitude of the location for UV data.</param>
        /// <param name="longitude">The longitude of the location for UV data.</param>
        /// <returns>An instance of <see cref="UVModel"/> containing UV data.</returns>
        public async Task<UVModel> GetUVDataAsync(double latitude, double longitude)
        {
            try
            {
                // Call the UV service to retrieve UV data
                return await _uvService.GetUVDataAsync(latitude, longitude);
            }
            catch (HttpRequestException ex)
            {
                // Log and handle HTTP request exceptions
                Console.WriteLine($"Failed to retrieve UV data. Please check your network connection and try again. {ex.Message}");
                logger.LogError($"Failed to retrieve UV data: {ex.Message}");
                return null; // Return null indicating failure
            }
            catch (Exception ex)
            {
                // Log and handle unexpected exceptions
                Console.WriteLine($"An unexpected error occurred while retrieving UV data: {ex.Message}");
                logger.LogError($"An unexpected error occurred while retrieving UV data: {ex.Message}");
                return null; // Return null indicating failure
            }
        }
    }
}
