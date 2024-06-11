using System;
using System.Collections.Generic;

namespace WeatherApplication.Views
{
    public class HuntingView
    {
        // Renders the hunting season data to the console
        public void Render(List<HuntingModel.HuntingSeason> seasons)
        {
            Console.WriteLine("\nHunting Season Data:");
            Console.WriteLine("--------------------");
            foreach (var season in seasons)
            {
                Console.WriteLine($"Species: {season.Species}");
                Console.WriteLine($"Hunting Dates: {season.HuntingDates}");
                Console.WriteLine($"Notes: {season.Notes}");
                Console.WriteLine();
            }
        }
    }
}
