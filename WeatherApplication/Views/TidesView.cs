﻿using System;
using System.Collections.Generic;
using WeatherApplication.Models;

namespace WeatherApplication.Views
{
    /// <summary>
    /// View class responsible for rendering tide data.
    /// </summary>
    public class TidesView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TidesView"/> class.
        /// </summary>
        /// <param name="tidesModel">Instance of TidesModel to subscribe to progress updates.</param>
        public TidesView(TidesModel tidesModel)
        {
            // Subscribe to the ProgressUpdated event of TidesModel
            tidesModel.ProgressUpdated += HandleProgressUpdate;
        }

        /// <summary>
        /// Handles progress update events from TidesModel and displays them in the console.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="message">The progress message to display.</param>
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
