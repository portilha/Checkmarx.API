using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Xml.Linq;
using Checkmarx.API;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Checkmarx.API.Tests
{
    [TestClass]
    public class CxClientUnitTests
    {
        public static IConfigurationRoot Configuration { get; private set; }

        private static CxClient clientV89;
        private static CxClient clientV9;
        private static CxClient clientV93;

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            // TODO REMOVE
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
                var version = clientV89.Version;
            }

            string v9 = Configuration["V9:URL"];
            if (!string.IsNullOrWhiteSpace(v9))
            {
                clientV9 =
                    new CxClient(new Uri(v9),
                    Configuration["V9:Username"],
                    new NetworkCredential("", Configuration["V9:Password"]).Password);
                var _ = clientV9.Version;
            }

            string v93 = Configuration["V93:URL"];
            if (!string.IsNullOrWhiteSpace(v93))
            {
                clientV93 =
                     new CxClient(new Uri(v93),
                     Configuration["V93:Username"],
                     new NetworkCredential("", Configuration["V93:Password"]).Password);
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

                    Console.WriteLine(JsonConvert.SerializeObject(result));
                }

                Console.WriteLine("== OSA Results ==");
                foreach (var osaScan in clientV89.GetOSAScansIds(item.Key))
                {
                    var osaResults = clientV89.GetOSAResults(osaScan);
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
            if (projects.Count() != 0)
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




            foreach (var result in clientV93.GetResultsForScan(1001263))
            {
                Trace.WriteLine(result.Comment);
            }

            //Assert.IsTrue(sut.Length != 0);
            //}
        }

        [TestMethod]
        public void GetScansFromODATA()
        {
            // Force the first login.
            Trace.WriteLine(clientV89.Version);

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            try
            {
                var result = clientV9.GetScansFromOData(5).Where(x => !x.IsLocked);

                foreach (var item in result)
                {
                    Trace.WriteLine(item.Id);
                }

                Assert.IsNotNull(result);
            }
            finally
            {
                watch.Stop();
                Console.WriteLine("name Time-ms: " + watch.Elapsed.TotalMilliseconds.ToString());
            }

            watch.Restart();

            try
            {
                var result = clientV89.GetSASTScanSummary(5);

                foreach (var item in result)
                {
                    Trace.WriteLine(item.Id);
                }

                Assert.IsNotNull(result);
            }
            finally
            {
                watch.Stop();
                Console.WriteLine("name Time-ms: " + watch.Elapsed.TotalMilliseconds.ToString());
            }
        }

        [TestMethod]
        public void RetrieveSingleScan()
        {
            var scan = clientV9.GetScanById(1003042);
            Assert.IsNotNull(scan);
        }

        [TestMethod]
        public void GEtScanLogs()
        {
            clientV89.GetScanLogs(1010075);
        }

        [TestMethod]
        public void GetScanCount()
        {
            Console.WriteLine(clientV9.GetScanCount());
        }

        [TestMethod]
        public void GetCWEDescription()
        {

            foreach (var queryGroup in clientV89.GetQueries())
            {
                foreach (var query in queryGroup.Queries)
                {

                    if (query.Cwe != 0)
                        Trace.WriteLine(clientV89.GetCWEDescription(query.Cwe));
                }
            }
        }

        [TestMethod]
        public void GetAllProjectLastScan()
        {
            var result = clientV9.GetProjectsWithLastScan();

            Trace.WriteLine(result.Count());

            foreach (var item in result.ToArray())
            {
                Trace.WriteLine($"{item.Id} {item.Name} - {item.LastScan?.Id} - {item.LastScan?.ScanCompletedOn}");
            }

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public void GetLastScanDateTest()
        {
            var lastScan = clientV93.GetLastScan(22);
            Assert.IsNotNull(lastScan.DateAndTime.EngineStartedOn);
        }

        [TestMethod]
        public void GetScanLanguagesTest()
        {
            foreach (var projectId in clientV9.GetProjects().Keys)
            {
                var scan = clientV9.GetAllSASTScans(projectId);

                foreach (var item in scan)
                {
                    Trace.WriteLine(string.Join(";", item.ScanState?.LanguageStateCollection));
                } 
            }

        }   

        const string DATE_FORMAT = "yyyy-MM-dd";

        [TestMethod]
        public void GetEHCTest()
        {
            DateTime startDateTime = DateTime.Now;
            DateTime endDateTime = DateTime.Now - TimeSpan.FromDays(9);

            var result = clientV89.GetRequest<JObject>($"/cxwebinterface/odata/v1/Scans?$select=Id,ProjectName,OwningTeamId,TeamName,ProductVersion,EngineServerId,Origin,PresetName,ScanRequestedOn,QueuedOn,EngineStartedOn,EngineFinishedOn,ScanCompletedOn,ScanDuration,FileCount,LOC,FailedLOC,TotalVulnerabilities,High,Medium,Low,Info,IsIncremental,IsLocked,IsPublic&$expand=ScannedLanguages($select=LanguageName)&$filter=ScanRequestedOn gt {startDateTime.ToString(DATE_FORMAT)}Z and ScanRequestedOn lt {endDateTime.ToString(DATE_FORMAT)}z");

            Assert.IsNotNull(result);

            result = clientV9.GetRequest<JObject>($"/cxwebinterface/odata/v1/Scans?$select=Id,ProjectName,OwningTeamId,TeamName,ProductVersion,EngineServerId,Origin,PresetName,ScanRequestedOn,QueuedOn,EngineStartedOn,EngineFinishedOn,ScanCompletedOn,ScanDuration,FileCount,LOC,FailedLOC,TotalVulnerabilities,High,Medium,Low,Info,IsIncremental,IsLocked,IsPublic&$expand=ScannedLanguages($select=LanguageName)&$filter=ScanRequestedOn gt {startDateTime.ToString(DATE_FORMAT)}Z and ScanRequestedOn lt {endDateTime.ToString(DATE_FORMAT)}z");

            var json = JsonConvert.SerializeObject(result);

            // string filePath = "";
            Trace.Write(json);

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void GetLicenseInfoTest()
        {
            var info = clientV93.GetLicense();

            Trace.WriteLine($"Checkmarx Version: {clientV93.Version}");
            Trace.WriteLine($"Projects: {info.CurrentProjectsCount}/{info.ProjectsAllowed}");
            Trace.WriteLine($"Users: {info.CurrentUsers}/{info.MaxUsers}");
            Trace.WriteLine($"License: {DateTime.Parse(info.ExpirationDate).ToString()}");

            Assert.IsNotNull(info);


        }

    }
}