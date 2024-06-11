using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherApplication.APIHelpers;

namespace WeatherApplication
{
    public class UVModel
    {
        private readonly HttpClientWrapper m_httpClient = HttpClientWrapper.Instance; // only need one browser // Singleton instance of HttpClientWrapper

        private string m_apiKey; // API key for authentication

        public string Coord { get; set; }

        public class UVData
        {
            [Required]
            public List<UVProduct> Products { get; set; }

            [Required]
            public string Coord { get; set; }
        }

        public class UVProduct
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public List<UVDataEntry> Values { get; set; }
        }

        public class UVDataEntry
        {
            [Required]
            public DateTime Time { get; set; }

            [Required]
            [Range(0, double.MaxValue)]
            public double Value { get; set; }
        }

        // Constructor to check if APIKey is null
        public UVModel(string apiKey, CoordInfo inputCoord)
        {
            m_apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        public class UVDataDeserializer
        {
            public UVData Deserialize(string json)
            {
                var uvData = new UVData { Products = new List<UVProduct>() };

                var jsonObject = JObject.Parse(json);
                if (jsonObject == null)
                {
                    throw new ArgumentNullException(nameof(json), "JSON object is null.");
                }

                // Check for the "coord" property
                var coordToken = jsonObject["coord"];
                if (coordToken == null)
                {
                    throw new JsonSerializationException("Missing 'coord' property in JSON object.");
                }
                uvData.Coord = coordToken.Value<string>();

                // Check for the "products" array
                var productsToken = jsonObject["products"];
                if (productsToken == null || !productsToken.HasValues)
                {
                    throw new JsonSerializationException("Missing 'products' array in JSON object.");
                }

                foreach (var productJson in productsToken)
                {
                    // Check for the "name" property
                    var nameToken = productJson["name"];
                    if (nameToken == null)
                    {
                        throw new JsonSerializationException("Missing 'name' property in product JSON object.");
                    }

                    var uvProduct = new UVProduct
                    {
                        Name = nameToken.Value<string>(),
                        Values = new List<UVDataEntry>()
                    };

                    // Check for the "values" array
                    var valuesToken = productJson["values"];
                    if (valuesToken == null || !valuesToken.HasValues)
                    {
                        throw new JsonSerializationException("Missing 'values' array in product JSON object.");
                    }

                    foreach (var valueJson in valuesToken)
                    {
                        // Check for the "time" property
                        var timeToken = valueJson["time"];
                        if (timeToken == null)
                        {
                            throw new JsonSerializationException("Missing 'time' property in value JSON object.");
                        }

                        // Check for the "value" property
                        var valueToken = valueJson["value"];
                        if (valueToken == null)
                        {
                            throw new JsonSerializationException("Missing 'value' property in value JSON object.");
                        }

                        var uvDataEntry = new UVDataEntry
                        {
                            Time = DateTime.Parse(timeToken.ToString()),
                            Value = double.Parse(valueToken.ToString())
                        };
                        uvProduct.Values.Add(uvDataEntry);
                    }
                    uvData.Products.Add(uvProduct);
                }
                return uvData;
            }
        }

        internal async Task<UVData> GetUVDataAsync(string apiKey, CoordInfo coordInfo)
        {

            throw new NotImplementedException();
        }
    }
}
