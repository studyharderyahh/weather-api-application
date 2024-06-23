using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace WeatherApplication
{
    /// <summary>
    /// The HuntingModel class represents the data and business logic for parsing hunting season data.
    /// </summary>
    public class HuntingModel
    {
        /// <summary>
        /// Represents a hunting season with species, hunting dates, notes, start date, and end date.
        /// </summary>
        public class HuntingSeason
        {
            /// <summary>
            /// Gets the species name for the hunting season.
            /// </summary>
            public string Species { get; }

            /// <summary>
            /// Gets the string representation of hunting dates.
            /// </summary>
            public string HuntingDates { get; }

            /// <summary>
            /// Gets additional notes about the hunting season.
            /// </summary>
            public string Notes { get; }

            /// <summary>
            /// Gets the start date of the hunting season, if available.
            /// </summary>
            public DateTime? StartDate { get; }

            /// <summary>
            /// Gets the end date of the hunting season, if available.
            /// </summary>
            public DateTime? EndDate { get; }

            /// <summary>
            /// Constructor to initialize a hunting season object.
            /// </summary>
            /// <param name="species">The species name.</param>
            /// <param name="huntingDates">The string representation of hunting dates.</param>
            /// <param name="notes">Additional notes about the hunting season.</param>
            /// <param name="startDate">The start date of the hunting season.</param>
            /// <param name="endDate">The end date of the hunting season.</param>
            public HuntingSeason(string species, string huntingDates, string notes, DateTime? startDate, DateTime? endDate)
            {
                Species = species ?? throw new ArgumentNullException(nameof(species));
                HuntingDates = huntingDates ?? throw new ArgumentNullException(nameof(huntingDates));
                Notes = notes ?? throw new ArgumentNullException(nameof(notes));
                StartDate = startDate;
                EndDate = endDate;
            }
        }

        /// <summary>
        /// Parses the hunting season data from a file and returns a list of HuntingSeason objects.
        /// </summary>
        /// <param name="filePath">The path to the file containing hunting season data.</param>
        /// <returns>A list of HuntingSeason objects parsed from the file.</returns>
        public static List<HuntingSeason> ParseHuntingSeasonData(string filePath)
        {
            var huntingSeasons = new List<HuntingSeason>();

            try
            {
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

                    // Parse start and end dates from the huntingDates string
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
            }
            catch (ArgumentNullException ex)
            {
                // Handle ArgumentNullException for null file path or parts
                Console.WriteLine($"Error parsing hunting season data: {ex.Message}");
                throw;
            }
            catch (FileNotFoundException ex)
            {
                // Handle FileNotFoundException for missing file
                Console.WriteLine($"File not found: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle any other exceptions during parsing
                Console.WriteLine($"An error occurred while parsing hunting season data: {ex.Message}");
                throw;
            }

            return huntingSeasons;
        }

        /// <summary>
        /// Filters hunting seasons by the specified month.
        /// </summary>
        /// <param name="huntingSeasons">The list of hunting seasons to filter.</param>
        /// <param name="month">The month (1-12) to filter by.</param>
        /// <returns>A filtered list of HuntingSeason objects that fall within the specified month.</returns>
        public static List<HuntingSeason> SearchByMonth(List<HuntingSeason> huntingSeasons, int month)
        {
            try
            {
                return huntingSeasons.Where(a =>
                    a.StartDate.HasValue && a.EndDate.HasValue &&
                    (a.StartDate.Value.Month <= month && a.EndDate.Value.Month >= month)).ToList();
            }
            catch (ArgumentNullException ex)
            {
                // Handle ArgumentNullException for null huntingSeasons
                Console.WriteLine($"Error filtering hunting seasons by month: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Handle any other exceptions during filtering
                Console.WriteLine($"An error occurred while searching hunting seasons by month: {ex.Message}");
                throw;
            }
        }
    }
}
