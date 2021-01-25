using System;

namespace Checkmarx.API.OSA
{
    public class OSAFindings
    {
        public Guid Id;
        public int Low { get; set; }
        public int Medium { get; set; }
        public int High { get; set; }
        public int NumberOfLibraries { get; set; }
        public int VulnerableLibraries { get; set; }
    }
}