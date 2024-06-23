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
    /// <summary>
    /// Represents the UV model containing products and coordinates.
    /// </summary>
    public class UVModel
    {
        /// <summary>
        /// Gets or sets the list of products containing UV values.
        /// </summary>
        [JsonProperty("products")]
        public List<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the coordinates associated with the UV data.
        /// </summary>
        [JsonProperty("coord")]
        public string Coord { get; set; }
    }

    /// <summary>
    /// Represents a product containing UV values and its associated name.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the list of UV values.
        /// </summary>
        [JsonProperty("values")]
        public List<Value> Values { get; set; }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Represents a single UV value at a specific time.
    /// </summary>
    public class Value
    {
        /// <summary>
        /// Gets or sets the timestamp of the UV measurement.
        /// </summary>
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or sets the UV value.
        /// </summary>
        [JsonProperty("value")]
        public double UVValue { get; set; }
    }
}
