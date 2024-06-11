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
        [JsonProperty("products")]
        public List<Product> Products { get; set; }

        [JsonProperty("coord")]
        public string Coord { get; set; }
    }

    public class Product
    {
        [JsonProperty("values")]
        public List<Value> Values { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Value
    {
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("value")]
        public double UVValue { get; set; }
    }
}
