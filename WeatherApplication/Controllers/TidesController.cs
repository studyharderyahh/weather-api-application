using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.FileHandlers;
using WeatherApplication.Models;
using WeatherApplication.Views;

namespace WeatherApplication.Controllers
{
    /// <summary>
    /// Controller class for handling tide data.
    /// </summary>
    public class TidesController
    {
        private readonly TidesModel m_tidesModel;
        private readonly TidesView m_tidesView;
        private TidesModel.TidesData? m_tidesData;

        // Singleton logger instance to log application activities
        private readonly Logger logger = Logger.Instance();

        /// <summary>
        /// Initializes a new instance of the <see cref="TidesController"/> class.
        /// </summary>
        /// <param name="model">The tides model.</param>
        /// <param name="view">The tides view.</param>
        /// <exception cref="ArgumentNullException">Thrown when model or view is null.</exception>
        public TidesController(TidesModel model, TidesView view)
        {
            m_tidesModel = model ?? throw new ArgumentNullException(nameof(model));
            m_tidesView = view ?? throw new ArgumentNullException(nameof(view));
        }

        /// <summary>
        /// Refreshes the tide data and updates the view.
        /// </summary>
        /// <param name="lat">The latitude for the tide data.</param>
        /// <param name="lon">The longitude for the tide data.</param>
        /// <param name="apiKey">The API key for accessing the tide data.</param>
        /// <param name="startDate">The start date for the tide data.</param>
        /// <param name="endDate">The end date for the tide data.</param>
        /// <param name="tidesBaseUrl">The base URL for the tide API.</param>
        public async Task RefreshTidesData(double lat, double lon, string apiKey, DateTime startDate, DateTime endDate, string tidesBaseUrl)
        {
            try
            {
                // Fetch the tide data using the model
                m_tidesData = await m_tidesModel.GetTidesDataAsync(lat, lon, apiKey, startDate, endDate, tidesBaseUrl);
                // Render the fetched data using the view
                m_tidesView.Render(m_tidesData);
            }
            catch (ArgumentException ex)
            {
                // Log and handle argument exceptions
                logger.LogError($"An error occurred in RefreshTidesData: {ex.Message}");
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            catch (HttpRequestException ex)
            {
                // Log and handle HTTP request exceptions
                logger.LogError($"An error occurred while retrieving tide data: {ex.Message}");
                Console.WriteLine($"An error occurred while retrieving tide data: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log and handle general exceptions
                logger.LogError($"An unexpected error occurred: {ex.Message}");
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
