using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Checkmarx.API
{
    public partial class ProjectDetails
    {
        [JsonProperty("id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long Id { get; set; }

        [JsonProperty("teamId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string TeamId { get; set; }

        [JsonProperty("name", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("isPublic", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPublic { get; set; }

        [JsonProperty("customFields", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<CustomField> CustomFields { get; set; }

        [JsonProperty("links", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<Link> Links { get; set; }

        [JsonProperty("owner", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Owner { get; set; }

        [JsonProperty("isDeprecated", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDeprecated { get; set; }

        [JsonProperty("isBranched", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsBranched { get; set; }

        [JsonProperty("originalProjectId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalProjectId { get; set; }

        [JsonProperty("branchedOnScanId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string BranchedOnScanId { get; set; }

        [JsonProperty("relatedProjects", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<long> RelatedProjects { get; set; }
    }

    public partial class CustomField
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class SourceSettingsLink
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("rel")]
        public string Rel { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
