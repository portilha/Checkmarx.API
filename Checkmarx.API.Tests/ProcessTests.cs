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
using Checkmarx.API.Tests.Utils;
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
            clientV9.DeleteAnyQueryGroupWithTheDescription(QueryDescription);
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

                    var exclusions = Exclusions.FromJson(TestUtils.ReadEmbeddedFile("exclusions.json"));

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




        


    }
}
