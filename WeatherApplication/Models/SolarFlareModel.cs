using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.Models
{
    public class SolarFlareModel
    {
        [JsonProperty("flrID")]
        public string FlareId { get; set; }

        [JsonProperty("beginTime")]
        public DateTime BeginTime { get; set; }

        [JsonProperty("peakTime")]
        public DateTime PeakTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("classType")]
        public string ClassType { get; set; }

        [JsonProperty("sourceLocation")]
        public string SourceLocation { get; set; }

        [JsonProperty("activeRegionNum")]
        public int ActiveRegionNumber { get; set; }
    }
}
