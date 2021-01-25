using System;
using Newtonsoft.Json;
namespace Checkmarx.API.SAST
{
    public partial class LanguageStateCollection
    {
        [JsonProperty("languageID")]
        public long LanguageId { get; set; }

        [JsonProperty("languageName")]
        public string LanguageName { get; set; }

        [JsonProperty("languageHash")]
        public string LanguageHash { get; set; }

        [JsonProperty("stateCreationDate")]
        public DateTimeOffset StateCreationDate { get; set; }
    }
}
