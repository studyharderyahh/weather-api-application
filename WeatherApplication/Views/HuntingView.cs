using System;
using System.Collections.Generic;

namespace WeatherApplication.Views
{
    public class HuntingView
    {
        /// <summary>
        /// Renders the hunting season data to the console.
        /// </summary>
        /// <param name="seasons">List of HuntingSeason objects containing hunting season information.</param>
        public void Render(List<HuntingModel.HuntingSeason> seasons)
        {
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
            Console.WriteLine("\nHunting Season Data:");
            Console.WriteLine("--------------------");

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
            Console.WriteLine($"Notes: {season.Notes}");
            Console.WriteLine();
        }
    }
}
