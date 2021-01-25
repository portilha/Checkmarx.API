using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Checkmarx.API.SAST
{
    public partial class Scan
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("project")]
        public EngineServer Project { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("scanType")]
        public FinishedScanStatus ScanType { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("dateAndTime")]
        public DateAndTime DateAndTime { get; set; }

        [JsonProperty("resultsStatistics")]
        public ResultsStatistics ResultsStatistics { get; set; }

        [JsonProperty("scanState")]
        public ScanState ScanState { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("initiatorName")]
        public string InitiatorName { get; set; }

        [JsonProperty("owningTeamId")]
        public Guid OwningTeamId { get; set; }

        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }

        [JsonProperty("isLocked")]
        public bool IsLocked { get; set; }

        [JsonProperty("isIncremental")]
        public bool IsIncremental { get; set; }

        [JsonProperty("scanRisk")]
        public int ScanRisk { get; set; }

        [JsonProperty("scanRiskSeverity")]
        public long ScanRiskSeverity { get; set; }

        [JsonProperty("engineServer")]
        public EngineServer EngineServer { get; set; }

        [JsonProperty("finishedScanStatus")]
        public FinishedScanStatus FinishedScanStatus { get; set; }

        [JsonProperty("partialScanReasons")]
        public object PartialScanReasons { get; set; }
  
        public static Scan FromJson(string json) => JsonConvert.DeserializeObject<Scan>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Scan self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
