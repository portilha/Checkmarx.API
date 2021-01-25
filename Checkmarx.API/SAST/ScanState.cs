using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Checkmarx.API.SAST
{
    public partial class ScanState
    {
        [JsonProperty("path")]
        public Uri Path { get; set; }

        [JsonProperty("sourceId")]
        public string SourceId { get; set; }

        [JsonProperty("filesCount")]
        public long FilesCount { get; set; }

        [JsonProperty("linesOfCode")]
        public long LinesOfCode { get; set; }

        [JsonProperty("failedLinesOfCode")]
        public long FailedLinesOfCode { get; set; }

        [JsonProperty("cxVersion")]
        public string CxVersion { get; set; }

        [JsonProperty("languageStateCollection")]
        public List<LanguageStateCollection> LanguageStateCollection { get; set; }
    }
}
