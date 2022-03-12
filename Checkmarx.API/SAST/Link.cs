namespace Checkmarx.API.SAST
{
    using Newtonsoft.Json;

    public partial class Link
    {
        [JsonProperty("rel")]
        public string Rel { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}

