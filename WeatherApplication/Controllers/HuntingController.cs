using System;
using System.Collections.Generic;
using System.IO;
using WeatherApplication.Views;
using WeatherApplication.FileHandlers;

namespace WeatherApplication.Controllers
{
    // Represents the controller in the MVC pattern
    public class HuntingController
    {
        private readonly HuntingModel m_huntingModel;
        private readonly HuntingView m_huntingView;

        Logger logger = Logger.Instance();

        // Constructor to initialize the model and view
        public HuntingController(HuntingModel model, HuntingView view)
        {
            m_huntingModel = model ?? throw new ArgumentNullException(nameof(model));
            m_huntingView = view ?? throw new ArgumentNullException(nameof(view));
        }

        // Loads hunting season data from a file and displays it using the view
        public void LoadAndDisplayHuntingSeasonData(string filePath)
        {
            try
            {
                // Parse the hunting season data using the model
                var seasons = HuntingModel.ParseHuntingSeasonData(filePath);
                // Render the parsed data using the view
                m_huntingView.Render(seasons);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found. {ex.Message}");
                logger.LogError($"File not found. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while parsing hunting season data. {ex.Message}");
                logger.LogError($"An error occurred while parsing hunting season data. {ex.Message}");

            }
        }
    }
}
