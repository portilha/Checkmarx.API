using System;
using Newtonsoft.Json;

namespace Checkmarx.API.OSA
{
    public partial class OSAScanDto
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("startAnalyzeTime")]
        public DateTimeOffset StartAnalyzeTime { get; set; }

        [JsonProperty("endAnalyzeTime")]
        public DateTimeOffset EndAnalyzeTime { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("sourceCodeOrigin")]
        public string SourceCodeOrigin { get; set; }

        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("sharedSourceLocationPaths")]
        public object SharedSourceLocationPaths { get; set; }
    }

    public partial class State
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("failureReason")]
        public object FailureReason { get; set; }
    }
}
