using System;
using System.Threading.Tasks; // For asynchronous programming.
using WeatherApplication.FileHandlers; // To use components of the WeatherApplication namespace.

namespace WeatherApplication
{
    public class WeatherAPIController
    {
        private readonly WeatherApplicationView m_weatherView; // Reference to the view.
        private readonly WeatherModel           m_weatherModel; // Reference to the model.
        private WeatherData?                    m_weatherData; // Holds the latest weather data.
        public WeatherAPIController(WeatherModel model, WeatherApplicationView view)
        {
            // Ensure the model and view are not null.
            m_weatherModel = model ?? throw new ArgumentNullException(nameof(model));
            m_weatherView = view ?? throw new ArgumentNullException(nameof(view));
        }
        public async Task RefreshWeatherData(string apiKey, string city)
        {
            // Ensure the city name is not null or empty.
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City name cannot be null or empty.");
            }
            try
            {
                // Call GetWeatherAsync method to retrieve weather data
                // Retrieve weather data asynchronously.
                m_weatherData = await m_weatherModel.GetWeatherAsync(apiKey, city);
            }
            catch (WeatherServiceException ex)
            {
                // Log any errors that occur during the data retrieval.
                ErrorLogger.Instance.LogError($"An error occurred while retrieving weather data: {ex.Message}");
            }
        }
        public void RefreshPanelView()
        {
            // Render the weather data only if it is available.
            if (null != m_weatherData)
            {
                m_weatherView.Render(m_weatherData);
            }
        }
    }
}
