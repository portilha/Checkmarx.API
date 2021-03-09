using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using Checkmarx.API;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Checkmarx.API.Tests
{
    [TestClass]
    public class CxClientUnitTests
    {
        public static IConfigurationRoot Configuration { get; private set; }

        private static CxClient clientV89;
        private static CxClient clientV9;


        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<CxClientUnitTests>();

            Configuration = builder.Build();

            string v8 = Configuration["V89:URL"];

            if (!string.IsNullOrWhiteSpace(v8))
            {
                clientV89 =
                        new CxClient(new Uri(v8),
                        Configuration["V89:Username"],
                        new NetworkCredential("", Configuration["V89:Password"]).Password); 
            }

            string v9 = Configuration["V9:URL"];
            if (!string.IsNullOrWhiteSpace(v9))
            {
                clientV9 =
                    new CxClient(new Uri(v9),
                    Configuration["V9:Username"],
                    new NetworkCredential("", Configuration["V9:Password"]).Password);
            }
        }

        [TestMethod]
        public void TestGetVersion()
        {
            Assert.AreEqual("V 9.0", clientV9.Version);
        }

        [TestMethod]
        public void TestGetOSA_V9()
        {

            var license = clientV9.GetLicense();

            Assert.IsTrue(license.IsOsaEnabled);
        }


        [TestMethod]
        public void TestGetOSA_V8()
        {

            var license = clientV89.GetLicense();

            Assert.IsTrue(license.IsOsaEnabled);
        }

        [TestMethod]
        public void V9ConnectTest()
        {

            foreach (var item in clientV9.GetProjects())
            {
                Trace.WriteLine(item.Key);
            }
        }

        [TestMethod]
        public void GetXMLReport()
        {
            // var memoryStream = client.GetScanReport(1010026, ReportType.XML);

            var memoryStream = @"csr-internal.xml";

            XDocument document = XDocument.Load(memoryStream);

            var queries = from item in document.Root.Elements("Query")
                          select new
                          {
                              CWE = int.Parse(item.Attribute("cweId").Value),
                              Name = item.Attribute("name").Value,
                              Language = item.Attribute("Language").Value,
                              Severity = item.Attribute("Severity").Value,
                              Results = item.Elements("Result")
                          };

            foreach (var query in queries)
            {
                var results = from result in query.Results
                              select new
                              {
                                  Id = long.Parse(result.Attribute("NodeId").Value),
                                  SimilarityId = result.Elements("Path").Single().Attribute("SimilarityId").Value,
                                  IsNew = result.Attribute("Status").Value == "New",
                                  State = result.Attribute("state").Value,
                                  Link = result.Attribute("DeepLink").Value,
                                  TruePositive = result.Attribute("FalsePositive").Value != "False",
                                  Comments = HttpUtility.HtmlDecode(result.Attribute("Remark").Value),
                                  AssignTo = result.Attribute("AssignToUser").Value,
                                  Nodes = result.Elements("Path").Single().Elements()
                              };

                Trace.WriteLine($"Query: {query.Language} {query.Name} - {query.Severity} CWE:{query.CWE} - {query.Results.Count()}");

                foreach (var result in results.Where(x => x.TruePositive))
                {
                    Trace.WriteLine($"\n\tResult: {result.Id} - Confirmed: {result.TruePositive} - {result.Nodes.Count()}\r\n{result.Comments}\n");

                    var nodes = from node in result.Nodes
                                select new
                                {
                                    FileName = node.Element("FileName").Value,
                                    Snippet = HttpUtility.HtmlDecode(node.Element("Snippet").Element("Code").Value),
                                };
                }
            }
        }

        [TestMethod]
        public void getRTFReport()
        {
            var memoryStream = clientV89.GetScanReport(1010026, ReportType.RTF);
            string outputFile = @"report.rtf";
            using (FileStream fs = File.Create(outputFile))
            {
                memoryStream.CopyTo(fs);
            }
        }

        [TestMethod]
        public void getPDFReport()
        {
            var memoryStream = clientV89.GetScanReport(1010026, ReportType.PDF);
            using (FileStream fs = File.Create(@"report.pdf"))
            {
                memoryStream.CopyTo(fs);
            }
        }

        [TestMethod]
        public void GetProjectsTest()
        {
            foreach (var keyValuePair in clientV89.GetTeams())
            {
                Console.WriteLine(keyValuePair.Value);
            }

            foreach (var item in clientV89.GetProjects())
            {
                Console.WriteLine($" === {item.Value} === ");

                var excluded = clientV89.GetExcludedSettings(item.Key);
                Console.WriteLine("ExcludedFolders:" + excluded.Item1);
                Console.WriteLine("ExcludedFiles:" + excluded.Item2);
                Console.WriteLine("Preset:" + clientV89.GetSASTPreset(item.Key));

                Console.WriteLine("== CxSAST Scans ==");
                foreach (var sastScan in clientV89.GetSASTScans(item.Key))
                {
                    var result = clientV89.GetSASTResults(sastScan);
                    //var scanState = sastScan["scanState"];

                    //result.LoC = (int)scanState.SelectToken("linesOfCode");
                    //result.FailedLoC = (int)scanState.SelectToken("failedLinesOfCode");
                    //result.LanguagesDetected = ((JArray)scanState["languageStateCollection"]).Select(x => x["languageName"].ToString()).ToList();

                    Console.WriteLine(JsonConvert.SerializeObject(result));
                }

                Console.WriteLine("== OSA Results ==");
                foreach (var osaScanUI in clientV89.GetOSAScans(item.Key))
                {
                    var osaResults = clientV89.GetOSAResults(osaScanUI);
                    Console.WriteLine(osaResults.ToString());
                }
                Console.WriteLine(" === == === == ");
            }
        }


        [TestMethod]
        public void GetPreset()
        {
            var presets = clientV89.GetPresets();

            foreach (var item in clientV89.GetProjects())
            {
                Trace.WriteLine($"{item.Key} " + clientV89.GetSASTPreset(item.Key));
            }


        }

        [TestMethod]
        public void SetScanSettings()
        {
            foreach (var project in clientV89.GetProjects())
            {
                clientV89.SetProjectSettings(project.Key, 36,
                    1, 1);
            }
        }

        [TestMethod]
        public void GetProjectSettings()
        {
            clientV89.GetProjectSettings(9);
        }

        [TestMethod]
        public void SetCustomFieldsTest()
        {
            var projectSettings = clientV89.GetProjectSettings(20);

            clientV89.SetCustomFields(projectSettings, new[] {
                new CustomField
                {
                    Id = 3,
                    Value = "Onboarded"
                }
            });
        }

        [TestMethod]
        public void TestCreationDate()
        {
            clientV89.GetProjectCreationDate(9);

        }
        [TestMethod]
        public void TestCreationDateV9()
        {
            clientV9.GetProjectCreationDate(9);
        }

        [TestMethod]
        public void GetProjectDetails()
        {
            foreach (var item in clientV89.GetAllProjectsDetails())
            {
                foreach (var customField in item.CustomFields)
                {

                }
            }

        }

        [TestMethod]
        public void GetScansDisplayData()
        {
            var projects = clientV9.GetProjects();
            if (projects.Count != 0)
            {
                var sut = clientV9.GetScansDisplayData(projects.Keys.First());
                Assert.IsTrue(sut.IsSuccesfull);
            }
        }

        [TestMethod]
        public void GetResultsForScan()
        {
            //var projects = clientV9.GetProjects();
            //if (projects.Count != 0)
            //{
            // clientV89.GetCommentsHistoryTest(1010075);

            //foreach (var result in clientV9.GetResultsForScan(1002369))
            //{
            //    Trace.WriteLine(result.Comment);
            //}


            CxClient clientV93 = 
                        new CxClient(new Uri(Configuration["V93:URL"]),
                        Configuration["V93:Username"],
                        new NetworkCredential("", Configuration["V93:Password"]).Password);

            foreach (var result in clientV93.GetResultsForScan(1001263))
            {
                Trace.WriteLine(result.Comment);
            }

            //Assert.IsTrue(sut.Length != 0);
            //}
        }


        [TestMethod]
        public void GetResultsTest()
        {
            long scanID = 1010075;

            //    .GetScanResults(scanID)
            //foreach (var groupOfResults in clientV89
            //    .GroupBy(x => x.QueryGroupName))
            //{
            //    Trace.WriteLine(groupOfResults.Key);

            //    foreach (var item in groupOfResults)
            //    {
            //        // Trace.WriteLine(item.)
            //    }
            //}
        }

        [TestMethod]
        public void GetScanLogs()
        {
            var result = clientV9.GetScanLogs(1003204);

            Assert.IsTrue(result);
        }
    }
}
