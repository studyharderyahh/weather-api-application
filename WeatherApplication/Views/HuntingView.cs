using System;
using System.Collections.Generic;

namespace WeatherApplication.Views
{
    /// <summary>
    /// View class responsible for rendering hunting season data to the console.
    /// </summary>
    public class HuntingView
    {
        /// <summary>
        /// Renders the hunting season data to the console.
        /// </summary>
        /// <param name="seasons">List of HuntingSeason objects containing hunting season information.</param>
        public void Render(List<HuntingModel.HuntingSeason> seasons)
        {
            // Check if seasons list is null or empty
            if (seasons == null || seasons.Count == 0)
            {
                Console.WriteLine("No hunting season data available.");
                return;
            }

            RenderSeasons(seasons);
        }

        /// <summary>
        /// Renders details of multiple hunting seasons.
        /// </summary>
        /// <param name="seasons">List of HuntingSeason objects containing hunting season information.</param>
        private void RenderSeasons(List<HuntingModel.HuntingSeason> seasons)
        {
            Console.WriteLine("Hunting Season Data:");
            Console.WriteLine("--------------------");

            // Iterate through each season and render its details
            foreach (var season in seasons)
            {
                RenderSeason(season);
            }
        }

        /// <summary>
        /// Renders details of a single hunting season.
        /// </summary>
        /// <param name="season">HuntingSeason object containing season information.</param>
        private void RenderSeason(HuntingModel.HuntingSeason season)
        {
            Console.WriteLine($"Species: {season.Species}");
            Console.WriteLine($"Hunting Dates: {season.HuntingDates}");

            // Check if notes are available to render
            if (!string.IsNullOrEmpty(season.Notes))
            {
                Console.WriteLine($"Notes: {season.Notes}");
            }

            Console.WriteLine(); // Blank line for separation
        }
    }
}
