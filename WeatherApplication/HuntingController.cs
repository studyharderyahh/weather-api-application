using System;
using System.Collections.Generic;
using System.IO;

namespace WeatherApplication
{
    public class HuntingController
    {
        private readonly HuntingModel m_huntingModel;

        public HuntingController(HuntingModel model)
        {
            m_huntingModel = model ?? throw new ArgumentNullException(nameof(model));
        }

        public List<HuntingModel.HuntingSeason> RefreshHuntingSeasonData(string filePath)
        {
            try
            {
                return HuntingModel.ParseHuntingSeasonData(filePath);
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException("File not found.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while parsing hunting season data.", ex);
            }
        }
    }
}
