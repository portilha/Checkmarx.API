namespace Checkmarx.API.SAST
{
    using Newtonsoft.Json;

    public partial class Project
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public Link Link { get; set; }
    }
}

