using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Checkmarx.API
{
    public partial class ProjectDetails
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("teamId")]
        public string TeamId { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }

        [JsonProperty("customFields")]
        public List<CustomField> CustomFields { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }
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
