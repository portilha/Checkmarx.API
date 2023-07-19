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
        private static CxClient clientV95;

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
        public void UploadQueryTest()
        {
            var projects = clientV9.GetProjects().ToList();
            var project = projects.FirstOrDefault(x => x.Key == 1);

            var queryFile = "D:\\Users\\bruno.vilela\\OneDrive - Checkmarx\\Documents\\query.txt";
            string query = File.ReadAllText(queryFile);
        }

        [TestMethod]
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

        [TestMethod]
        public void CheckResultsIgnoreStatusTest()
        {
            var projects = clientV9.GetProjects();
            foreach(var proj in projects)
            {
                var lastScan = clientV9.GetLastScan(proj.Key);
                if(lastScan != null)
                {
                    var oDataScanResults = clientV9.GetODataV95Results(lastScan.Id).ToList();
                    if(oDataScanResults.Any(x => x.StateId == 5 && x.DetectionDate >= new DateTime(2023, 6, 6)))
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

        [TestMethod]
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

        [TestMethod]
        public void GetLastScanResultsTest()
        {
            var scans = clientV9.GetScans(18122, true).ToList();
            var lastScan = clientV9.GetLastScan(18122, true);
            if (lastScan != null)
            {
                var results = clientV9.GetODataResults(lastScan.Id);
                var test = results.Where(x => x.Severity == CxDataRepository.Severity.High).ToList();


                var toVerify = results.Where(x => x.StateId == 0).Count();
                var toVerify2 = clientV9.GetTotalToVerifyFromScan(lastScan.Id);

                Trace.WriteLine($"ScanId: {lastScan.Id} | High: {lastScan.Results.High} | Medium: {lastScan.Results.Medium} | Low: {lastScan.Results.Low} | Info: {lastScan.Results.Info} | ToVerify: {toVerify}");
            }
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
        public void GEtLastSCanDate()
        {
            var lastScan = clientV9.GetLastScan(3142);

            //lastScan.IsIncremental

            Trace.WriteLine(lastScan?.DateAndTime?.EngineStartedOn);
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
            foreach (var item in clientV93.GetProjects())
            {
                Trace.WriteLine(item.Key);
            }

            //var projects = clientV9.GetProjects();
            //var projById = projects.Where(x => x.Key == 23484);
            //var projById2 = projects.Where(x => x.Key == 18730);
            //var projByName = projects.Where(x => x.Value == "VSYS2R_dev");
        }


        [TestMethod]
        public void GetSourceCodeSettingsTest()
        {

            foreach (var project in clientV93.GetProjects())
            {
                var projectConfig = clientV93.GetProjectConfiguration(project.Key);



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
        public void CreateProjectTEst()
        {
            clientV9.SASTClient.ProjectsManagement_PostByprojectAsync(new SaveProjectDto
            {
                IsPublic = true,
                Name = "ProjectName",
                OwningTeam = "34"
            });
        }

        [TestMethod]
        public void CreateBranchTest()
        {
            clientV9.SASTClient.BranchProjects_BranchByidprojectAsync(123, new BranchProjectDto
            {
                Name = "New Branch Name"
            }).Wait();

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

        [TestMethod]
        public void UploadPresetTest()
        {
            cxPortalWebService93.CxPresetDetails preset = clientV93.GetPresetDetails(2);

            var newPreset = new cxPortalWebService93.CxPresetDetails()
            {
                id = preset.id

            };

            clientV93.PortalSOAP.UpdatePresetAsync(null, preset);
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
    }
}
