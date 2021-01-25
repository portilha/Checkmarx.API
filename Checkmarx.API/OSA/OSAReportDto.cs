using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Checkmarx.API.OSA
{
    public partial class OSAReportDto
    {
        [JsonProperty("totalLibraries")]
        public int TotalLibraries { get; set; }

        [JsonProperty("highVulnerabilityLibraries")]
        public int HighVulnerabilityLibraries { get; set; }

        [JsonProperty("mediumVulnerabilityLibraries")]
        public int MediumVulnerabilityLibraries { get; set; }

        [JsonProperty("lowVulnerabilityLibraries")]
        public int LowVulnerabilityLibraries { get; set; }

        [JsonProperty("nonVulnerableLibraries")]
        public int NonVulnerableLibraries { get; set; }

        [JsonProperty("vulnerableAndUpdated")]
        public int VulnerableAndUpdated { get; set; }

        [JsonProperty("vulnerableAndOutdated")]
        public int VulnerableAndOutdated { get; set; }

        [JsonProperty("vulnerabilityScore")]
        public string VulnerabilityScore { get; set; }

        [JsonProperty("totalHighVulnerabilities")]
        public int TotalHighVulnerabilities { get; set; }

        [JsonProperty("totalMediumVulnerabilities")]
        public int TotalMediumVulnerabilities { get; set; }

        [JsonProperty("totalLowVulnerabilities")]
        public int TotalLowVulnerabilities { get; set; }
        
        [JsonIgnore]
        public int LibrariesAtLegalRick { get; set; }

        [JsonIgnore]
        public int TotalNumberOutdatedVersionLibraries { get; set; }

        public int GetTotalVulnerableLibraries()
        {
            return LowVulnerabilityLibraries + MediumVulnerabilityLibraries + HighVulnerabilityLibraries;
        }
    }
}
