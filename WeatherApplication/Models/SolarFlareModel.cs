using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication.Models
{
    /// <summary>
    /// Represents a solar flare event with its properties.
    /// </summary>
    public class SolarFlareModel
    {
        /// <summary>
        /// Gets or sets the flare ID.
        /// </summary>
        [JsonProperty("flrID")]
        public string FlareId { get; set; }

        /// <summary>
        /// Gets or sets the beginning time of the solar flare event.
        /// </summary>
        [JsonProperty("beginTime")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// Gets or sets the peak time of the solar flare event.
        /// </summary>
        [JsonProperty("peakTime")]
        public DateTime PeakTime { get; set; }

        /// <summary>
        /// Gets or sets the ending time of the solar flare event.
        /// </summary>
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the class type of the solar flare event.
        /// </summary>
        [JsonProperty("classType")]
        public string ClassType { get; set; }

        /// <summary>
        /// Gets or sets the source location of the solar flare event.
        /// </summary>
        [JsonProperty("sourceLocation")]
        public string SourceLocation { get; set; }

        /// <summary>
        /// Gets or sets the active region number associated with the solar flare event.
        /// </summary>
        [JsonProperty("activeRegionNum")]
        public int ActiveRegionNumber { get; set; }

        /// <summary>
        /// Default constructor required for JSON serialization.
        /// </summary>
        public SolarFlareModel()
        {
            // Initialize non-nullable properties with default values
            BeginTime = DateTime.MinValue;
            PeakTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
            ActiveRegionNumber = 0;
        }

        /// <summary>
        /// Constructor to initialize all properties of the SolarFlareModel.
        /// </summary>
        public SolarFlareModel(string flareId, DateTime beginTime, DateTime peakTime, DateTime endTime, string classType, string sourceLocation, int activeRegionNumber)
        {
            FlareId = flareId ?? throw new ArgumentNullException(nameof(flareId));
            BeginTime = beginTime;
            PeakTime = peakTime;
            EndTime = endTime;
            ClassType = classType ?? throw new ArgumentNullException(nameof(classType));
            SourceLocation = sourceLocation ?? throw new ArgumentNullException(nameof(sourceLocation));
            ActiveRegionNumber = activeRegionNumber;
        }
    }
}
