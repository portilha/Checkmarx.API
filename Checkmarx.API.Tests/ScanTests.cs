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
using System.Xml;
using System.Xml.Linq;
using Checkmarx.API;
using Checkmarx.API.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.OData.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Checkmarx.API.CxClient;

namespace Checkmarx.API.Tests
{
    [TestClass]
    public class ScanTests
    {
        public static IConfigurationRoot Configuration { get; private set; }

        private static CxClient clientV89;
        private static CxClient clientV9;
        private static CxClient clientV93;
        private static CxClient clientV95;

        private static string QueryDescription = "PT temporary query";

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            // TODO REMOVE
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<EngProceduresTests>();

            Configuration = builder.Build();

            string v8 = Configuration["V89:URL"];

            if (!string.IsNullOrWhiteSpace(v8))
            {
                clientV89 =
                        new CxClient(new Uri(v8),
                        Configuration["V89:Username"],
                        new NetworkCredential("", Configuration["V89:Password"]).Password);

                //Assert.IsTrue(clientV89.Version.Major == 8);
            }

            string v9 = Configuration["V9:URL"];
            if (!string.IsNullOrWhiteSpace(v9))
            {
                //var builderuri = new UriBuilder(v9);
                //var uri = builderuri.Uri;

                clientV9 =
                    new CxClient(new Uri(v9),
                    Configuration["V9:Username"],
                    new NetworkCredential("", Configuration["V9:Password"]).Password);

                //Assert.IsTrue(clientV9.Version.Major >= 9);
            }

            string v93 = Configuration["V93:URL"];
            if (!string.IsNullOrWhiteSpace(v93))
            {
                clientV93 =
                     new CxClient(new Uri(v93),
                     Configuration["V93:Username"],
                     new NetworkCredential("", Configuration["V93:Password"]).Password);
            }

            string v95 = Configuration["V95:URL"];
            if (!string.IsNullOrWhiteSpace(v95))
            {
                clientV95 =
                     new CxClient(new Uri(v95),
                     Configuration["V95:Username"],
                     new NetworkCredential("", Configuration["V95:Password"]).Password);
            }
        }

