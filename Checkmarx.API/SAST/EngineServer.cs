using Newtonsoft.Json;
namespace Checkmarx.API.SAST
{
    public partial class EngineServer
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public object Link { get; set; }
    }
}
