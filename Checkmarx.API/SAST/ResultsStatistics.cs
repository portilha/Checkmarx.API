using Newtonsoft.Json;
namespace Checkmarx.API.SAST
{
    public partial class ResultsStatistics
    {
        [JsonProperty("link")]
        public object Link { get; set; }
    }
}
