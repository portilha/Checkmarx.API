namespace Checkmarx.API.SAST
{
    using System;
    using Newtonsoft.Json;

    public partial class ScanQueue
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("stage")]
        public Stage Stage { get; set; }

        [JsonProperty("stageDetails")]
        public string StageDetails { get; set; }

        [JsonProperty("stepDetails")]
        public string StepDetails { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }


        [Newtonsoft.Json.JsonProperty("engineId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public long? EngineId { get; set; }

        [Newtonsoft.Json.JsonProperty("engineFinishedOn", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.DateTimeOffset? EngineFinishedOn { get; set; }


        [JsonProperty("languages")]
        public Language[] Languages { get; set; }

        [JsonProperty("teamId")]
        public string TeamId { get; set; }

        [JsonProperty("dateCreated")]
        public DateTimeOffset DateCreated { get; set; }

        [JsonProperty("queuedOn")]
        public DateTimeOffset? QueuedOn { get; set; }

        [JsonProperty("engineStartedOn")]
        public DateTimeOffset? EngineStartedOn { get; set; }

        [JsonProperty("completedOn")]
        public DateTimeOffset? CompletedOn { get; set; }

        [JsonProperty("loc")]
        public long Loc { get; set; }

        [JsonProperty("isIncremental")]
        public bool IsIncremental { get; set; }

        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("queuePosition")]
        public long QueuePosition { get; set; }

        [JsonProperty("totalPercent")]
        public long TotalPercent { get; set; }

        [JsonProperty("stagePercent")]
        public long StagePercent { get; set; }

        [JsonProperty("initiator")]
        public string Initiator { get; set; }
    }
}

