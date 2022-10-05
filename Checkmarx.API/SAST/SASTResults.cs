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

        public uint High { get; set; }

        public uint Medium { get; set; }

        public uint Low { get; set; }

        public uint Info { get; set; }

        public int FalseNegatives { get; set; }

        public int FalsePositives { get; set; }

        public int Queries { get; set; }

        public ICollection<string> LanguagesDetected { get; set; }
    }
}
