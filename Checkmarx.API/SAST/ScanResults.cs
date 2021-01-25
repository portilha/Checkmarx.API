using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Checkmarx.API.SAST
{

    public partial class ScanResults
    {
        [JsonProperty("highSeverity")]
        public uint HighSeverity { get; set; }

        [JsonProperty("mediumSeverity")]
        public uint MediumSeverity { get; set; }

        [JsonProperty("lowSeverity")]
        public uint LowSeverity { get; set; }

        [JsonProperty("infoSeverity")]
        public uint InfoSeverity { get; set; }

        [JsonProperty("statisticsCalculationDate")]
        public DateTimeOffset StatisticsCalculationDate { get; set; }
    }

}
