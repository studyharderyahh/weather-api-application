using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    // The HuntingModel class represents the data and the business logic for parsing hunting season data.
    public class HuntingModel
    {
        public class HuntingSeason
        {
            public string Species { get; }
            public string HuntingDates { get; }
            public string Notes { get; }
            public DateTime? StartDate { get; }
            public DateTime? EndDate { get; }

            // Constructor to initialize the hunting season
            public HuntingSeason(string species, string huntingDates, string notes, DateTime? startDate, DateTime? endDate)
            {
                Species = species ?? throw new ArgumentNullException(nameof(species));
                HuntingDates = huntingDates ?? throw new ArgumentNullException(nameof(huntingDates));
                Notes = notes ?? throw new ArgumentNullException(nameof(notes));
                StartDate = startDate;
                EndDate = endDate;
            }
        }

        public static List<HuntingSeason> ParseHuntingSeasonData(string filePath)
        {
            var huntingSeasons = new List<HuntingSeason>();

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            var dateFormats = new[] { "MMMM", "MMMM yyyy", "MMMM dd, yyyy", "MMM", "MMM yyyy", "MMM dd, yyyy" };

            // Skip the header line (first line)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                // Split each line by comma
                string[] parts = line.Split(new[] { ',' }, 3);

                string species = parts[0].Trim();
                string huntingDates = parts[1].Trim();
                string notes = parts.Length > 2 ? parts[2].Trim() : ""; // Handle cases where Notes field is empty

                DateTime? startDate = null;
                DateTime? endDate = null;

                var dates = huntingDates.Split(new[] { " to ", " through ", " (", ")" }, StringSplitOptions.RemoveEmptyEntries);
                if (dates.Length == 2)
                {
                    DateTime parsedStartDate, parsedEndDate;
                    if (DateTime.TryParseExact(dates[0].Trim(), dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) &&
                        DateTime.TryParseExact(dates[1].Trim(), dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate))
                    {
                        startDate = parsedStartDate;
                        endDate = parsedEndDate;
                    }
                }

                var season = new HuntingSeason(species, huntingDates, notes, startDate, endDate);
                huntingSeasons.Add(season);
            }

            return huntingSeasons;
        }

        public static List<HuntingSeason> SearchByMonth(List<HuntingSeason> huntingSeasons, int month)
        {
            return huntingSeasons.Where(a =>
                a.StartDate.HasValue && a.EndDate.HasValue &&
                (a.StartDate.Value.Month <= month && a.EndDate.Value.Month >= month)).ToList();
        }
    }
}
