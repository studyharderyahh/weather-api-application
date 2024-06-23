using System;
using System.Collections.Generic;
using System.IO;
using WeatherApplication.Views;
using WeatherApplication.FileHandlers;

namespace WeatherApplication.Controllers
{
    /// <summary>
    /// Represents the controller in the MVC pattern for handling hunting season data.
    /// </summary>
    public class HuntingController
    {
        private readonly HuntingModel m_huntingModel;
        private readonly HuntingView m_huntingView;

        // Singleton logger instance to log application activities
        private readonly Logger logger = Logger.Instance();

        /// <summary>
        /// Initializes a new instance of the <see cref="HuntingController"/> class.
        /// </summary>
        /// <param name="model">The hunting model.</param>
        /// <param name="view">The hunting view.</param>
        /// <exception cref="ArgumentNullException">Thrown when model or view is null.</exception>
        public HuntingController(HuntingModel model, HuntingView view)
        {
            m_huntingModel = model ?? throw new ArgumentNullException(nameof(model));
            m_huntingView = view ?? throw new ArgumentNullException(nameof(view));
        }

        /// <summary>
        /// Loads hunting season data from a file and displays it using the view.
        /// </summary>
        /// <param name="filePath">The path to the hunting season data file.</param>
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
                // Log and display file not found error
                Console.WriteLine($"File not found. {ex.Message}");
                logger.LogError($"File not found. {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log and display general errors
                Console.WriteLine($"An error occurred while parsing hunting season data. {ex.Message}");
                logger.LogError($"An error occurred while parsing hunting season data. {ex.Message}");
            }
        }
    }
}
