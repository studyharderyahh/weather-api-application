using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApplication.FileHandlers;
using WeatherApplication.Views;

namespace WeatherApplication.Controllers
{
    public class TidesController
    {
        private readonly TidesModel m_tidesModel;
        private readonly TidesView m_tidesView;
        private TidesModel.TidesData? m_tidesData;

        Logger logger = Logger.Instance();

        public TidesController(TidesModel model, TidesView view)
        {
            m_tidesModel = model ?? throw new ArgumentNullException(nameof(model));
            m_tidesView = view ?? throw new ArgumentNullException(nameof(view));
        }

        public async Task RefreshTidesData(double lat, double lon, string apiKey, DateTime startDate, DateTime endDate)
        {
            try
            {
                m_tidesData = await m_tidesModel.GetTidesData(lat, lon, apiKey, startDate, endDate);
                m_tidesView.Render(m_tidesData);
            }
            catch (ArgumentException ex)
            {
                //ErrorLogger.Instance.LogError($"An error occurred in RefreshTidesData: {ex.Message}");
                logger.LogError($"An error occurred in RefreshTidesData: {ex.Message}");
            }
        }
    }
}
