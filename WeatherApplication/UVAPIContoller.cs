using System;
using System.Threading.Tasks;
using WeatherApplication;

namespace WeatherApplication
{
    public class UVController
    {
        private readonly UVModel                    m_UVModel;
        private readonly WeatherApplicationView     m_UVView;
        private UVModel.UVData?                     m_UVData;

        public UVController(UVModel model, WeatherApplicationView view)
        {
            m_UVModel = model ?? throw new ArgumentNullException(nameof(model));
            m_UVView = view ?? throw new ArgumentNullException(nameof(view));
        }

        public async Task RefreshUVData(string apiKey, CoordInfo coordInfo)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty.");
            }
            try
            {
                // Call GetUVDataAsync method to retrieve UV data
                m_UVData = await m_UVModel.GetUVDataAsync(apiKey, coordInfo);
                // Render the UV data
                m_UVView.Render(m_UVData);
            }
            catch (UVServiceException ex)
            {
                ErrorLogger.Instance.LogError($"An error occurred while retrieving UV data: {ex.Message}");
            }
        }

        public class UVServiceException : Exception
        {
            public UVServiceException() { }
            public UVServiceException(string message) : base(message) { }
            public UVServiceException(string message, Exception innerException) : base(message, innerException) { }
        }
    }
}
