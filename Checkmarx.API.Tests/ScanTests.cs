﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Checkmarx.API;
using Checkmarx.API.Exceptions;
using Checkmarx.API.Models;
using cxPriorityWebService;
using Microsoft.Extensions.Configuration;
using Microsoft.OData.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Checkmarx.API.CxClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OData.Client;
using System.Diagnostics;

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

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            // TODO REMOVE
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<ProcessTests>();

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
        public void V97ConnectionTest()
        {
            try
            {
                // Scan
                var scan = clientV9.GetLastScan(157);
                //var scan = clientV89.GetLastScan(157);

                // Queries
                var scanQueries = clientV9.GetQueriesForScan(scan.Id).GroupBy(x => x.Severity).ToDictionary(x => x.Key, y => y.Count());
                var scanQueriesCritical = scanQueries.ContainsKey((int)CxDataRepository97.Severity.Critical) ? scanQueries[(int)CxDataRepository97.Severity.Critical] : 0;
                var scanQueriesHigh = scanQueries.ContainsKey((int)CxDataRepository97.Severity.High) ? scanQueries[(int)CxDataRepository97.Severity.High] : 0;
                var scanQueriesMedium = scanQueries.ContainsKey((int)CxDataRepository97.Severity.Medium) ? scanQueries[(int)CxDataRepository97.Severity.Medium] : 0;
                var scanQueriesLow = scanQueries.ContainsKey((int)CxDataRepository97.Severity.Low) ? scanQueries[(int)CxDataRepository97.Severity.Low] : 0;
                var scanQueriesInfo = scanQueries.ContainsKey((int)CxDataRepository97.Severity.Info) ? scanQueries[(int)CxDataRepository97.Severity.Info] : 0;
                Trace.WriteLine($"Detected Project Queries: Criticals: {scanQueriesCritical} | Highs: {scanQueriesHigh} | Mediums: {scanQueriesMedium} | Lows: {scanQueriesLow} | Info: {scanQueriesInfo}");

                Trace.WriteLine("");

                // SOAP
                var soapResults = clientV9.GetResultsForScan(scan.Id);
                var soapQueryCounters = clientV9.GetODataScanResultsQuerySeverityCounters(scan.Id);

                var soapCriticals = soapResults.Count(x => x.Severity == (int)CxDataRepository97.Severity.Critical && x.State != (int)ResultState.NonExploitable);
                var soapHigh = soapResults.Count(x => x.Severity == (int)CxDataRepository97.Severity.High && x.State != (int)ResultState.NonExploitable);
                var soapMedium = soapResults.Count(x => x.Severity == (int)CxDataRepository97.Severity.Medium && x.State != (int)ResultState.NonExploitable);
                var soapLow = soapResults.Count(x => x.Severity == (int)CxDataRepository97.Severity.Low && x.State != (int)ResultState.NonExploitable);
                var soapInfo = soapResults.Count(x => x.Severity == (int)CxDataRepository97.Severity.Info && x.State != (int)ResultState.NonExploitable);
                Trace.WriteLine($"SOAP Results - Criticals: {soapCriticals} | Highs: {soapHigh} | Mediums: {soapMedium} | Lows: {soapLow} | Info: {soapInfo}");
                Trace.WriteLine($"SOAP Query Counters - Criticals: {soapQueryCounters[CxDataRepository97.Severity.Critical]} | Highs: {soapQueryCounters[CxDataRepository97.Severity.High]} | Mediums: {soapQueryCounters[CxDataRepository97.Severity.Medium]} | Lows: {soapQueryCounters[CxDataRepository97.Severity.Low]}");

                Trace.WriteLine("");

                // Odata
                var oDataScanResults = clientV9.GetODataResults(scan.Id).ToList();
                var odataQueryCounters = clientV9.GetScanResultsQuerySeverityCounters(scan.Id);

                var criticals = oDataScanResults.Count(x => x.Severity == CxDataRepository97.Severity.Critical && x.StateId != (int)ResultState.NonExploitable);
                var high = oDataScanResults.Count(x => x.Severity == CxDataRepository97.Severity.High && x.StateId != (int)ResultState.NonExploitable);
                var medium = oDataScanResults.Count(x => x.Severity == CxDataRepository97.Severity.Medium && x.StateId != (int)ResultState.NonExploitable);
                var low = oDataScanResults.Count(x => x.Severity == CxDataRepository97.Severity.Low && x.StateId != (int)ResultState.NonExploitable);
                var info = oDataScanResults.Count(x => x.Severity == CxDataRepository97.Severity.Info && x.StateId != (int)ResultState.NonExploitable);

                Trace.WriteLine($"OData Scan - Criticals: {scan.Results.Critical} | Highs: {scan.Results.High} | Mediums: {scan.Results.Medium} | Lows: {scan.Results.Low} | Info: {scan.Results.Info}");
                Trace.WriteLine($"OData Results - Criticals: {criticals} | Highs: {high} | Mediums: {medium} | Lows: {low} | Info: {info}");
                Trace.WriteLine($"OData Query Counters - Criticals: {odataQueryCounters[CxDataRepository97.Severity.Critical]} | Highs: {odataQueryCounters[CxDataRepository97.Severity.High]} | Mediums: {odataQueryCounters[CxDataRepository97.Severity.Medium]} | Lows: {odataQueryCounters[CxDataRepository97.Severity.Low]}");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        [TestMethod]
        public void MarkResultsTest()
        {
            long projId = 2740;
            long scanId = 1024036;
            string comment = "Rolling back again";

            //var resultStateList = clientV89.GetResultStateList();
            // 0, "To Verify"
            // 1, "Not Exploitable"
            // 2, "Confirmed"
            // 3, "Urgent"
            // 4, "Proposed Not Exploitable"

            string resultState = "0";

            var oDataScanResults = clientV89.GetODataResults(1024036);
            var results = new List<PortalSoap.ResultStateData>();
            try
            {
                foreach (var result in oDataScanResults)
                {
                    results.Add(new PortalSoap.ResultStateData
                    {
                        projectId = projId,
                        PathId = result.PathId,
                        Remarks = comment,
                        scanId = scanId,
                        data = resultState,
                        ResultLabelType = (int)PortalSoap.ResultLabelTypeEnum.State
                    });
                }

                if (results.Any())
                {
                    Trace.WriteLine($"Updating {results.Count} for scan {scanId}");

                    clientV89.UpdateSetOfResultStateAndComment(results.ToArray(), comment);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, $"Error updating result states for scan {scanId}");
            }
        }

        [TestMethod]
        public void CompareScanResultsTest()
        {
            var compareScan = clientV89.GetCompareScanResultsAsync(1003127, 1009757).Results.ToList();

            var previousScanNEResultList = clientV89.GetNotExploitableFromScan(1003127).ToList();
            var scanNEResultList = clientV89.GetNotExploitableFromScan(1009757).ToList();

            var results = clientV89.GetResultsForScanByState(1009757, ResultState.NonExploitable);
            var previousResults = clientV89.GetResultsForScanByState(1003127, ResultState.NonExploitable);

            //// < 9.5
            //var results1 = clientV9.GetResultsForScan(1661644).ToList();
            //var results2 = clientV9.GetODataResults(1661644).ToList();

            // >= 9.5
            //var results3 = clientV9.GetResultsForScan(1000122).ToList();
            //var results4 = clientV9.GetODataResults(1731996).Where(x => x.QueryId != null && x.State != null && x.StateId != 1).ToList();
            //var results5 = clientV9.GetODataV95Results(1000122).ToList();

            //var test1 = results3.FirstOrDefault();
            //var test2 = results4.Where(x => x.PathId == test1.PathId);

            //var results4 = clientV9.GetResult(1011900).ToList();

            //var scan = clientV9.GetScanById(1011900);
            //var results = scan.Results;

            //var results5 = clientV9.GetSASTResults(1011900);

            //Trace.WriteLine($"Results: {results4}");
        }

        [TestMethod]
        public void TriggerScanTest()
        {
            var scandId = clientV9.RunSASTScan(17735, presetId: 100000, configurationId: 1);
        }

        [TestMethod]
        public void DeleteScanTest()
        {
            clientV9.DeleteScan(1894824);
        }

        [TestMethod]
        public void ReadScanLogsTest()
        {
            var scanId = 1000013;

            var coverage = clientV9.GetScanCoverage(scanId);
            var langsTmp = clientV9.GetScannedLanguages(scanId);
        }

        [TestMethod]
        public void GetQueryTimeTest()
        {
            var scanId = 1951776;

            var keyValuePairs = clientV93.GetQueriesRuntimeDuration(scanId);

            foreach (var language in keyValuePairs)
            {
                Trace.WriteLine(language.Key.ToString());
                foreach (var query in language.Value)
                    Trace.WriteLine("\t" + query.Key.ToString() + " took " + query.Value);
            }
        }

        [TestMethod]
        public void GetProjectConfigurationTest()
        {
            var projectSettings = clientV93.GetProjectConfiguration(16524);
        }

        [TestMethod]
        public void LockScanTest()
        {
            clientV89.LockScan(1020859);
        }

        [TestMethod]
        public void UnlockScanTest()
        {
            clientV89.UnlockScan(1020859);
        }

        [TestMethod]
        public void GetLastScanResultsTest()
        {
            // var scans = clientV93.GetScans(18123, true).ToList();
            var lastScan = clientV9.GetLastScan(36829, onlyPublic: false);
            if (lastScan != null)
            {
                var normalHigh = lastScan.Results.High;
                var normalMedium = lastScan.Results.Medium;
                var normalLow = lastScan.Results.Low;

                var results = clientV9.GetResultsForScan(lastScan.Id);
                //var results = clientV9.GetScanResultsWithStateExclusions(lastScan.Id, new List<long>() { 8, 9 });
                var countHigh = results.Where(x => x.Severity == (int)Severity.High).Count();
                var countMedium = results.Where(x => x.Severity == (int)Severity.Medium).Count();
                var countLow = results.Where(x => x.Severity == (int)Severity.Low).Count();
            }
        }


        [TestMethod]
        public void ListSCansTEst()
        {
            Trace.WriteLine(clientV93.GetLastScan(18123).Id);
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
                    var oDataScanResults = clientV9.GetODataResults(lastScan.Id).ToList();
                    if (oDataScanResults.Any(x => x.StateId == 5 && x.DetectionDate >= new DateTime(2023, 6, 6)))
                        Trace.WriteLine($"Project {proj.Key}");
                }
            }
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

            // ODATA
            Stopwatch odataV9ResultsStopwatch = new Stopwatch();
            odataV9ResultsStopwatch.Start();
            var results2 = clientV95.GetODataResults(1000122).ToList();
            odataV9ResultsStopwatch.Stop();
            Trace.WriteLine($"Time elapsed getting scan ODATA V9 results: {odataV9ResultsStopwatch.Elapsed}");
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

            var test = clientV9.GetProjectCustomFields(21927);

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

                                var uri = result.GetLink(clientV93, 1, scan.Id);

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

                                    var uri = Checkmarx.API.Utils.GetLink(resultPath, clientV93, project.Key, scan.Id);

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
                var uri = Checkmarx.API.Utils.GetLink(item, clientV93, item.Scan.ProjectId.Value, item.ScanId.Value);

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

        [TestMethod]
        public void GetLastScanTest()
        {
            long projectId = 26555;

            //var project = clientV9.GetProjects().FirstOrDefault(x => x.Key == projectId);

            bool hasScanRunning = clientV9.ProjectHasScanRunning(projectId);

            var scans = clientV9.GetScans(projectId, true).ToList();
            var scansNotFinished = clientV9.GetScans(projectId, false).ToList();

            var calculatedLastScan = scans.OrderByDescending(x => x.DateAndTime.EngineStartedOn).FirstOrDefault();
            var calculatedLastScanNotFinished = scansNotFinished.OrderByDescending(x => x.DateAndTime.EngineStartedOn).FirstOrDefault();

            var lastScan = clientV9.GetLastScan(projectId);
            var lastScanNotFinished = clientV9.GetLastScan(projectId, finished: false);

            Trace.WriteLine(lastScan.Id);
        }

        [TestMethod]
        public void ListScanQueueTest()
        {
            foreach (var scan in clientV93.GetScansQueue())
            {
                Trace.WriteLine(scan.Project.Id + " - " + scan.Id.ToString());

                var scanDef = clientV93.GetScanById(scan.Id);

                Trace.WriteLine($"Scan {scanDef.Id} - {scanDef.Status.Name}");

            }

            Trace.WriteLine("Failed Scans:");

            foreach (var scan in clientV93.GetFailingScans())
            {
                Trace.WriteLine($"Scan {scan.Id}");

            }


        }

        #region Write Tests


        public void ReRunScanWithPresetTest()
        {
            clientV93.RunSASTScan(12132, presetId: 100000);
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

        long ghostScan = 2020992;
        long unfinishedScan = 2022041;
        long incrementalScan = 2022063;

        [TestMethod]
        public void GetGhostScanTest()
        {
            var scan = clientV93.GetScanById(ghostScan);

            if (scan.DateAndTime.EngineStartedOn == null)
            {
                Trace.WriteLine(scan.Id);
            }

            var scanOData = clientV93.ODataV95.Scans.Where(x => x.Id == ghostScan).Single();

            Assert.IsNotNull(scanOData.EngineStartedOn);
        }

        [TestMethod]
        public void GetAllGhostScansFromProject()
        {
            foreach (var item in clientV9.ODataV95.Scans.Where(x => x.ProjectId == 38097 && x.ScanType == 1 && x.EngineFinishedOn == null))
            {
                Trace.WriteLine(item.Comment);
            }
        }

        [TestMethod]
        public void GetScansExcludingGhostScansTest()
        {
            var scans = clientV9.GetScans(38097, true, includeGhostScans: false);

            foreach (var scan in scans)
            {
                Trace.WriteLine($"{scan.Id} - ScanType {scan.ScanType.Id} and EngineFinishedOn {scan.DateAndTime.EngineFinishedOn}");
            }
        }

        [TestMethod]
        public void GetScanResultsGroupedByPresetIdTest()
        {
            var res = clientV9.GetResultsForScan(18263);

            var resByQueryId = res.GroupBy(x => x.QueryId).ToDictionary(x => x.Key, y => y.Count());
            var resByPresetId = res.GroupBy(x => clientV9.GetPresetQueryId(x.QueryVersionCode)).ToDictionary(x => x.Key, y => y.Count());
        }

        [TestMethod]
        public void GetProjectsTest()
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var newProjects = fetchProjectsViaOData().Result;

            Trace.WriteLine("ODATA Paging: " + newProjects.Count + " took " + watch.Elapsed.TotalSeconds.ToString());

            watch.Restart();

            var projDetails = clientV93.GetAllProjectsDetails()
                     .Where(x => x.TeamId != "-1")
                     .ToList();

            Trace.WriteLine("REST: " + projDetails.Count + " took " + watch.Elapsed.TotalSeconds.ToString());

            watch.Restart();

            List<SAST.OData.Project> oldQueryProjects = clientV93.ODataV95.Projects
                     .Expand(x => x.LastScan)
                     .Expand(y => y.LastScan.ScannedLanguages)
                     .Expand(x => x.Preset)
                     .Expand(x => x.CustomFields)
                     .Where(y => y.OwningTeamId != -1)
                     .ToList();

            Trace.WriteLine("ODATA Single Request: " + oldQueryProjects.Count + " took " + watch.Elapsed.TotalSeconds.ToString());

            var newIds = newProjects.Select(x => x.Id).ToHashSet();

            List<long> newProjectsIDs = new List<long>();
            foreach (var item in newProjects)
            {
                if (!newProjectsIDs.Contains(item.Id))
                    newProjectsIDs.Add(item.Id);
                else
                    Trace.Write("Duplicated:" + item.Id);
            }

            foreach (var item in oldQueryProjects.Where(x => !newIds.Contains(x.Id)))
            {
                Trace.Write(item.Id);
            }

            Assert.AreEqual(projDetails.Count, oldQueryProjects.Count, "The REST and the OData call should return the same number of projects");

            Assert.AreEqual(oldQueryProjects.Count, newProjects.Count, "The OData with and without paging should return the same number of projects");
        }

        private ICollection<API.SAST.OData.Project> getProjectsViaOData()
        {
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

            var allProjects = new List<API.SAST.OData.Project>();
            int pageSize = 500;
            int skip = 0;
            bool hasMoreResults = true;

            while (hasMoreResults)
            {

#if DEBUG
                watch.Restart();
                try
                {
#endif

                    var projectsQuery = clientV93.ODataV95.Projects
                                    .Expand(x => x.LastScan)
                                    .Expand(y => y.LastScan.ScannedLanguages)
                                    .Expand(x => x.Preset)
                                    .Expand(x => x.CustomFields)
                                    .Where(y => y.OwningTeamId != -1)
                                    .OrderBy(p => p.Id) // Ensure consistent ordering across pages
                                    .Skip(skip)
                                    .Take(pageSize);

                    var projects = projectsQuery.ToList();

                    allProjects.AddRange(projects);

                    Trace.WriteLine($"Fetched {projects.Count} projects. Total: {allProjects.Count}");

                    if (projects.Count < pageSize)
                    {
                        hasMoreResults = false;
                    }
                    else
                    {
                        skip += pageSize;
                    }
#if DEBUG
                }
                finally
                {
                    watch.Stop();
                    Trace.WriteLine("Fetch Time-ms: " + watch.Elapsed.TotalSeconds.ToString());
                }
#endif
            }

            return allProjects;
        }


        private static readonly int MaxRetries = 10;
        private static readonly int MinPageSize = 10;

        public static async Task<ICollection<API.SAST.OData.Project>> fetchProjectsViaOData()
        {
            var allProjects = new List<SAST.OData.Project>();
            int currentPageSize = 10000;
            int skip = 0;
            bool hasMoreResults = true;
            int attempt = 0;

            while (hasMoreResults)
            {
                try
                {
                    var projects = await ExecuteQuery(skip, currentPageSize);

                    if (projects.Count == 0)
                    {
                        hasMoreResults = false;
                    }
                    else
                    {
                        attempt = 0;

                        allProjects.AddRange(projects);
                        skip += projects.Count;
                        Trace.WriteLine($"Successfully fetched {projects.Count} projects. Total: {allProjects.Count}");

                        if (projects.Count < currentPageSize)
                            hasMoreResults = false;

                        // Optionally increase page size on success
                        // currentPageSize = Math.Min(currentPageSize * 2, InitialPageSize);
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error occurred: {ex.Message}");

                    attempt++;
                    if (attempt == MaxRetries)
                    {
                        throw; // Rethrow if all retries failed
                    }

                    // Reduce page size on failure
                    currentPageSize = Math.Max(currentPageSize / 2, MinPageSize);
                    Trace.WriteLine($"Reduced page size to {currentPageSize}");

                    // Delay the next request
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                    Trace.WriteLine($"Attempt {attempt} failed. Retrying in {delay.TotalSeconds} seconds...");
                    Thread.Sleep(delay);
                }
            }

            return allProjects;
        }


        private static Task<List<SAST.OData.Project>> ExecuteQuery(int skip, int pageSize)
        {
            return Task.Run(() => clientV93.ODataV95.Projects
                .Expand(x => x.LastScan)
                .Expand(y => y.LastScan.ScannedLanguages)
                .Expand(x => x.Preset)
                .Expand(x => x.CustomFields)
                .Where(y => y.OwningTeamId != -1)
                // .OrderBy(p => p.Id) // Ensure consistent ordering
                .Skip(skip)
                .Take(pageSize)
                .ToList());
        }

        [TestMethod]
        public void ConflictIdInODataQueryTest()
        {
            for (int i = 0; i < 10; i++)
            {
                List<SAST.OData.Project> projects = clientV93.ODataV95.Projects
                         .Expand(x => x.LastScan)
                         .Expand(y => y.LastScan.ScannedLanguages)
                         .Expand(x => x.Preset)
                         .Expand(x => x.CustomFields)
                         .Where(y => y.IsPublic && y.OwningTeamId != -1)
                         .ToList();

                Trace.WriteLine(i + " - " + projects.Count);

                Dictionary<long, SAST.OData.Project> result = [];
                foreach (var x in projects)
                {
                    if (!result.ContainsKey(x.Id))
                        result.Add(x.Id, x);
                    else
                        Trace.WriteLine(x.Id);
                }
            }
        }

        [TestMethod]
        public void GetXSSResultsTest()
        {
            var results = clientV93.GetResultsForScan(2013975); // SOAP ...
            Trace.WriteLine("Count: " + results.Count());
            foreach (var result in results)
                Trace.WriteLine(result.QueryId + ";" + result.QueryVersionCode);

            var odataResults = clientV93.GetODataResults(2013975); // OData
            Trace.WriteLine("ODATA Count: " + odataResults.Count());
            foreach (var odataResult in odataResults)
                Trace.WriteLine(odataResult.QueryId + ";" + odataResult.QueryVersionId + ";" + odataResult.Query.Name);

            Assert.AreEqual(odataResults.Count(), results.Count(), "The results from OData and SOAP should be equal");
        }


        [TestMethod]
        public void PriorityAPIGetScanResultsTest()
        {
            try
            {
                var resultProperties = typeof(SoapSingleResultData).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);

                Trace.WriteLine("Property values using PortalSoap");
                var results = clientV9.GetResultsForScan(1705892); // Portal SOAP 
                foreach (var result in results)
                {
                    foreach (var property in resultProperties)
                        Trace.WriteLine(property.Name + ": " + property.GetValue(result));
                }

                //Trace.WriteLine("");
                //Trace.WriteLine("Property values using Priority");
                //var resultsPriority = clientV9.GetResultsForScan(1705892, usePriority: true); // Priority
                //foreach (var result in resultsPriority)
                //{
                //    foreach (var property in resultProperties)
                //        Trace.WriteLine(property.Name + ": " + property.GetValue(result));
                //}
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        [TestMethod]
        public void CompareSoapVsOdataScanResultsTest()
        {
            var projects = clientV93.GetAllProjectsDetails();
            foreach (var project in projects)
            {
                try
                {
                    var scanInfo = clientV93.GetLastScan(project.Id);
                    if (scanInfo != null)
                    {
                        var odataResults = clientV93.GetODataResults(scanInfo.Id); // OData
                        var results = clientV93.GetResultsForScan(scanInfo.Id); // SOAP
                        if (odataResults.Count() != results.Count())
                        {
                            var odataQueryIds = odataResults.Where(x => x.QueryId.HasValue).Select(x => x.QueryId.Value).Distinct();
                            var queryIds = results.Select(x => x.QueryId).Distinct();

                            // Find queries in odata that are not in soap
                            var difference1 = odataQueryIds.Except(queryIds).ToList();

                            // Find queries in soap that are not in odata
                            var difference2 = queryIds.Except(odataQueryIds).ToList();

                            // Total diff
                            var totalDiff = difference1.Union(difference2);

                            Trace.WriteLine($"Project {project.Id} - Scan {scanInfo.Id} - Query Difference: {string.Join(";", totalDiff)}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error for project {project.Id}: {ex.Message}");
                }
            }
        }

        [TestMethod]
        public void FetchScansByDateTest()
        {
            var filterDate = new DateTime(2024, 6, 23, 0, 3, 0);

            var scanBefore = clientV93.GetScans(18096, true, scanKind: CxClient.ScanRetrieveKind.Last, maxScanDate: filterDate, includeGhostScans: false).SingleOrDefault();

            Trace.WriteLine($"Scan Before: {scanBefore.Id}");

            var scanAfter = clientV93.GetScans(18096, true, scanKind: CxClient.ScanRetrieveKind.First, minScanDate: filterDate, includeGhostScans: false).SingleOrDefault();

            Trace.WriteLine($"Scan After: {scanAfter.Id}");
        }




    }
}
