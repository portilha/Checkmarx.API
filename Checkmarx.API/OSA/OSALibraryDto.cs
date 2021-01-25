using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Checkmarx.API.OSA
{
        public partial class OSALibraryDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("releaseDate")]
        public DateTimeOffset ReleaseDate { get; set; }

        [JsonProperty("highUniqueVulnerabilityCount")]
        public long HighUniqueVulnerabilityCount { get; set; }

        [JsonProperty("mediumUniqueVulnerabilityCount")]
        public long MediumUniqueVulnerabilityCount { get; set; }

        [JsonProperty("lowUniqueVulnerabilityCount")]
        public long LowUniqueVulnerabilityCount { get; set; }

        [JsonProperty("notExploitableVulnerabilityCount")]
        public long NotExploitableVulnerabilityCount { get; set; }

        [JsonProperty("newestVersion")]
        public object NewestVersion { get; set; }

        [JsonProperty("newestVersionReleaseDate")]
        public object NewestVersionReleaseDate { get; set; }

        [JsonProperty("numberOfVersionsSinceLastUpdate")]
        public long NumberOfVersionsSinceLastUpdate { get; set; }

        [JsonProperty("confidenceLevel")]
        public long ConfidenceLevel { get; set; }

        [JsonProperty("matchType")]
        public MatchType MatchType { get; set; }

        [JsonProperty("licenses")]
        public List<string> Licenses { get; set; }

        [JsonProperty("outdated")]
        public bool Outdated { get; set; }

        [JsonProperty("severity")]
        public Severity Severity { get; set; }

        [JsonProperty("riskScore")]
        public long RiskScore { get; set; }

        [JsonProperty("locations")]
        public List<object> Locations { get; set; }

        [JsonProperty("packageRepository")]
        public string PackageRepository { get; set; }
    }

    public partial class MatchType
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public partial class Severity
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
