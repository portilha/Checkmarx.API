using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Checkmarx.API.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class SASTODataUnitTest
    {

        public static IConfigurationRoot Configuration { get; private set; }

        private static CxClient clientV89;
        private static CxClient clientV9;
        private static CxClient clientV94;

        [ClassInitialize]   
        public static void InitializeTest(TestContext testContext)
        {
            // TODO REMOVE
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<SASTODataUnitTest>();

            Configuration = builder.Build();

            string v8 = Configuration["V89:URL"];

            if (!string.IsNullOrWhiteSpace(v8))
            {
                clientV89 =
                        new CxClient(new Uri(v8),
                        Configuration["V89:Username"],
                        new NetworkCredential("", Configuration["V89:Password"]).Password);

                Assert.IsTrue(clientV89.Version.Major == 8);
            }

            string v9 = Configuration["V9:URL"];
            if (!string.IsNullOrWhiteSpace(v9))
            {
                clientV9 =
                    new CxClient(new Uri(v9),
                    Configuration["V9:Username"],
                    new NetworkCredential("", Configuration["V9:Password"]).Password);

                Assert.IsTrue(clientV9.Version.Major >= 9);
            }

            string v94 = Configuration["V94:URL"];
            if (!string.IsNullOrWhiteSpace(v94))
            {
                clientV94 =
                     new CxClient(new Uri(v94),
                     Configuration["V94:Username"],
                     new NetworkCredential("", Configuration["V94:Password"]).Password);
            }

        }


        [TestMethod]
        public void ODataConnectTest()
        {
            Assert.IsTrue(clientV94.Connected);
        }

        [TestMethod]
        public void ResultsWithDetectionDateTest()
        {
            foreach (SAST.OData.Project proj in clientV94.ODataV94.Projects.Take(1))
            {
                Trace.WriteLine(proj.Name);

                foreach (var scan in clientV94.ODataV94.Scans.Where(x => x.ProjectId == proj.Id).Take(3))
                {
                    Trace.WriteLine(scan.Id);

                    foreach (var result in clientV94.ODataV94.Results.Where(x => x.ScanId == scan.Id))
                    {
                        Trace.WriteLine(result.SimilarityId +  " " + result.DetectionDate.ToString());
                    }
                }
            }
        }

        
    }
}
