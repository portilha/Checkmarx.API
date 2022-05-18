namespace Checkmarx.API.SAST
{
    using Newtonsoft.Json;

    public partial class Engine
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("link")]
        public Link Link { get; set; }
    }
}

