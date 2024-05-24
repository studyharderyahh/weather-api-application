using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class HuntingModel
    {
        public class HuntingSeason
        {
            public string Species { get; }
            public string HuntingDates { get; }
            public string Notes { get; }

            public HuntingSeason(string species, string huntingDates, string notes)
            {
                Species = species ?? throw new ArgumentNullException(nameof(species));
                HuntingDates = huntingDates ?? throw new ArgumentNullException(nameof(huntingDates));
                Notes = notes ?? throw new ArgumentNullException(nameof(notes));
            }
        }

        public static List<HuntingSeason> ParseHuntingSeasonData(string filePath)
        {
            var huntingSeasons = new List<HuntingSeason>();

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Skip the header line (first line)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                // Split each line by comma
                string[] parts = line.Split(',');

                // Create a new HuntingSeason object and populate its properties

                string Species = parts[0];
                string HuntingDates = parts[1];
                string Notes = parts.Length > 2 ? parts[2] : ""; // Handle cases where Notes field is empty


                var season = new HuntingSeason(Species, HuntingDates, Notes);


                huntingSeasons.Add(season);

            }
            return huntingSeasons;

        }
    }
}
