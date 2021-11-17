using CxDataRepository;
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

        /// <summary>
        /// https://.checkmarx.net/cxwebclient/ViewerMain.aspx?scanid=1032992&projectid=12&pathid=1
        /// </summary>
        /// <param name="result"></param>
        /// <param name="clientV93"></param>
        /// <returns></returns>
        public static Uri GetLink(this PortalSoap.CxWSSingleResultData result, CxClient clientV93, long projectId, long scanId)
        {
            return new Uri(clientV93.SASTServerURL, $"cxwebclient/ViewerMain.aspx?scanid={scanId}&projectid={projectId}&pathid={result.PathId}");
        }

        public static Uri GetLink(this CxAuditWebServiceV9.CxWSResultPath resultPath, CxClient clientV93, long projectId, long scanId)
        {
            return new Uri(clientV93.SASTServerURL, $"cxwebclient/ViewerMain.aspx?scanid={scanId}&projectid={projectId}&pathid={resultPath.PathId}");
        }

        public static Uri GetLink(this Result item, CxClient clientV93, long? projectId, long? scanId)
        {
            return new Uri(clientV93.SASTServerURL, $"cxwebclient/ViewerMain.aspx?scanid={scanId}&projectid={projectId}&pathid={item.PathId}");
        }
    }
}
