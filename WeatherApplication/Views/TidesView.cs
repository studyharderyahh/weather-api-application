using System;
using System.Collections.Generic;
using WeatherApplication.Models;

namespace WeatherApplication.Views
{
    public class TidesView
    {

        public TidesView(TidesModel tidesModel)
        {
            tidesModel.ProgressUpdated += HandleProgressUpdate;
        }

        private void HandleProgressUpdate(object sender, string message)
        {
            Console.WriteLine(message); // Display progress in console
        }
        /// <summary>
        /// Renders the tide data including metadata and values.
        /// </summary>
        /// <param name="tideData">The tide data to be rendered.</param>
        public void Render(TidesModel.TidesData tideData)
        {
            Console.WriteLine("Tides Data:");

            RenderMetadata(tideData);
            RenderTideValues(tideData.Values);
        }

        /// <summary>
        /// Renders the metadata of the tide data.
        /// </summary>
        /// <param name="tideData">The tide data containing metadata.</param>
        private void RenderMetadata(TidesModel.TidesData tideData)
        {
            Console.WriteLine("\nMetadata:");
            Console.WriteLine($"Latitude: {tideData.Metadata.Latitude}");
            Console.WriteLine($"Longitude: {tideData.Metadata.Longitude}");
            Console.WriteLine($"Datum: {tideData.Metadata.Datum}");
            Console.WriteLine($"Start Date: {tideData.Metadata.Start}");
            Console.WriteLine($"Number of Days: {tideData.Metadata.Days}");
            Console.WriteLine($"Interval: {tideData.Metadata.Interval}");
            Console.WriteLine($"Height: {tideData.Metadata.Height}");
        }

        /// <summary>
        /// Renders the tide values.
        /// </summary>
        /// <param name="values">The list of tide values to be rendered.</param>
        /// <param name="maxValuesToShow">The maximum number of tide values to show. Default is 10.</param>
        private void RenderTideValues(List<TidesModel.TideValue> values, int maxValuesToShow = 10)
        {
            Console.WriteLine("\nTide Values:");
            for (int i = 0; i < values.Count && i < maxValuesToShow; i++)
            {
                Console.WriteLine($"Time: {values[i].Time}, Value: {values[i].Value}");
            }
        }
    }
}
