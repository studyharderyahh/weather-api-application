using System;
using System.Collections.Generic;
using System.IO;
using WeatherApplication.Views;

namespace WeatherApplication.Controllers
{
    public class HuntingController
    {
        private readonly HuntingModel m_huntingModel;
        private readonly HuntingView m_huntingView;

        public HuntingController(HuntingModel model, HuntingView view)
        {
            m_huntingModel = model ?? throw new ArgumentNullException(nameof(model));
            m_huntingView = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void LoadAndDisplayHuntingSeasonData(string filePath)
        {
            try
            {
                var seasons = m_huntingModel.ParseHuntingSeasonData(filePath);
                m_huntingView.Render(seasons);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File not found.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while parsing hunting season data.");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
