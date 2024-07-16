using Checkmarx.API.OSA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Checkmarx.API.Models
{
    public class SoapSingleResultData
    {
        public long QueryId { get; set; }

        public long PathId { get; set; }

        public string SourceFolder { get; set; }

        public string SourceFile { get; set; }

        public long SourceLine { get; set; }

        public string SourceObject { get; set; }

        public string DestFolder { get; set; }

        public string DestFile { get; set; }

        public long DestLine { get; set; }

        public int NumberOfNodes { get; set; }

        public string DestObject { get; set; }

        public string Comment { get; set; }

        public int State { get; set; }

        public int Severity { get; set; }

        public string AssignedUser { get; set; }

        public System.Nullable<int> ConfidenceLevel { get; set; }

        public ResultStatus ResultStatus { get; set; }

        public string IssueTicketID { get; set; }

        public long QueryVersionCode { get; set; }
    }

    public enum ResultStatus
    {
        Fixed,
        Reoccured,
        New,
    }
}
