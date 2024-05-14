using System;
using System.Collections.Generic;

namespace WeatherApplication
{
    public class HuntingView
    {
        public void Render(List<HuntingModel.HuntingSeason> seasons)
        {
            Console.WriteLine("Hunting Season Data:");
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
