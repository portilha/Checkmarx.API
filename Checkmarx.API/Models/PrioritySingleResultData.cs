using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkmarx.API.Models
{
    public class PrioritySingleResultData : SoapSingleResultData
    {
        public long SimilarityID { get; set; }

        public System.Nullable<double> CxRank { get; set; }

        public string BflNodeShortName { get; set; }

        public System.Nullable<int> BflGroupSize { get; set; }

        public System.Nullable<System.DateTime> Date { get; set; }
    }
}
