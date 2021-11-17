using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml.Linq;
using Checkmarx.API;

using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Checkmarx.API.CxClient;

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
        public void GetSourceCodeAndRunScanTest()
        {
            //var projectId = clientV93.GetProjects().First().Key;
            //// var scanID = clientV9.GetAllSASTScans(projectId).First().Id;

            //clientV9.RunSASTScan(projectId);

            string fileNAme = @"mysource.zip";
            File.WriteAllBytes(fileNAme, clientV9.GetSourceCode(1006182));

            Assert.IsTrue(File.Exists(fileNAme));


        }

        [TestMethod]
        public void SuggestExclusionsTest()
        {
            string extractPath = Path.GetTempFileName();

            string zipPath = Path.GetTempFileName();

            File.WriteAllBytes(zipPath, clientV9.GetSourceCode(1006182));

            // unzip
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            var exclusions = Exclusions.FromJson("exclusion.json");

            Regex[] filesRegex = exclusions.Files.Select(x => new Regex(x, RegexOptions.Compiled)).ToArray();
            Regex[] foldersRegex = exclusions.Folders.Select(x => new Regex(x, RegexOptions.Compiled)).ToArray();

            // detect exclusions
            foreach (var directoy in Directory.EnumerateDirectories(extractPath, "*.*"))
            {
                if (foldersRegex.Any(x => x.Match(directoy).Success))
                {

                }
            }

            foreach (var file in Directory.EnumerateFiles(extractPath, "*.*"))
            {
                if (filesRegex.Any(x => x.Match(file).Success))
                {

                }
            }

            // detect multilanguage
        }


        [TestMethod]
        public void ConnectionTest()
        {
            CxClient clientTEst = new CxClient(new Uri(""), "", "w");
            Assert.IsTrue(clientTEst.Connected);
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
        public void GetPresetTest()
        {
            foreach (var item in clientV89.GetPresets())
            {
                Trace.WriteLine($"{item.Key} {item.Value}");

                clientV89.GetPresetCWE(item.Value);
            }
        }

        [TestMethod]
        public void GetQueriesInformationTest()
        {
            Trace.WriteLine(clientV9.GetQueryInformation().ToString());
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
        public void ExtractCustomQueries()
        {
            string rootPath = @"D:\queries";

            StringBuilder sb = new StringBuilder();

            string version = Path.Combine(rootPath, clientV93.Version);

            var projects = clientV93.GetProjects();
            var teams = clientV93.GetTeams();

            foreach (var queryGroup in clientV93.QueryGroups)
            {
                if (queryGroup.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Cx)
                    continue;

                var packageName = queryGroup.PackageFullName.Replace(':', '\\');
                if (queryGroup.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Project)
                {
                    var match = Regex.Match(queryGroup.PackageTypeName, @"CxProject_(\d+)");
                    if (match.Success)
                    {
                        int id = int.Parse(match.Groups[1].Value);

                        var teamName = string.Empty;

                        var projectName = queryGroup.PackageTypeName;
                        if (projects.ContainsKey(id))
                        {
                            projectName = projects[id];
                            teamName = clientV93.GetProjectTeamName(id).Replace('/', '\\');
                            if (teamName.StartsWith('\\'))
                            {
                                teamName = teamName.Remove(0, 1);
                            }
                        }

                        packageName = Path.Combine(queryGroup.LanguageName, "Teams", teamName, "Projects", projectName, queryGroup.Name);
                    }
                }
                else if (queryGroup.PackageType == cxPortalWebService93.CxWSPackageTypeEnum.Team)
                {
                    string teamID = queryGroup.OwningTeam.ToString();
                    string teamPath = queryGroup.PackageTypeName;
                    if (teams.ContainsKey(teamID))
                    {
                        teamPath = teams[teamID].Replace('/', '\\');
                        if (teamPath.StartsWith('\\'))
                        {
                            teamPath = teamPath.Remove(0, 1);
                        }
                    }
                    packageName = Path.Combine(queryGroup.LanguageName, "Teams", teamPath, "Queries", queryGroup.Name);
                }

                string folder = Path.Combine(version, packageName);

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                sb.AppendLine($"+[{queryGroup.LanguageName}] {queryGroup.PackageFullName} :: {queryGroup.PackageTypeName} {queryGroup.Status}");

                foreach (var query in queryGroup.Queries)
                {
                    File.WriteAllText(Path.Combine(folder, query.Name + ".cs"), query.Source);
                }
            }

            Trace.WriteLine(sb.ToString());


        }

        [TestMethod]
        public void GetResultsForScan()
        {
            var queries = clientV93.GetQueries().SelectMany(x => x.Queries).ToDictionary(x => x.QueryId);

            StringBuilder sb = new StringBuilder();

            foreach (var result in clientV9.GetResultsForScan(1006616).GroupBy(x => x.Severity)) // 
            {
                sb.AppendLine(CxClient.toSeverityToString(result.Key));

                foreach (var severity in result.GroupBy(x => x.QueryId))
                {
                    sb.AppendLine(queries[severity.Key].Name);

                    foreach (var item in severity)
                    {
                        sb.AppendLine($"{ item.PathId } {CxClient.toResultStateToString((ResultState)item.State)}");
                    }

                }

            }

            Trace.WriteLine(sb.ToString());
        }

        [TestMethod]
        public void GetScansFromODATA()
        {
            // Force the first login.
            Trace.WriteLine(clientV89.Version);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            try
            {
                var result = clientV93.GetScansFromOData(198);
                foreach (var item in result)
                {
                    Trace.WriteLine(item.ProductVersion);
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
                var result = clientV93.GetSASTScanSummary(5);
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
        public void GetScanLogsTest()
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

        [TestMethod]
        public void TestCxUpgradeProject()
        {
            foreach (var item in clientV93.GetProjects())
            {
                var settings = clientV93.GetProjectSettings(item.Key);
                Trace.WriteLine(settings.Name + " " + settings.Owner);

                var lastscan = clientV93.GetLastScan(item.Key);

                if (lastscan != null)
                    Trace.WriteLine("\t" + lastscan.Origin + " " + lastscan.InitiatorName);
            }
        }

        [TestMethod]
        public void ListComponentConfigurationsTests()
        {
            foreach (var item in Enum.GetValues(typeof(SAST.Group)))
            {
                Trace.WriteLine("\n###" + item.ToString() + "\n");

                foreach (var configuration in clientV9.GetConfigurations((SAST.Group)item))
                {
                    Trace.WriteLine("\t" + configuration.Key + " -> " + configuration.Description + "\n\t" + configuration.Value);
                }
            }

            // Check if the Codebashing integration is done -> codebashingIsEnabled
            // Check if the action to do after the incremental is FULL instead of FAIL -> INCREMENTAL_SCAN_THRESHOLD_ACTION
            // Check if the STMP configuration is turned on -> SMTPHost
        }

        [TestMethod]
        public void GetCompareScansTest()
        {
            var results = clientV93.GetScansDiff(234, 234234);

            foreach (var item in results.GroupBy(x => x.ResultStatus))
            {

            }
        }

        [TestMethod]
        public void GetSourceCodeTest()
        {
            clientV89.GetSourceCode(2323);

            //Assert.IsTrue(clientV89.Connected);
            //Assert.IsTrue(File.Exists(""));
        }


        [TestMethod]
        public void RunScanOnTheLatestVersion()
        {
            var version = clientV93.Version;

            foreach (var project in clientV93.GetProjects())
            {
                Trace.WriteLine(project.Value);

                var lastScan = clientV93.GetLastScan(project.Key);
                if (lastScan != null)
                {
                    if (!lastScan.ScanState.CxVersion.EndsWith("HF10"))
                    {
                        clientV93.RunSASTScan(project.Key);
                    }
                }

                Trace.WriteLine(string.Empty);
            }
        }

        [TestMethod]
        public void GetFailingScansTests()
        {


            foreach (var item in clientV93.FailedScans.GroupBy(x => x.ProjectId))
            {
                Trace.WriteLine("Project " + item.First().ProjectName);

                foreach (var reasons in item.Reverse())
                {
                    string reason = reasons.Details;
                    if (reason.EndsWith('\n'))
                        reason = reasons.Details.Remove(reasons.Details.IndexOf('\n'));

                    Trace.WriteLine("\t" + reason + " with LoC : " + reasons.LOC
                        + " on " + new DateTime(reasons.CreatedOn).ToString()
                        + " started by " + reasons.Initiator);
                }
            }
        }


        [TestMethod]
        public void GetPresets()
        {
            foreach (var item in clientV89.GetPresets())
            {
                Trace.WriteLine(item.Value);
            }
        }

        [TestMethod]
        public void MyTestMethod()
        {
            //foreach (var item in clientV89.GetProjects())
            //{
            //    Trace.WriteLine(item.Value);
            //}



            foreach (var item in clientV89.GetPresets())
            {
                if (item.Value.Contains("ASA"))
                {
                    //  Trace.WriteLine(clientV89.GetPresetCWE(item.Value));
                }
            }

            //foreach (dynamic item in clientV89.QueryGroups)
            //{
            //    foreach (var query in item.Queries)
            //    {
            //        Trace.WriteLine((string)query.Source);
            //    }
            //}
        }

        [TestMethod]
        public void GetVersionWithoutConnectingTest()
        {
            Trace.WriteLine(GetVersionWithoutConnecting(@"https://localhost/"));
        }


        [TestMethod]
        public void GetNotExploitableResults()
        {
            var projects = clientV93.GetProjects();

            var date = (DateTime.Now - TimeSpan.FromDays(30)).Date;

            var last2MonthScans = clientV93.GetScansFromOData(1).Where(x => x.EngineStartedOn.Date > date);

            Dictionary<long, CxAuditWebServiceV9.CxWSResultPath> similarityIdResult =
                new Dictionary<long, CxAuditWebServiceV9.CxWSResultPath>();

            StringBuilder stringBuilder = new StringBuilder();

            var project = projects[1];

            stringBuilder.AppendLine($"<h1>{project}</h1>");

            var queries = clientV93.GetQueries().SelectMany(x => x.Queries).ToDictionary(x => x.QueryId);

            foreach (var scan in last2MonthScans)
            {
                var scanResults = clientV93.GetResultsForScan(scan.Id);
                foreach (var results in scanResults.GroupBy(x => x.Severity))
                {
                    stringBuilder.AppendLine("<h2>" + toSeverityToString(results.Key) + "</h2>");

                    foreach (var severity in results.GroupBy(x => x.QueryId))
                    {
                        stringBuilder.AppendLine("<h3>" + queries[severity.Key].Name + "</h3>");

                        foreach (var result in severity)
                        {
                            if (((ResultState)result.State) == ResultState.NonExploitable)
                            {
                                var pathhistory = clientV93.GetPathCommentsHistory(scan.Id, result.PathId);

                                var uri = Utils.GetLink(result, clientV93, 1, scan.Id);

                                stringBuilder.AppendLine($"<a href=\"{uri.AbsoluteUri}\">{uri.AbsoluteUri}</a>");

                                stringBuilder.AppendLine("<ul>");
                                foreach (var comment in result.Comment.Split(CommentSeparator, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    stringBuilder.AppendLine("<li><p>" + comment + "</p></li>");
                                }
                                stringBuilder.AppendLine("</ul>");
                            }
                        }
                    }
                }
            }

            File.WriteAllText(@"ne.html", stringBuilder.ToString());
        }

        [TestMethod]
        public void GetNotExploitableResultsAudit()
        {
            var projects = clientV93.GetProjects();

            var date = (DateTime.Now - TimeSpan.FromDays(60)).Date;

            HashSet<long> similarityIdResult =
                    new HashSet<long>();

            StringBuilder stringBuilder = new StringBuilder();


            foreach (var project in projects)
            {
                var last2MonthScans = clientV93.GetScansFromOData(project.Key).Where(x => x.EngineStartedOn.Date > date).ToArray();

                if (!last2MonthScans.SelectMany(x => clientV93.GetResult(x.Id))
                      .Where(x => x.PathPerResult.Any(y => ((ResultState)y.State) == ResultState.NonExploitable && !similarityIdResult.Contains(y.SimilarityId)))
                      .Any())
                {
                    continue;
                }

                stringBuilder.AppendLine($"<h1>{project.Value}</h1>");
                stringBuilder.AppendLine("<ul>");

                foreach (var scan in last2MonthScans)
                {
                    // stringBuilder.AppendLine($"<h2>Scan: {scan.Id}</h2>");

                    var scanResults = clientV93.GetResult(scan.Id).Where(x => x.PathPerResult.Any(y => ((ResultState)y.State) == ResultState.NonExploitable && !similarityIdResult.Contains(y.SimilarityId)));

                    if (!scanResults.Any())
                        continue;

                    stringBuilder.AppendLine("<li>");

                    foreach (var resultsBySeverity in scanResults.GroupBy(x => x.Severity))
                    {
                        stringBuilder.AppendLine("<h3>" + toSeverityToString(resultsBySeverity.Key) + "</h3>");
                        stringBuilder.AppendLine("<ul>");

                        foreach (var resultsByQuery in resultsBySeverity.GroupBy(x => x.QueryId))
                        {
                            stringBuilder.AppendLine($"<h4> { resultsByQuery.First().QueryName } [{resultsByQuery.Key}]</h4>");

                            stringBuilder.AppendLine("<li>");
                            stringBuilder.AppendLine("<ul>");

                            foreach (var result in resultsByQuery)
                            {
                                foreach (var resultPath in result.PathPerResult.Where(y => ((ResultState)y.State) == ResultState.NonExploitable))
                                {
                                    if (similarityIdResult.Contains(resultPath.SimilarityId))
                                        continue;

                                    similarityIdResult.Add(resultPath.SimilarityId);

                                    var uri = Utils.GetLink(resultPath, clientV93, project.Key, scan.Id);

                                    stringBuilder.AppendLine("<li>");
                                    stringBuilder.AppendLine($"<a href=\"{uri.AbsoluteUri}\">Result [{toResultStateToString((ResultState)resultPath.State)}]</a>");

                                    stringBuilder.AppendLine("<ul>");
                                    foreach (var comment in resultPath.Comment.Split(CommentSeparator, StringSplitOptions.RemoveEmptyEntries).Reverse())
                                    {
                                        stringBuilder.AppendLine("<li><p>" + comment + "</p></li>");
                                    }
                                    stringBuilder.AppendLine("</ul>");
                                    stringBuilder.AppendLine("</li>");

                                }
                            }

                            stringBuilder.AppendLine("</ul>");
                            stringBuilder.AppendLine("</li>");
                        }

                        stringBuilder.AppendLine("</ul>");
                    }

                    stringBuilder.AppendLine("</li>");
                }

                stringBuilder.AppendLine("</ul>");
            }

            File.WriteAllText(@"ne_allprojects.html", stringBuilder.ToString());
        }


        [TestMethod]
        public void GetODataCommentsTest()
        {
            foreach (var item in clientV93.GetODataResults(1031805).Where(x => x.PathId == 38))
            {
                var uri = Utils.GetLink(item, clientV93, item.Scan.ProjectId, item.ScanId);

                Trace.WriteLine($"{uri.AbsoluteUri}");

                Trace.WriteLine("scanid=" + item.ScanId);
                Trace.WriteLine("pathid=" + item.PathId);
                Trace.WriteLine(item.Comment);
            }
        }
    }
}