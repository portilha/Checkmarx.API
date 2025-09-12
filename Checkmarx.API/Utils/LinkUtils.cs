using Checkmarx.API.Models;
using CxDataRepository;
using System;

namespace Checkmarx.API.Utils
{
    public static class LinkUtils
    {
        /// <summary>
        /// https://.checkmarx.net/cxwebclient/ViewerMain.aspx?scanid=1032992&projectid=12&pathid=1
        /// </summary>
        /// <param name="result"></param>
        /// <param name="clientV93"></param>
        /// <returns></returns>
        public static Uri GetLink(this SoapSingleResultData result, CxClient clientV93, long projectId, long scanId)
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

        public static Uri GetLink(this Checkmarx.API.SAST.OData.Result item, CxClient clientV94, long projectId, long scanId)
        {
            return new Uri(clientV94.SASTServerURL, $"cxwebclient/ViewerMain.aspx?scanid={scanId}&projectid={projectId}&pathid={item.PathId}");
        }

        public static Uri GetLink(this CxDataRepository97.Result item, CxClient clientV94, long projectId, long scanId)
        {
            return new Uri(clientV94.SASTServerURL, $"cxwebclient/ViewerMain.aspx?scanid={scanId}&projectid={projectId}&pathid={item.PathId}");
        }
    }
}
