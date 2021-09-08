using cxPortalWebService93;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkmarx.API
{
    public static class Utils
    {
        public static Uri GetLink(this CxWSSingleResultData result)
        {
            return new Uri(result.PathId.ToString());
        }

    }
}
