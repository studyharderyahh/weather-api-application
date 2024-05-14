using System;
using System.Threading.Tasks;
using WeatherApplication;

namespace WeatherApplication
{
    public class WeatherAPIController
    {
        private readonly WeatherApplicationView m_weatherView;
        private readonly WeatherModel           m_weatherModel;
        private WeatherData?                    m_weatherData;
        public WeatherAPIController(WeatherModel model, WeatherApplicationView view)
        {
            m_weatherModel = model ?? throw new ArgumentNullException(nameof(model));
            m_weatherView = view ?? throw new ArgumentNullException(nameof(view));
        }
        public async Task RefreshWeatherData(string apiKey, string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City name cannot be null or empty.");
            }
            try
            {
                // Call GetWeatherAsync method to retrieve weather data
                m_weatherData = await m_weatherModel.GetWeatherAsync(apiKey, city);
            }
            catch (WeatherServiceException ex)
            {
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
