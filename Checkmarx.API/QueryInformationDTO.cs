using System.Collections.Generic;

namespace Checkmarx.API
{
    public class QueryInformationDTO
    {
        public QueryInformationDTO()
        {
                
        }

        public long QueryId { get; set; }
        
        public long PresetQueryId { get; set; }
        
        public string Language { get; set; }
        
        public string PackageType { get; set; }
        
        public string QueryGroup { get; set; }
        
        public string QueryName { get; set; }
        
        public bool IsExecutable { get; set; }
        
        public long CWE { get; set; }
        
        public string Severity { get; set; }

        public string SystemSeverity { get; set; }

        /// <summary>
        /// CWE Categories associated with the Query, separated by comma
        /// </summary>
        public string Categories { get; set; }

        /// <summary>
        /// Key: Preset Name, Value: Is Preset Present in the Scan
        /// </summary>
        public Dictionary<string, bool> PresetsPresence { get; set; }
    }
}