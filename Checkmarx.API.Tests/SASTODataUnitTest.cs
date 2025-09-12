using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Checkmarx.API.Utils;

namespace Checkmarx.API.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class SASTODataUnitTest
    {

        public static IConfigurationRoot Configuration { get; private set; }


        private static CxClient clientV95;

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            // TODO REMOVE
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<SASTODataUnitTest>();

            Configuration = builder.Build();


            string v94 = Configuration["V95:URL"];
            if (!string.IsNullOrWhiteSpace(v94))
            {
                clientV95 =
                     new CxClient(new Uri(v94),
                     Configuration["V95:Username"],
                     new NetworkCredential("", Configuration["95:Password"]).Password);
            }
        }


        [TestMethod]
        public void ODataConnectTest()
        {
            Assert.IsTrue(clientV95.Connected);
        }

        [TestMethod]
        public void ResultsWithDetectionDateTest()
        {
            foreach (SAST.OData.Project proj in clientV95.ODataV95.Projects.Take(1))
            {
                Trace.WriteLine(proj.Name);

                foreach (var scan in clientV95.ODataV95.Scans.Where(x => x.ProjectId == proj.Id).Take(3))
                {
                    Trace.WriteLine(scan.Id);

                    foreach (var result in clientV95.ODataV95.Results.Where(x => x.ScanId == scan.Id))
                    {
                        Trace.WriteLine(result.SimilarityId + " " + result.DetectionDate.ToString());
                    }
                }
            }
        }

        [TestMethod]
        public void ProjectTest()
        {
            foreach (var proj in clientV95.ODataV95.Projects.Expand(x => x.CustomFields))
            {
                Trace.WriteLine(proj.Name + " " + proj.IsPublic);

                foreach (var cf in proj.CustomFields)
                {
                    Trace.WriteLine("\t" + cf.FieldName + " = " + cf.FieldValue);
                }
            }
        }

        /// <summary>
        /// https://checkmarx.atlassian.net/wiki/spaces/KC/pages/1374388434/CxSAST+OData+API+Overview+Examples
        /// </summary>
        [TestMethod]
        public void ProjectsExpandQueryTest()
        {
            foreach (var proj in clientV95.ODataV95.Projects.Expand("LastScan($expand=Results())&$top=1&$skip=0"))
            {
                Assert.IsNotNull(proj.LastScan);

                Trace.WriteLine(proj.Name + " " + proj.LastScan.Id);

                foreach (var result in proj.LastScan.Results)
                {
                    Trace.WriteLine("\t" + result.GetLink(clientV95, proj.Id, proj.LastScan.Id).AbsolutePath + " " + result.SimilarityId + " " + result.DetectionDate);
                }
            }
        }

        [TestMethod]
        public void ResultScanTest()
        {
            foreach (var result in clientV95.ODataV95.Results.Expand(x => x.Scan).OrderByDescending(x => x.DetectionDate).Take(100))
            {

                Trace.WriteLine("\t" + result.GetLink(clientV95, result.Scan.ProjectId.Value, result.Scan.Id).AbsoluteUri + " " + result.SimilarityId + " " + result.DetectionDate);

            }
        }

        [TestMethod]
        public void ScansTest()
        {
            foreach (var scan in clientV95.ODataV95.Scans.Take(40))
            {
                Trace.WriteLine(scan.ProjectId + " " + scan.Id + " " + scan.PresetName);
            }
        }

        [TestMethod]
        public void ProjectCountTest()
        {
            Assert.IsTrue(clientV95.ODataV95.Projects.Count() > 0);
            Assert.IsTrue(clientV95.ODataV95.Scans.Count() > 0);
            Assert.IsTrue(clientV95.ODataV95.Results.Count() > 0);
        }
    }
}