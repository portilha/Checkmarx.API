using Newtonsoft.Json;
namespace Checkmarx.API.SAST
{
    public partial class Details
    {
        [JsonProperty("stage")]
        public string Stage { get; set; }

        [JsonProperty("step")]
        public string Step { get; set; }
    }
}
