using System.Collections.Generic;

namespace Checkmarx.API.SAST
{
    public class SASTResults
    {
        public long Id { get; set; }
        public int FailedLoC { get; set; }

        /// <summary>
        /// Get from the comments on the Queries
        /// </summary>
        public ICollection<string> TunningInfo { get; set; }

        public int FirstLoc { get; set; }

        public uint FirstHigh { get; set; }

        public uint FirstMedium { get; set; }

        public uint FirstLow { get; set; }

        public int FinalLoc { get; set; }

        public uint FinalHigh { get; set; }

        public uint FinalMedium { get; set; }

        public uint FinalLow { get; set; }

        public uint Info { get; set; }

        public int FalseNegatives { get; set; }

        public int FalsePositives { get; set; }
        public int TotalNotExploitableMarked { get; set; }
        public int TotalFinalResults { get; set; }

        public int Queries { get; set; }

        public ICollection<string> LanguagesDetected { get; set; }
    }
}
