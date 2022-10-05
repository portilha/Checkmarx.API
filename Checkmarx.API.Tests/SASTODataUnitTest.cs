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


        private static CxClient clientV94;

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            // TODO REMOVE
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<SASTODataUnitTest>();

            Configuration = builder.Build();


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
                        Trace.WriteLine(result.SimilarityId + " " + result.DetectionDate.ToString());
                    }
                }
            }
        }

        [TestMethod]
        public void ProjectTest()
        {
            foreach (var proj in clientV94.ODataV94.Projects.Expand(x => x.CustomFields))
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
            foreach (var proj in clientV94.ODataV94.Projects.Expand("LastScan($expand=Results($filter=Severity%20eq%20CxDataRepository.Severity%27High%27))&$top=1&$skip=0"))
            {
                Trace.WriteLine(proj.Name + " " + proj.LastScan.Id);
            }
        }

        [TestMethod]
        public void ScansTest()
        {
            foreach (var scan in clientV94.ODataV94.Scans.Take(40))
            {
                Trace.WriteLine(scan.ProjectId + " " + scan.Id + " " + scan.PresetName);
            }
        }


        [TestMethod]
        public void ProjectCountTest()
        {
            Assert.IsTrue(clientV94.ODataV94.Projects.Count() > 0);
            Assert.IsTrue(clientV94.ODataV94.Scans.Count() > 0);
            Assert.IsTrue(clientV94.ODataV94.Results.Count() > 0);

        }


    }
}