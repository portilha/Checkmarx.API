using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Checkmarx.API.OSA
{ 
    public partial class OSALicenseDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("riskLevel")]
        public string RiskLevel { get; set; }

        [JsonProperty("copyrightRiskScore")]
        public long CopyrightRiskScore { get; set; }

        [JsonProperty("patentRiskScore")]
        public long PatentRiskScore { get; set; }

        [JsonProperty("copyLeft")]
        public object CopyLeft { get; set; }

        [JsonProperty("linking")]
        public object Linking { get; set; }

        [JsonProperty("royalityFree")]
        public object RoyalityFree { get; set; }

        [JsonProperty("referenceType")]
        public string ReferenceType { get; set; }

        [JsonProperty("reference")]
        public Uri Reference { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

}
