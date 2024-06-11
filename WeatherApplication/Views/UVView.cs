using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.Views
{
    public class UVView
    {
        public void DisplayUVData(UVModel uvModel)
        {
            if (uvModel != null)
            {
                Console.WriteLine($"Coordinates: {uvModel.Coord}");
                foreach (var product in uvModel.Products)
                {
                    int counter = 0;
                    Console.WriteLine($"\nProduct Name: {product.Name}");
                    foreach (var value in product.Values)
                    {
                        if (counter < 10) {
                            Console.WriteLine($"Time: {value.Time}, UV Index: {value.UVValue}");
                        }
                        counter++;

                    }
                }
            }
            else
            {
                Console.WriteLine("No data available.");
            }
        }
    }
}
