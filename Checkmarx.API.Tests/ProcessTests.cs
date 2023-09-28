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
    public class ProcessTests
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
        public void CountQueriesTest()
        {
            int projectId = 1;

            var projects = clientV9.GetProjects();
            var project = projects[projectId];

            var querieGroups = clientV9.GetQueries();
            var queries = querieGroups.SelectMany(x => x.Queries);

            var querieGroupCounter = querieGroups.Count();
            var querieCounter = queries.Count();

            var alreadyCreatedQuery = querieGroups.Where(x => x.Name.ToLower() == "hardcoded_password" && x.ProjectId == projectId).FirstOrDefault();
            if (alreadyCreatedQuery != null)
                Trace.WriteLine($"WARNING: The custom query \"hardcoded_password\" exists in this project query group.");

            Trace.WriteLine($"Querie Groups: {querieGroupCounter}");
            Trace.WriteLine($"Queries: {querieCounter}");
        }

        [TestMethod]
        public void CountQueriesTest2()
        {
            int projectId = 8;

            var projects = clientV9.GetAllProjectsDetails();
            var project = projects.FirstOrDefault(x => x.Id == projectId);

            var totalProjectQueries = clientV9.GetProjectLevelQueries(project.Id).Count();
            var totalTeamCorpQueries = clientV9.GetTeamCorpLevelQueries(project.TeamId).Count();
        }

        [TestMethod]
        public void CleanCustomQueriesTest()
        {
            CleanCustomQueries(QueryDescription);
        }

        public void ExclusionTest()
        {
            var exclusions = clientV9.GetExcludedSettings(2);
            clientV9.SetExcludedSettings(1, "test", "test.js");
        }

        public void SuggestExclusionsTest()
        {
            int projectId = 2;
            int scanId = 1000005;

            var currentExclusions = clientV9.GetExcludedSettings(projectId);
            if (string.IsNullOrEmpty(currentExclusions.Item1) || string.IsNullOrEmpty(currentExclusions.Item2))
            {
                string extractPath = Path.GetTempFileName();
                string zipPath = Path.GetTempFileName();

                try
                {
                    File.WriteAllBytes(zipPath, clientV9.GetSourceCode(scanId));

                    if (File.Exists(extractPath))
                        File.Delete(extractPath);

                    // unzip
                    ZipFile.ExtractToDirectory(zipPath, extractPath, true);

                    var exclusions = Exclusions.FromJson(File.ReadAllText("Assets/exclusions.json"));

                    Regex[] filesRegex = exclusions.Files.Select(x => new Regex(x, RegexOptions.Compiled)).ToArray();
                    Regex[] foldersRegex = exclusions.Folders.Select(x => new Regex(x, RegexOptions.Compiled)).ToArray();

                    List<string> foldersExclusions = new List<string>();
                    List<string> filesExclusions = new List<string>();

                    // detect exclusions
                    foreach (var directoy in Directory.EnumerateDirectories(extractPath, "*.*"))
                    {
                        if (foldersRegex.Any(x => x.Match(directoy).Success))
                        {
                            foreach (var item in foldersRegex.Where(x => x.Match(directoy).Success))
                            {
                                if (!foldersExclusions.Any(x => x == item.ToString()))
                                    foldersExclusions.Add(item.ToString());
                            }
                        }
                    }

                    foreach (var file in Directory.EnumerateFiles(extractPath, "*.*"))
                    {
                        if (filesRegex.Any(x => x.Match(file).Success))
                        {
                            foreach (var item in filesRegex.Where(x => x.Match(file).Success))
                            {
                                if (!filesExclusions.Any(x => x == item.ToString()))
                                    filesExclusions.Add(item.ToString());
                            }
                        }
                    }

                    string finalFolderExclusions = string.IsNullOrEmpty(currentExclusions.Item1) ?
                        foldersExclusions.Any() ? string.Join(",", foldersExclusions) : string.Empty
                        : currentExclusions.Item1;
                    string finalFileExclusions = string.IsNullOrEmpty(currentExclusions.Item2) ?
                        filesExclusions.Any() ? string.Join(",", filesExclusions) : string.Empty
                        : currentExclusions.Item2;

                    clientV9.SetExcludedSettings(projectId, finalFolderExclusions, finalFileExclusions);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (File.Exists(zipPath))
                        File.Delete(zipPath);

                    if (File.Exists(extractPath))
                        File.Delete(extractPath);
                }
            }
        }

        [TestMethod]
        public void InsertDeleteQueryTest()
        {
            // Gets default preset and Configuration
            var presets = clientV9.GetPresets();
            var defaultPreset = presets.Where(x => x.Value == "ASA Premium").FirstOrDefault();

            if (defaultPreset.Key == 0)
                throw new Exception($"No preset found with name ...");

            var configurations = clientV9.GetConfigurationSetList();
            var defaultConfiguration = configurations.Where(x => x.ConfigSetName == "Default Configuration").FirstOrDefault();

            if (defaultConfiguration == null)
                throw new Exception($"No Configuration found with name ...");

            // Get projects
            var projects = clientV9.GetAllProjectsDetails().Where(x => x.Id == 8).ToList();

            // Get query
            var queryFile = "D:\\Users\\bruno.vilela\\OneDrive - Checkmarx\\Documents\\use_of_hardcoded_password.txt";
            string customQuery = File.ReadAllText(queryFile);
            string queryName = Path.GetFileNameWithoutExtension(queryFile);

            // Get querie and query group
            var querieGroups = clientV9.GetAuditQueries();
            var queries = querieGroups.SelectMany(x => x.Queries);

            var querieGroupCounter = querieGroups.Count();
            var querieCounter = queries.Count();

            // Get query templates
            var possibleQueries = queries.Where(x => x.Name.ToLower() == queryName).Select(x => x.PackageId).Distinct();
            var queryGroupSources = querieGroups.Where(x => possibleQueries.Contains(x.PackageId) && x.PackageType == CxAuditWebServiceV9.CxWSPackageTypeEnum.Cx);
            

            // Object with the created query to update for each project
            CxAuditWebServiceV9.CxWSQueryGroup createdQueryGroup = null;
            foreach (var project in projects)
            {
                try
                {
                    // Get detected languages
                    var lastScan = clientV9.GetLastScan(project.Id);
                    var scanInfo = GetScanAccuracyAndLanguagesFromScanLog(lastScan.Id);

                    // Logic to select group query to override
                    if(!scanInfo.Item2.Any())
                        throw new Exception($"No query group found to override with the name {queryName}");

                    string language = null;
                    if(scanInfo.Item2.Any(x => x.ToLower() == "csharp"))
                        language = "csharp";
                    else if (scanInfo.Item2.Any(x => x.ToLower() == "java"))
                        language = "java";
                    else if (scanInfo.Item2.Any(x => x.ToLower() == "javascript"))
                        language = "javascript";
                    else if (scanInfo.Item2.Any(x => x.ToLower() == "python"))
                        language = "python";


                    CxAuditWebServiceV9.CxWSQueryGroup queryGroupSource = null;
                    CxAuditWebServiceV9.CxWSQuery queryGroupSourceQuery = null;
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        queryGroupSource = queryGroupSources.Where(x => x.PackageFullName.ToLower().StartsWith(language)).FirstOrDefault();
                        if(queryGroupSource != null)
                            queryGroupSourceQuery = queryGroupSource.Queries.Where(x => x.Name.ToLower() == queryName).FirstOrDefault();
                    }
                    
                    if(queryGroupSource == null || queryGroupSourceQuery == null)
                    {
                        foreach(var lang in scanInfo.Item2)
                        {
                            queryGroupSource = queryGroupSources.Where(x => x.PackageFullName.ToLower().StartsWith(lang.ToLower())).FirstOrDefault();
                            if (queryGroupSource != null)
                                queryGroupSourceQuery = queryGroupSource.Queries.Where(x => x.Name.ToLower() == queryName).FirstOrDefault();
                        }
                    }

                    if (queryGroupSource == null || queryGroupSourceQuery == null)
                        throw new Exception($"No query group found to override with the name {queryName}");

                    // O QUE FAZER QUANDO JA EXISTE UMA PARA MESMO PROJECTO?
                    var alreadyCreatedQuery = querieGroups.Where(x => x.Name.ToLower() == "use_of_hardcoded_password" && x.ProjectId == project.Id).FirstOrDefault();
                    if (alreadyCreatedQuery != null)
                        continue;

                    // The first time will create, after that updates the query to run for the current project
                    if (createdQueryGroup == null)
                    {
                        var projConfig = clientV9.GetProjectConfigurations(project.Id);
                        var destinationTeamId = Convert.ToInt32(projConfig.ProjectSettings.AssociatedGroupID);

                        createdQueryGroup = InsertQueryForProject(queryGroupSource, queryGroupSourceQuery, project.Id, destinationTeamId, customQuery, QueryDescription);

                        var scandId = clientV9.RunSASTScan(project.Id, useLastScanPreset: false, presetId: defaultPreset.Key, configurationId: (int)defaultConfiguration.ID);
                    }

                    var querieGroups4 = clientV9.GetQueries();
                    var queries4 = querieGroups4.SelectMany(x => x.Queries);

                    var querieGroupCounter4 = querieGroups4.Count();
                    var querieCounter4 = queries4.Count();

                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error inserting query into project [{project.Id}] {project.Name}");
                }
            }

            // Delete Created Query Groups by query description
            CleanCustomQueries(QueryDescription);

            var querieGroups3 = clientV9.GetQueries();
            var queries3 = querieGroups3.SelectMany(x => x.Queries);

            var querieGroupCounter3 = querieGroups3.Count();
            var querieCounter3 = queries3.Count();
        }

        private Tuple<double, List<string>> GetScanAccuracyAndLanguagesFromScanLog(long scanId)
        {
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
            double scanAccuracy = 0;
            List<string> scanLanguages = new List<string>();

            string log = File.ReadAllText(logFilePath);
            Regex regex = new Regex("^Scan\\scoverage:\\s+(?<pc>[\\d\\.]+)\\%", RegexOptions.Multiline);
            MatchCollection mc = regex.Matches(log);
            foreach (Match m in mc)
            {
                GroupCollection groups = m.Groups;
                double.TryParse(groups["pc"].Value.Replace(".", ","), out scanAccuracy);
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
                scanLanguages = langsTmp;
            }

            return new Tuple<double, List<string>>(scanAccuracy, scanLanguages);
        }

        public void CleanCustomQueries(string queryDescription)
        {
            // Get querie and query group
            var querieGroups = clientV9.GetAuditQueries();
            var queryGroupSource = querieGroups.Where(x => x.Description == queryDescription);

            // Delete Query Group
            if (queryGroupSource.Any())
            {
                foreach (var createdQueryGroup in queryGroupSource)
                    DeleteQueryGroup(createdQueryGroup);
            }
        }

        public CxAuditWebServiceV9.CxWSQueryGroup InsertQueryForProject(CxAuditWebServiceV9.CxWSQueryGroup queryGroupSource, CxAuditWebServiceV9.CxWSQuery queryGroupSourceQuery, long projectId, int teamId, string customQuery, string description)
        {
            // Create query objects and insert new
            CxAuditWebServiceV9.CxWSQuery queryToUp = new CxAuditWebServiceV9.CxWSQuery()
            {
                Cwe = queryGroupSourceQuery.Cwe,
                EngineMetadata = " ",
                Name = queryGroupSourceQuery.Name,
                Severity = queryGroupSourceQuery.Severity,
                Source = customQuery,
                Status = CxAuditWebServiceV9.QueryStatus.New,
                Type = CxAuditWebServiceV9.CxWSQueryType.Regular,
                IsExecutable = queryGroupSourceQuery.IsExecutable,
                IsEncrypted = queryGroupSourceQuery.IsEncrypted
            };

            string packageTypeName = $"CxProject_{projectId}";
            string packageFullName = $"{queryGroupSource.LanguageName}:CxProject_{projectId}:{queryGroupSource.Name}";
            var newQuerieGroup = new CxAuditWebServiceV9.CxWSQueryGroup()
            {
                //Description = queryGroupSource.Description,
                Description = description,
                IsEncrypted = queryGroupSource.IsEncrypted,
                IsReadOnly = queryGroupSource.IsReadOnly,
                Language = queryGroupSource.Language,
                LanguageName = queryGroupSource.LanguageName,
                Name = queryGroupSource.Name,
                OwningTeam = teamId,
                PackageType = CxAuditWebServiceV9.CxWSPackageTypeEnum.Project,
                PackageTypeName = packageTypeName,
                PackageFullName = packageFullName,
                ProjectId = projectId,
                Status = CxAuditWebServiceV9.QueryStatus.New,
                Queries = new CxAuditWebServiceV9.CxWSQuery[] { queryToUp }
            };

            clientV9.UploadQueries(new CxAuditWebServiceV9.CxWSQueryGroup[] { newQuerieGroup });

            // For some reason, the description is not added when creating. We need to update again with the description
            var querieGroupsRefresh = clientV9.GetAuditQueries();
            var createdQueryGroup = querieGroupsRefresh.FirstOrDefault(x => x.PackageType == CxAuditWebServiceV9.CxWSPackageTypeEnum.Project && x.ProjectId == projectId && x.PackageFullName == packageFullName);

            createdQueryGroup.Description = description;
            clientV9.UploadQueries(new CxAuditWebServiceV9.CxWSQueryGroup[] { createdQueryGroup });

            return createdQueryGroup;
        }

        public void UpdateQueryGroup(CxAuditWebServiceV9.CxWSQueryGroup queryGroupToUpdate, long projectId, string description)
        {
            queryGroupToUpdate.Status = CxAuditWebServiceV9.QueryStatus.Edited;
            queryGroupToUpdate.PackageTypeName = $"CxProject_{projectId}";
            queryGroupToUpdate.PackageFullName = $"{queryGroupToUpdate.LanguageName}:CxProject_{projectId}:{queryGroupToUpdate.Name}";
            queryGroupToUpdate.ProjectId = projectId;
            queryGroupToUpdate.Description = description;

            clientV9.UploadQueries(new CxAuditWebServiceV9.CxWSQueryGroup[] { queryGroupToUpdate });
        }

        public void DeleteQueryGroup(CxAuditWebServiceV9.CxWSQueryGroup queryGroupToDelete)
        {
            foreach (var query in queryGroupToDelete.Queries)
                query.Status = CxAuditWebServiceV9.QueryStatus.Deleted;

            if (queryGroupToDelete.Queries.All(x => x.Status == CxAuditWebServiceV9.QueryStatus.Deleted))
                queryGroupToDelete.Status = CxAuditWebServiceV9.QueryStatus.Deleted;

            clientV9.UploadQueries(new CxAuditWebServiceV9.CxWSQueryGroup[] { queryGroupToDelete });
        }
    }
}
