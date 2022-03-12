namespace Checkmarx.API.SAST
{
    using Newtonsoft.Json;

    public partial class Stage
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}

