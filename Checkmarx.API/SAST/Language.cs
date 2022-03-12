namespace Checkmarx.API.SAST
{
    using Newtonsoft.Json;

    public partial class Language
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

