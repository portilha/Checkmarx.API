using Newtonsoft.Json;
namespace Checkmarx.API.SAST
{
    public partial class FinishedScanStatus
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
