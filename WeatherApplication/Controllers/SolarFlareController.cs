using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.Services;
using WeatherApplication.Views;
using WeatherApplication.FileHandlers;

namespace WeatherApplication.Controllers
{
    /// <summary>
    /// Controller class for handling solar flare data.
    /// </summary>
    public class SolarFlareController
    {
        private readonly SolarFlareService solarFlaresService;
        private readonly SolarFlareView solarFlaresView;
        // Singleton logger instance to log application activities
        private readonly Logger logger = Logger.Instance();

        /// <summary>
        /// Initializes a new instance of the <see cref="SolarFlareController"/> class.
        /// </summary>
        /// <param name="flaresService">The service to retrieve solar flare data.</param>
        /// <param name="flaresView">The view to display solar flare data.</param>
        public SolarFlareController(SolarFlareService flaresService, SolarFlareView flaresView)
        {
            solarFlaresService = flaresService ?? throw new ArgumentNullException(nameof(flaresService));
            solarFlaresView = flaresView ?? throw new ArgumentNullException(nameof(flaresView));
        }

        /// <summary>
        /// Retrieves solar flare data and displays it using the view.
        /// </summary>
        /// <param name="startDate">The start date for the data retrieval.</param>
        /// <param name="endDate">The end date for the data retrieval.</param>
        /// <param name="apiKey">The API key for accessing the solar flare data.</param>
        /// <param name="solarFlareBaseUrl">The base URL for the solar flare API.</param>
        public async Task GetFlaresAndDisplayAsync(string startDate, string endDate, string apiKey, string solarFlareBaseUrl)
        {
            try
            {
                // Retrieve solar flare data using the service
                var flares = await solarFlaresService.GetFlaresAsync(startDate, endDate, apiKey, solarFlareBaseUrl);
                // Display the retrieved data using the view
                solarFlaresView.DisplayFlares(flares);
            }
            catch (HttpRequestException ex)
            {
                // Log and display HTTP request errors
                Console.WriteLine($"An error occurred while retrieving solar flare data: {ex.Message}");
                Logger.Instance().LogError($"An error occurred while retrieving solar flare data: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log and display general errors
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Logger.Instance().LogError($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
