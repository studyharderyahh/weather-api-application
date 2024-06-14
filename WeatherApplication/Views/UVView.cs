using System;
using System.Collections.Generic;
using WeatherApplication.Models;

namespace WeatherApplication.Views
{
    /// <summary>
    /// View class responsible for displaying UV data to the console.
    /// </summary>
    public class UVView
    {
        /// <summary>
        /// Displays the UV data including coordinates and product details.
        /// </summary>
        /// <param name="uvModel">The UVModel containing UV data to display.</param>
        public void DisplayUVData(UVModel uvModel)
        {
            if (uvModel == null)
            {
                Console.WriteLine("No UV data available.");
                return;
            }

            PrintCoordinates(uvModel.Coord);

            foreach (var product in uvModel.Products)
            {
                DisplayProductData(product);
            }
        }

        /// <summary>
        /// Prints the coordinates associated with the UV data.
        /// </summary>
        /// <param name="coordinates">The coordinates to print.</param>
        private void PrintCoordinates(string coordinates)
        {
            Console.WriteLine($"Coordinates: {coordinates}");
        }

        /// <summary>
        /// Displays detailed data for a specific UV product.
        /// </summary>
        /// <param name="product">The Product object containing UV data to display.</param>
        /// <param name="maxValuesToShow">Maximum number of UV values to display.</param>
        private void DisplayProductData(Product product, int maxValuesToShow = 10)
        {
            Console.WriteLine($"\nProduct Name: {product.Name}");
            DisplayUVValues(product.Values, maxValuesToShow);
        }

        /// <summary>
        /// Displays UV values associated with a specific product.
        /// </summary>
        /// <param name="values">List of UV values to display.</param>
        /// <param name="maxValuesToShow">Maximum number of UV values to display.</param>
        private void DisplayUVValues(List<Value> values, int maxValuesToShow)
        {
            foreach (var value in values)
            {
                if (maxValuesToShow <= 0)
                {
                    break;
                }

                PrintUVValue(value);
                maxValuesToShow--;
            }
        }

        /// <summary>
        /// Prints a single UV value to the console.
        /// </summary>
        /// <param name="value">The UV value object containing time and UV index.</param>
        private void PrintUVValue(Value value)
        {
            Console.WriteLine($"Time: {value.Time}, UV Index: {value.UVValue}");
        }
    }
}