        [TestMethod]
        public void ReadScanLogsTest()
        {
            try
            {
                var scanId = 1000013;

                var logsScanZip = clientV9.GetScanLogs(scanId);

                string tempDirectory = Path.GetTempPath();
                string logPath = Path.Combine(tempDirectory, $"{scanId}");

                if (Directory.Exists(logPath))
                    Directory.Delete(logPath, true);

                var zipPath = Path.Combine(logPath, $"{scanId}.zip");

                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);

                File.WriteAllBytes(zipPath, logsScanZip);

                ZipFile.ExtractToDirectory(zipPath, logPath);

                ZipFile.ExtractToDirectory(Path.Combine(logPath, $"Scan_{scanId}.zip"), logPath);

                string logFilePath = Directory.GetFiles(logPath, "*.log").First();

                // Read Log
                double firstFinalScanAccuracy = 0;
                List<string> firstFinalScanLanguages = new List<string>();

                string log = File.ReadAllText(logFilePath);
                Regex regex = new Regex("^Scan\\scoverage:\\s+(?<pc>[\\d\\.]+)\\%", RegexOptions.Multiline);
                MatchCollection mc = regex.Matches(log);
                foreach (Match m in mc)
                {
                    GroupCollection groups = m.Groups;
                    double.TryParse(groups["pc"].Value.Replace(".", ","), out firstFinalScanAccuracy);
                }

                //Languages that will be scanned: Java=3, CPP=1, JavaScript=1, Groovy=6, Kotlin=361
                Regex regexLang = new Regex("^Languages\\sthat\\swill\\sbe\\sscanned:\\s+(?:(\\w+)\\=\\d+\\,?\\s?)+", RegexOptions.Multiline);
                MatchCollection mcLang = regexLang.Matches(log);
                var langsTmp = new List<string>();
                foreach (Match m in mcLang)
                {
                    System.Text.RegularExpressions.GroupCollection groups = m.Groups;
                    foreach (System.Text.RegularExpressions.Group g in groups)
                    {
                        foreach (Capture c in g.Captures)
                        {
                            if (c.Value != "" && !c.Value.StartsWith("Languages that will be scanned:"))
                            {
                                langsTmp.Add(c.Value);
                            }
                        }
                    }
                }

                if (langsTmp.Count > 0)
                {
                    firstFinalScanLanguages = langsTmp;
                }

            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error: {ex.Message}");
            }
        }

        [TestMethod]
        public void GetProjectConfigurationTest()
        {
            var projId = 16524;
            var projectSettings = clientV93.GetProjectConfiguration(projId);
        }

        [TestMethod]
        public void GetLastScanResultsTest()
        {
            // var scans = clientV93.GetScans(18123, true).ToList();
            var lastScan = clientV93.GetLastScan(18123, true);

            Assert.IsTrue(lastScan.Id == 5498);

            //if (lastScan != null)
            //{
            //    var results = clientV93.GetODataResults(lastScan.Id);
            //    var test = results.Where(x => x.Severity == CxDataRepository.Severity.High).ToList();


            //    var toVerify = results.Where(x => x.StateId == 0).Count();
            //    var toVerify2 = clientV93.GetTotalToVerifyFromScan(lastScan.Id);

            //    Trace.WriteLine($"ScanId: {lastScan.Id} | High: {lastScan.Results.High} | Medium: {lastScan.Results.Medium} | Low: {lastScan.Results.Low} | Info: {lastScan.Results.Info} | ToVerify: {toVerify}");
            //}
        }


        [TestMethod]
        public void ListSCansTEst()
        {
            //foreach (var scan in )
            //{
            Trace.WriteLine(clientV93.GetLastScan(18123).Id);
            //}

        }

        [TestMethod]
        public void GetScansTest()
        {
            var firstScan = clientV9.GetFirstScan(17683);
            var lastScan = clientV9.GetLastScan(17683);
            var lastFullScan = clientV9.GetLastScan(17683, true);
            var lastScanFinishOrFailed = clientV9.GetLastScanFinishOrFailed(17683);
            var lockedScan = clientV9.GetLockedScan(17683);
        }

        [TestMethod]
        public void CheckResultsIgnoreStatusTest()
        {
            var projects = clientV9.GetProjects();
            foreach (var proj in projects)
            {
                var lastScan = clientV9.GetLastScan(proj.Key);
                if (lastScan != null)
                {
                    var oDataScanResults = clientV9.GetODataV95Results(lastScan.Id).ToList();
                    if (oDataScanResults.Any(x => x.StateId == 5 && x.DetectionDate >= new DateTime(2023, 6, 6)))
                        Trace.WriteLine($"Project {proj.Key}");
                }
            }
        }

        [TestMethod]
        public void GetVulnerabilitiesFromScanTest()
        {
            //// < 9.5
            //var results1 = clientV9.GetResultsForScan(1661644).ToList();
            //var results2 = clientV9.GetODataResults(1661644).ToList();

            // >= 9.5
            //var results3 = clientV9.GetResultsForScan(1000122).ToList();
            var results4 = clientV9.GetODataResults(1731996).Where(x => x.QueryId != null && x.State != null && x.StateId != 1).ToList();
            //var results5 = clientV9.GetODataV95Results(1000122).ToList();

            //var test1 = results3.FirstOrDefault();
            //var test2 = results4.Where(x => x.PathId == test1.PathId);

            //var results4 = clientV9.GetResult(1011900).ToList();

            //var scan = clientV9.GetScanById(1011900);
            //var results = scan.Results;

            //var results5 = clientV9.GetSASTResults(1011900);

            Trace.WriteLine($"Results: {results4}");
        }

        [TestMethod]
        public void GetVulnerabilitiesFromScanSpeedTest()
        {
            // SOAP
            Stopwatch soapResultsStopwatch = new Stopwatch();
            soapResultsStopwatch.Start();
            var results1 = clientV95.GetResultsForScan(1000122).ToList();
            soapResultsStopwatch.Stop();
            Trace.WriteLine($"Time elapsed getting scan soap results: {soapResultsStopwatch.Elapsed}");

            // ODATA V9
            Stopwatch odataV9ResultsStopwatch = new Stopwatch();
            odataV9ResultsStopwatch.Start();
            var results2 = clientV95.GetODataResults(1000122).ToList();
            odataV9ResultsStopwatch.Stop();
            Trace.WriteLine($"Time elapsed getting scan ODATA V9 results: {odataV9ResultsStopwatch.Elapsed}");

            // ODATA V95
            Stopwatch odataV95ResultsStopwatch = new Stopwatch();
            odataV95ResultsStopwatch.Start();
            var results3 = clientV95.GetODataV95Results(1000122).ToList();
            odataV95ResultsStopwatch.Stop();
            Trace.WriteLine($"Time elapsed getting scan ODATA V95 results: {odataV95ResultsStopwatch.Elapsed}");
        }

        [TestMethod]
        public void GEtLastSCanDate()
        {
            var lastScan = clientV9.GetLastScan(3142);

            //lastScan.IsIncremental

            Trace.WriteLine(lastScan?.DateAndTime?.EngineStartedOn);
        }

        [TestMethod]
        public void GetSourceCodeFromScanTest()
        {
            //var projectId = clientV93.GetProjects().First().Key;
            //// var scanID = clientV9.GetAllSASTScans(projectId).First().Id;

            //clientV9.RunSASTScan(projectId);

            string fileNAme = @"mysource.zip";

            File.WriteAllBytes(fileNAme, clientV9.GetSourceCode(1006182));

            Assert.IsTrue(File.Exists(fileNAme));
        }

        [TestMethod]
        public void GetResultsForScan()
        {
            // var projects = clientV93.GetProjects();

            var csvFields = new List<object>();

            long scanId = 1004407;

            var resultStateList = clientV93.GetResultStateList();

            var stateList = resultStateList.ToArray();

            // ask just once.
            var queryGroups = clientV93.GetQueries().ToArray();

            var queries = queryGroups.SelectMany(x => x.Queries).ToDictionary(x => x.QueryId);

            Dictionary<long, cxPortalWebService93.CxWSQueryGroup> queryToQueryGroup = new Dictionary<long, cxPortalWebService93.CxWSQueryGroup>();
            foreach (var queryGroup in queryGroups)
            {
                foreach (var query in queryGroup.Queries)
                {
                    queryToQueryGroup.Add(query.QueryId, queryGroup);
                }
            }

            var headers = new List<string>(new[] { "CxVersion", "Language", "Severity", "Query Id", "Query Name", });
            headers.AddRange(stateList.Select(x => x.Value));
            headers.AddRange(new[] { "New", "Fixed", "Reoccured" });
            headers.Add("Package");

            StringBuilder sb = new StringBuilder("sep=;\n" + string.Join(";", headers.Select(x => $"\"{x}\"")) + "\n");

            var scanInfo = clientV93.GetScanById(scanId);

            foreach (var result in clientV93.GetResultsForScan(scanId).GroupBy(x => x.Severity)) // 
            {
                foreach (var severity in result.GroupBy(x => x.QueryId))
                {
                    csvFields = new List<object>();

                    var query = queries[severity.Key];

                    csvFields.Add(scanInfo.ScanState.CxVersion);

                    csvFields.Add(queryToQueryGroup[query.QueryId].LanguageName);
                    csvFields.Add(CxClient.toSeverityToString(result.Key));

                    csvFields.Add(query.QueryId);
                    csvFields.Add(query.Name);

                    var resultByState = severity.GroupBy(x => x.State).ToDictionary(x => x.Key);

                    foreach (var item in stateList)
                    {
                        csvFields.Add(!resultByState.ContainsKey((int)item.Key) ? 0 : resultByState[(int)item.Key].Count());
                    }

                    //     sb.AppendLine($"\t\t{CxClient.toResultStateToString((ResultState)state.Key)} - {state.Count()}");

                    // New, Fixed, Recorrence
                    var resultByType = severity.GroupBy(x => x.ResultStatus).ToDictionary(x => x.Key);

                    csvFields.Add(!resultByType.ContainsKey(PortalSoap.CompareStatusType.New) ? 0 : resultByType[PortalSoap.CompareStatusType.New].Count());
                    csvFields.Add(!resultByType.ContainsKey(PortalSoap.CompareStatusType.Fixed) ? 0 : resultByType[PortalSoap.CompareStatusType.Fixed].Count());
                    csvFields.Add(!resultByType.ContainsKey(PortalSoap.CompareStatusType.Reoccured) ? 0 : resultByType[PortalSoap.CompareStatusType.Reoccured].Count());

                    csvFields.Add(query.Status.ToString());

                    sb.AppendLine(string.Join(";", csvFields.Select(x => $"\"{x}\"")));
                }

            }

            File.WriteAllText($"C:\\test\\{scanInfo.Project.Name}_{scanInfo.Id}.csv", sb.ToString());
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
        public void GetLastScanDateTest()
        {
            var lastScan = clientV93.GetLastScan(22);
            Assert.IsNotNull(lastScan.DateAndTime.EngineStartedOn);
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
                      .Where(x => x.PathPerResult.Any(y => ((ResultState)y.State) == ResultState.NonExploitable &&
                      !similarityIdResult.Contains(y.SimilarityId)))
                      .Any())
                {
                    continue;
                }

                stringBuilder.AppendLine($"<h1>{project.Value}</h1>");
                stringBuilder.AppendLine("<ul>");

                foreach (var scan in last2MonthScans)
                {
                    // stringBuilder.AppendLine($"<h2>Scan: {scan.Id}</h2>");

                    var scanResults = clientV93.GetResult(scan.Id)
                        .Where(x => x.PathPerResult.Any(y => ((ResultState)y.State) == ResultState.NonExploitable && !similarityIdResult.Contains(y.SimilarityId)));

                    if (!scanResults.Any())
                        continue;

                    stringBuilder.AppendLine("<li>");

                    foreach (var resultsBySeverity in scanResults.GroupBy(x => x.Severity))
                    {
                        stringBuilder.AppendLine("<h3>" + toSeverityToString(resultsBySeverity.Key) + "</h3>");
                        stringBuilder.AppendLine("<ul>");

                        foreach (var resultsByQuery in resultsBySeverity.GroupBy(x => x.QueryId))
                        {
                            stringBuilder.AppendLine($"<h4> {resultsByQuery.First().QueryName} [{resultsByQuery.Key}]</h4>");

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

        [TestMethod]
        public void GetFailingScansTest()
        {
            var results = clientV93.GetFailingScans();

            foreach (var item in results.GroupBy(x => x.Id))
            {
                Console.WriteLine($"# Failing Scan Id: {item.Key}");

                foreach (var failedScan in item)
                {
                    Console.WriteLine(failedScan.Details);
                }
            }
        }

        [TestMethod]
        public void ScanODataTest()
        {
            Trace.WriteLine(clientV93.ODataV95.Scans.Count());

            foreach (var item in clientV93.ODataV95.Scans.Where(x => x.Id > 1003598))
            {
                Trace.WriteLine(item.Id);
            }
        }

        #region Write Tests

        public void ReRunScanWithPresetTest()
        {
            clientV93.RunSASTScan(12132, presetId: 100000);

            //var projects = clientV93.GetProjects();
            //if (projects.Any())
            //{
            //    //var project = projects.Where(x => x.Key == 12132).FirstOrDefault().Key;
            //    //int? presetId = 1000;

            //    clientV93.RunSASTScan(12132, presetId: 100000);
            //}

            //foreach (var item in clientV93.GetPresets())
            //{
            //    Trace.WriteLine($"{item.Key} {item.Value}");
            //}
        }

        public void UpdateResultStateTest()
        {
            long projectId = 1127;
            long scanId = 1038685;
            long pathId = 27;

            // Get current comments
            var currentComments = clientV89.GetAllCommentRemarksForScanAndPath(scanId, pathId);

            // Update result state
            clientV89.UpdateResultState(projectId, scanId, pathId, ResultState.ToVerify);

            // Get current comments
            var updatedComments = clientV89.GetAllCommentRemarksForScanAndPath(scanId, pathId);
        }

        public void AddResultCommentTest()
        {
            long projectId = 1127;
            long scanId = 1038685;
            long pathId = 27;

            // Get current comments
            var currentComments = clientV89.GetAllCommentRemarksForScanAndPath(scanId, pathId);

            // Add result comment
            clientV89.AddResultComment(projectId, scanId, pathId, "Added new comment");

            // Get current comments
            var updatedComments = clientV89.GetAllCommentRemarksForScanAndPath(scanId, pathId);
        }

        #endregion
    }
}
