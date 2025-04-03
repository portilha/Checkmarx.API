using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using Checkmarx.API.Exceptions;
using Checkmarx.API.Models;
using Checkmarx.API.Tests.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.OData.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        private static CxClient clientV95;

        const string DATE_FORMAT = "yyyy-MM-dd";

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
        public void RetryPolicyTest()
        {
            RetryPolicyProvider retryPolicyProvider = new RetryPolicyProvider(2);

            var mockResponse = new Mock<HttpWebResponse>();
            mockResponse.Setup(r => r.StatusCode).Returns(HttpStatusCode.InternalServerError); // Change to HttpStatusCode.RequestTimeout for 408
            //mockResponse.Setup(r => r.StatusCode).Returns(HttpStatusCode.RequestTimeout);
            //mockResponse.Setup(r => r.StatusCode).Returns(HttpStatusCode.BadRequest);

            var webException = new WebException(
                "Test exception",
                null,
                WebExceptionStatus.ProtocolError,
                mockResponse.Object
            );

            Assert.ThrowsException<WebException>(() =>
                retryPolicyProvider.ExecuteWithRetry(() =>
                {
                    throw webException;
                    return 0;
                })
            );
        }

        [TestMethod]
        public void ODataRetryableTest()
        {
            try
            {
                var projects1 = clientV9.ODataV95.Projects.Expand(x => x.Preset)
                                                      .Expand(x => x.CustomFields)
                                                      .Where(y => y.IsPublic && y.OwningTeamId != -1)
                                                      .ToList();

                DateTime date = new DateTime(2024, 2, 2);

                Checkmarx.API.SAST.Scan scanBeforeChange = clientV9.GetScans(4, true, scanKind: CxClient.ScanRetrieveKind.Last, maxScanDate: date, includeGhostScans: false).SingleOrDefault();
            }
            catch (DataServiceQueryException ex)
            {
                Trace.WriteLine($"Status Code: {ex.Response.StatusCode}");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Status Code: {ex.Message}");
            }
        }

        [TestMethod]
        public void ProjectExclusionsTest()
        {
            var projects = clientV93.ODataV95.Projects.Expand(x => x.Preset)
                                                      .Expand(x => x.CustomFields)
                                                      .Where(y => y.IsPublic && y.OwningTeamId != -1)
                                                      .ToList();
            int inError = 0;
            foreach (var project in projects)
            {
                try
                {
                    var exclusions = clientV93.GetExcludedSettings(project.Id);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error for project {project.Id}. Reason: {ex.Message}");
                    inError++;
                }
            }

            Trace.WriteLine($"Error fetching project exclusions for {inError} projects in {projects.Count()}");
        }

        [TestMethod]
        public void GetAuthenticationBearTokenTest()
        {
            Trace.WriteLine(clientV93.AuthenticationToken);
        }

        [TestMethod]
        public void Connectv93Test()
        {
            int attempts = 100;
            int failAttempts = 0;
            for (int i = 0; i < attempts; i++)
            {
                try
                {
                    string v93 = Configuration["V93:URL"];
                    if (!string.IsNullOrWhiteSpace(v93))
                    {
                        clientV93 =
                             new CxClient(new Uri(v93),
                             Configuration["V93:Username"],
                             new NetworkCredential("", Configuration["V93:Password"]).Password);
                    }
                    //Thread.Sleep(TimeSpan.FromSeconds(1));
                    var test = clientV93.Connected;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"{i} Fail connection. Reason: {ex.Message}");
                    failAttempts++;
                }
            }

            Trace.WriteLine($"{failAttempts} fail attempts in {attempts}");
        }

        [TestMethod]
        public void OdataSpeedTest()
        {
            //var scanResults = clientV9.GetODataResults(1731996).Where(x => x.QueryId != null && x.State != null && x.StateId != 1).ToList();
            var scanResults = clientV9.GetODataResults(1607268).Where(x => x.QueryId != null && x.State != null && x.StateId != 1).ToList();

            var scanResultsHigh = scanResults.Where(x => x.Severity == CxDataRepository97.Severity.High);
            var scanResultsMedium = scanResults.Where(x => x.Severity == CxDataRepository97.Severity.Medium);
            var scanResultsLow = scanResults.Where(x => x.Severity == CxDataRepository97.Severity.Low);

            var InitialQueriesHigh = scanResultsHigh.Select(x => x.QueryId).Distinct().Count();
            var InitialQueriesMedium = scanResultsMedium.Select(x => x.QueryId).Distinct().Count();
            var InitialQueriesLow = scanResultsLow.Select(x => x.QueryId).Distinct().Count();

            Trace.WriteLine($"High: {InitialQueriesHigh} | Medium: {InitialQueriesMedium} | Low: {InitialQueriesLow}");
        }

        [TestMethod]
        public void OdataSpeedTest3()
        {
            var severityCounters = clientV89.GetODataScanResultsQuerySeverityCounters(1003321);

            var InitialQueriesHigh = severityCounters[CxDataRepository97.Severity.High];
            var InitialQueriesMedium = severityCounters[CxDataRepository97.Severity.Medium];
            var InitialQueriesLow = severityCounters[CxDataRepository97.Severity.Low];

            Trace.WriteLine($"High: {InitialQueriesHigh} | Medium: {InitialQueriesMedium} | Low: {InitialQueriesLow}");
        }

        [TestMethod]
        public void OdataSpeedTest4()
        {
            var severityCounters = clientV89.GetScanResultsQuerySeverityCounters(1003321);

            var InitialQueriesHigh = severityCounters[CxDataRepository97.Severity.High];
            var InitialQueriesMedium = severityCounters[CxDataRepository97.Severity.Medium];
            var InitialQueriesLow = severityCounters[CxDataRepository97.Severity.Low];

            Trace.WriteLine($"High: {InitialQueriesHigh} | Medium: {InitialQueriesMedium} | Low: {InitialQueriesLow}");
        }

        [TestMethod]
        public void QueriesTest()
        {
            // Test1
            string language = "java";
            string queryName = "sql_injection";

            List<long> queriesIds = new List<long>();
            var foundQueries = clientV9.GetQueriesByLanguageAndOrName(language, queryName);
            foreach (var querie in foundQueries)
                queriesIds.Add(querie.Key.QueryId);

            // Test2
            string language2 = null;
            string queryName2 = "sql_injection";

            List<long> queriesIds2 = new List<long>();
            var foundQueries2 = clientV9.GetQueriesByLanguageAndOrName(language2, queryName2);
            foreach (var querie in foundQueries2)
                queriesIds2.Add(querie.Key.QueryId);
        }

        [TestMethod]
        public void CertificateTest()
        {
            var severityCounters = clientV89.GetAllProjectsDetails().ToList();
        }

        [TestMethod]
        public void CheckProjectCustomFieldsTest()
        {
            var customFields = clientV93.GetProjectCustomFields(81);

            foreach (var item in customFields)
                Trace.WriteLine($"{item.Key} : {item.Value.Value}");
        }

        [TestMethod]
        public void GetConfigurationLstTEst()
        {
            foreach (var item in clientV93.PortalSOAP.GetConfigurationSetListAsync(null).Result.ConfigSetList)
            {
                Trace.WriteLine(item.ConfigSetName);
            }
        }

        [TestMethod]
        public void SuggestExclusionsTest()
        {
            string extractPath = Path.GetTempFileName();

            string zipPath = Path.GetTempFileName();

            File.WriteAllBytes(zipPath, clientV9.GetSourceCode(1006182));

            // unzip
            ZipFile.ExtractToDirectory(zipPath, extractPath);

            var exclusions = Exclusions.FromJson(TestUtils.ReadEmbeddedFile("exclusions.json"));

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
            Assert.AreEqual("V 9.0", clientV9.Version.ToString());
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
        public void GetSourceCodeSettingsTest()
        {

            foreach (var project in clientV93.GetProjects())
            {
                var projectConfig = clientV93.GetProjectConfigurations(project.Key);

                switch (projectConfig.SourceCodeSettings.SourceOrigin)
                {
                    case PortalSoap.SourceLocationType.Local:
                        break;
                    case PortalSoap.SourceLocationType.Shared:
                        break;
                    case PortalSoap.SourceLocationType.SourceControl:
                        {
                            switch (projectConfig.SourceCodeSettings.SourceControlSetting.Repository)
                            {
                                case PortalSoap.RepositoryType.TFS:
                                    break;
                                case PortalSoap.RepositoryType.SVN:
                                    break;
                                case PortalSoap.RepositoryType.CVS:
                                    break;
                                case PortalSoap.RepositoryType.GIT:
                                    {
                                        Trace.WriteLine($"Project {project.Key} {project.Value}");

                                        GetGitSourceSettingsDto projectConfigSAST = clientV93.SASTClient.GitSourceSettings_GetGitSettingsByidAsync(project.Key).Result;

                                        Trace.WriteLine("SOAP: ");

                                        Trace.WriteLine("Repo:" + projectConfig.SourceCodeSettings.SourceControlSetting.ServerName);
                                        Trace.WriteLine("Branch:" + projectConfig.SourceCodeSettings.SourceControlSetting.GITBranch);
                                        Trace.WriteLine("User:" + projectConfig.SourceCodeSettings.SourceControlSetting.UserCredentials?.User);

                                        if (projectConfig.SourceCodeSettings.SourceControlSetting.UseSSH)
                                        {
                                            Trace.WriteLine("SSH:" + projectConfig.SourceCodeSettings.SourceControlSetting.SSHPublicKey);
                                        }

                                        Trace.WriteLine("REST: ");

                                        Trace.WriteLine("Repo:" + projectConfigSAST.Url);
                                        Trace.WriteLine("Branch:" + projectConfigSAST.Branch);
                                        Trace.WriteLine("SSH:" + projectConfigSAST.UseSsh);
                                    }
                                    break;
                                case PortalSoap.RepositoryType.Perforce:
                                    break;
                                case PortalSoap.RepositoryType.NONE:
                                    break;
                                default:
                                    break;
                            }
                        }

                        break;
                    case PortalSoap.SourceLocationType.SourcePulling:
                        break;
                    default:
                        break;
                }
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
        public void GetProjects2Test()
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
            foreach (var item in clientV93.GetPresets())
            {
                Trace.WriteLine($"{item.Key} {item.Value}");

                //clientV93.GetPresetCWE(item.Value);
            }
        }

        [TestMethod]
        public void GetQueriesInformationTest()
        {
            Trace.WriteLine(clientV9.GetQueryInformation().ToString());
        }

        [TestMethod]
        public void GetProjectSettings()
        {
            clientV89.GetProjectSettings(9);
        }

        [TestMethod]
        public void GetCustomFieldsTest()
        {
            var cfs = clientV93.GetSASTCustomFields();
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

            string version = Path.Combine(rootPath, clientV93.Version.ToString());

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
        public void ResultStateTest()
        {
            foreach (var item in clientV93.PortalSOAP.GetResultStateListAsync(null).Result.ResultStateList)
            {
                Trace.WriteLine(item.ResultID + " -> " + item.ResultName);
            }
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
        public void GetCxUpgradeProjectOriginTest()
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
        public void CheckBranchesTest()
        {
            var projectDetails = clientV9.GetAllProjectsDetails();

            foreach (var item in projectDetails.Where(x => x.IsBranched.HasValue && x.IsBranched.Value))
            {
                Trace.Write(item.Name + " -> " + item.OriginalProjectId);
            }
        }

        [TestMethod]
        public void CheckAPISupportTest()
        {
            var versions = new string[] { "0.1", "1", "1.1", "1.2", "1.3", "2", "2.1", "2.2", "3" };

            Trace.WriteLine($"> {clientV89.Version}");

            foreach (var version in versions)
            {
                Trace.WriteLine($"{version} - {clientV89.SupportsRESTAPIVersion(version)}");
            }

            Trace.WriteLine($"> {clientV9.Version}");
            foreach (var version in versions)
            {
                Trace.WriteLine($"{version} - {clientV9.SupportsRESTAPIVersion(version)}");
            }

            Trace.WriteLine($"> {clientV93.Version}");
            foreach (var version in versions)
            {
                Trace.WriteLine($"{version} - {clientV93.SupportsRESTAPIVersion(version)}");
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
        public void GetVersionWithoutConnectingTest()
        {
            Trace.WriteLine(GetVersionWithoutConnecting(@"https://localhost/"));
        }


        [TestMethod]
        public void GetLastScanTest()
        {
            var scan = clientV93.GetLastScan(39049, true, true);



            Assert.IsNotNull(scan);
        }


        #region Write Tests

        public void DisableUserTest()
        {
            try
            {
                string email = "bruno.vilela@checkmarx.com";

                clientV9.DisableUserByEmail(email);
                Trace.WriteLine($"User {email} disabled.");
            }
            catch (UserNotFoundException ex)
            {
                Trace.WriteLine($"Error disabling user. Reason: {ex.Message}.");
            }
            catch
            {
                throw;
            }
        }

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

        public void UploadPresetTest()
        {
            cxPortalWebService93.CxPresetDetails preset = clientV93.GetPresetDetails(2);

            var newPreset = new cxPortalWebService93.CxPresetDetails()
            {
                id = preset.id

            };

            clientV93.PortalSOAP.UpdatePresetAsync(null, preset);
        }

        public void CreateProjectTEst()
        {
            clientV9.SASTClient.ProjectsManagement_PostByprojectAsync(new SaveProjectDto
            {
                IsPublic = true,
                Name = "ProjectName",
                OwningTeam = "34"
            });
        }

        public void CreateBranchTest()
        {
            clientV9.SASTClient.BranchProjects_BranchByidprojectAsync(123, new BranchProjectDto
            {
                Name = "New Branch Name"
            }).Wait();

        }

        #endregion
    }
}
