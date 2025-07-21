using Newtonsoft.Json;
namespace Checkmarx.API.SAST
{
    public partial class Status
    {
        public const string Finished = "Finished";
        public const string Canceled = "Canceled";
        public const string Failed = "Failed";

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("details")]
        public Details Details { get; set; }
    }
}

