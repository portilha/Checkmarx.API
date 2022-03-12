using Newtonsoft.Json;
namespace Checkmarx.API.SAST
{
    public partial class Status
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("details")]
        public Details Details { get; set; }
    }
}

