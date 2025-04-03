using Checkmarx.API.SAST.OData;
using System;
using System.Text;

namespace Checkmarx.API
{
    internal static class CxOData
    {
        /// <summary>
        /// Returns Container from V1 ODATA Version 8.9
        /// </summary>
        /// <param name="webserverAddress"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Container from CxSASTOData</returns>
        internal static Default.ODataClient8 ConnectToOData(Uri webserverAddress, string username, string password, int defaultRetries = 10)
        {
            Console.WriteLine($"Connecting to OData ({webserverAddress.AbsoluteUri}CxWebInterface/odata/v1/)");

            Uri serviceUri = new Uri(webserverAddress, "/CxWebInterface/odata/v1/");
            Default.ODataClient8 context = new Default.ODataClient8(serviceUri, defaultRetries)
            {
                Timeout = 7200,
                MergeOption = Microsoft.OData.Client.MergeOption.NoTracking

            };

            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            //Registering the handle to the BuildingRequest event. 
            context.BuildingRequest += (sender, e) =>
            {
                e.Headers.Add("Authorization", $"Basic {auth}");
                e.Headers.Add("Keep-Alive", "timeout=60, max=1000");
            };

            Console.WriteLine("Connected to OData!");

            return context;
        }

        /// <summary>
        /// Returns Container from V1 ODATA Version 9
        /// </summary>
        /// <param name="webserverAddress"></param>
        /// <param name="bearerToken"></param>
        /// <returns>Container from CxSASTODataV9</returns>
        internal static DefaultV9.Container ConnectToODataV9(Uri webserverAddress, string bearerToken, int defaultRetries = 10)
        {
            Console.WriteLine($"Connecting to OData V9 ({webserverAddress.AbsoluteUri}CxWebInterface/odata/v1/)");

            Uri serviceUri = new Uri(webserverAddress, "/CxWebInterface/odata/v1/");
            DefaultV9.Container context = new DefaultV9.Container(serviceUri, defaultRetries)
            {
                Timeout = 7200,
                MergeOption = Microsoft.OData.Client.MergeOption.NoTracking,
                SaveChangesDefaultOptions = Microsoft.OData.Client.SaveChangesOptions.None

            };

            //string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            //Registering the handle to the BuildingRequest event. 
            context.BuildingRequest += (sender, e) =>
            {
                e.Headers.Add("Authorization", $"Bearer {bearerToken}");
                e.Headers.Add("Keep-Alive", "timeout=60, max=1000");
            };

            Console.WriteLine("Connected to OData V9!");

            return context;
        }

        internal static ODataClient95 ConnectToODataV95(Uri webserverAddress, string bearerToken, int defaultRetries = 10)
        {
            Console.WriteLine($"Connecting to OData V95 ({webserverAddress.AbsoluteUri}CxWebInterface/odata/v1/)");

            Uri serviceUri = new Uri(webserverAddress, "/CxWebInterface/odata/v1/");
            var context = new ODataClient95(serviceUri, defaultRetries)
            {
                Timeout = 7200,
                MergeOption = Microsoft.OData.Client.MergeOption.NoTracking
            };

            //string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            //Registering the handle to the BuildingRequest event. 
            context.BuildingRequest += (sender, e) =>
            {
                e.Headers.Add("Authorization", $"Bearer {bearerToken}");
                e.Headers.Add("Keep-Alive", "timeout=60, max=1000");
            };

            Console.WriteLine("Connected to OData V9!");

            return context;
        }

        internal static CxDataRepository97.Container ConnectToODataV97(Uri webserverAddress, string bearerToken, int defaultRetries = 10)
        {
            Console.WriteLine($"Connecting to OData V97 ({webserverAddress.AbsoluteUri}CxWebInterface/odata/v1/)");

            Uri serviceUri = new Uri(webserverAddress, "/CxWebInterface/odata/v1/");
            var context = new CxDataRepository97.Container(serviceUri, defaultRetries)
            {
                Timeout = 7200,
                MergeOption = Microsoft.OData.Client.MergeOption.NoTracking
            };

            //string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            //Registering the handle to the BuildingRequest event. 
            context.BuildingRequest += (sender, e) =>
            {
                e.Headers.Add("Authorization", $"Bearer {bearerToken}");
                e.Headers.Add("Keep-Alive", "timeout=60, max=1000");
            };

            Console.WriteLine("Connected to OData V9!");

            return context;
        }
    }
}
