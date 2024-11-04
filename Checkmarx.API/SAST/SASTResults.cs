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

        public int Loc { get; set; }

        public uint Critical { get; set; }

        public uint High { get; set; }

        public uint Medium { get; set; }

        public uint Low { get; set; }

        public uint Info { get; set; }

        public uint CriticalToVerify { get; set; }

        public uint HighToVerify { get; set; }

        public uint MediumToVerify { get; set; }

        public uint LowToVerify { get; set; }

        public uint ToVerify { get; set; }

        public uint NotExploitableMarked { get; set; }

        public uint PNEMarked { get; set; }

        public uint OtherStates { get; set; }

        public int FalseNegatives { get; set; }

        public int FalsePositives { get; set; }

        public int Queries { get; set; }

        public ICollection<string> LanguagesDetected { get; set; }
    }
}
