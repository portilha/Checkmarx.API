﻿using Checkmarx.API.SAST.OData;
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
        internal static Default.ODataClient8 ConnectToOData(Uri webserverAddress, string username, string password)
        {
            Console.WriteLine($"Connecting to OData ({webserverAddress.AbsoluteUri}CxWebInterface/odata/v1/)");

            Uri serviceUri = new Uri(webserverAddress, "/CxWebInterface/odata/v1/");
            Default.ODataClient8 context = new Default.ODataClient8(serviceUri);
            context.Timeout = 7200;

            string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            //Registering the handle to the BuildingRequest event. 
            context.BuildingRequest += (sender, e) =>
            {
                e.Headers.Add("Authorization", $"Basic {auth}");
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
        internal static DefaultV9.Container ConnectToODataV9(Uri webserverAddress, string bearerToken)
        {
            Console.WriteLine($"Connecting to OData V9 ({webserverAddress.AbsoluteUri}CxWebInterface/odata/v1/)");

            Uri serviceUri = new Uri(webserverAddress, "/CxWebInterface/odata/v1/");
            DefaultV9.Container context = new DefaultV9.Container(serviceUri);
            context.Timeout = 7200;

            //string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            //Registering the handle to the BuildingRequest event. 
            context.BuildingRequest += (sender, e) =>
            {
                e.Headers.Add("Authorization", $"Bearer {bearerToken}");
            };

            Console.WriteLine("Connected to OData V9!");

            return context;
        }

        internal static ODataClient95 ConnectToODataV95(Uri webserverAddress, string bearerToken)
        {
            Console.WriteLine($"Connecting to OData V95 ({webserverAddress.AbsoluteUri}CxWebInterface/odata/v1/)");

            Uri serviceUri = new Uri(webserverAddress, "/CxWebInterface/odata/v1/");
            var context = new ODataClient95(serviceUri);
            context.Timeout = 7200;

            //string auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
            //Registering the handle to the BuildingRequest event. 
            context.BuildingRequest += (sender, e) =>
            {
                e.Headers.Add("Authorization", $"Bearer {bearerToken}");
            };

            Console.WriteLine("Connected to OData V9!");

            return context;
        }
    }
}
