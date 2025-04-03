using System;

namespace Checkmarx.API.Interfaces
{
    public interface IODataScan
    {
        public bool IsPublic { get; set; }
        public string ProductVersion { get; set; }
        public DateTimeOffset? ScanRequestedOn { get; set; }
        public DateTimeOffset? EngineFinishedOn { get; set; }
        public int ScanType { get; set; }
        public bool IsLocked { get; set; }
    }
}
