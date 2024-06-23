using System;
using System.Threading.Tasks;
using WeatherApplication.FileHandlers;
using WeatherApplication.Views;

namespace WeatherApplication.Controllers
{
    /// <summary>
    /// Controller class for managing weather data.
    /// </summary>
    public class WeatherAPIController
    {
        private readonly WeatherApplicationView m_weatherView; // Reference to the view.
        private readonly WeatherModel m_weatherModel; // Reference to the model.
        private WeatherData? m_weatherData; // Holds the latest weather data.

        private readonly Logger logger = Logger.Instance();

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherAPIController"/> class.
        /// </summary>
        /// <param name="model">The weather model for retrieving weather data.</param>
        /// <param name="view">The weather view for rendering weather data.</param>
        /// <exception cref="ArgumentNullException">Thrown when model or view is null.</exception>
        public WeatherAPIController(WeatherModel model, WeatherApplicationView view)
        {
            m_weatherModel = model ?? throw new ArgumentNullException(nameof(model));
            m_weatherView = view ?? throw new ArgumentNullException(nameof(view));
        }

        /// <summary>
        /// Retrieves weather data asynchronously and updates the model.
        /// </summary>
        /// <param name="apiKey">The API key for accessing the weather service.</param>
        /// <param name="city">The city for which weather data is requested.</param>
        /// <param name="weatherUrl">The base URL for the weather API.</param>
        /// <exception cref="ArgumentException">Thrown when the city name is null or empty.</exception>
        public async Task RefreshWeatherData(string apiKey, string city, string weatherUrl)
        {
            // Ensure the city name is not null or empty.
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City name cannot be null or empty.");
            }

            try
            {
                // Call GetWeatherAsync method to retrieve weather data asynchronously.
                m_weatherData = await m_weatherModel.GetWeatherAsync(apiKey, city, weatherUrl);
            }
            catch (WeatherServiceException ex)
            {
                // Log and handle specific exceptions related to the weather service.
                logger.LogError($"An error occurred while retrieving weather data: {ex.Message}");
                // Optionally re-throw the exception if additional handling is needed.
                throw;
            }
            catch (Exception ex)
            {
                // Log and handle unexpected exceptions.
                logger.LogError($"An unexpected error occurred while retrieving weather data: {ex.Message}");
                // Optionally re-throw the exception if additional handling is needed.
                throw;
            }
        }

        /// <summary>
        /// Refreshes the weather view panel with the latest weather data.
        /// </summary>
        public void RefreshPanelView()
        {
            // Render the weather data only if it is available.
            if (m_weatherData != null)
            {
                m_weatherView.Render(m_weatherData);
            }
        }
    }
}
