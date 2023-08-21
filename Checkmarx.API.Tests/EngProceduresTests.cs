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
    public class EngProceduresTests
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
        public void ExclusionTest()
        {
            var exclusions = clientV9.GetExcludedSettings(1);
            clientV9.SetExcludedSettings(1, "test", "test.js");
        }

        [TestMethod]
        public void InsertDeleteQueryTest()
        {
            // NOTES
            // 1 - Is the query to create always the same? It can be something different? Or can it be more than one?

            // Gets default preset and Configuration
            var presets = clientV9.GetPresets();
            var defaultPreset = presets.Where(x => x.Value == "OWASP TOP 10 - 2017").FirstOrDefault();

            if (defaultPreset.Key == 0)
                throw new Exception($"No preset found with name ...");

            var configurations = clientV9.GetConfigurationSetList();
            var defaultConfiguration = configurations.Where(x => x.ConfigSetName == "Default Configuration").FirstOrDefault();

            if (defaultConfiguration == null)
                throw new Exception($"No Configuration found with name ...");

            // Get projects
            var projects = clientV9.GetProjects().ToList();

            // Get query
            var queryFile = "D:\\Users\\bruno.vilela\\OneDrive - Checkmarx\\Documents\\hardcoded_password.txt";
            string customQuery = File.ReadAllText(queryFile);
            string queryName = Path.GetFileNameWithoutExtension(queryFile);

            // Get querie and query group
            var querieGroups = clientV9.GetAuditQueries();
            var queries = querieGroups.SelectMany(x => x.Queries);

            var querieGroupCounter = querieGroups.Count();
            var querieCounter = queries.Count();

            // Get query templates
            var possibleQueries = queries.Where(x => x.Name.ToLower() == queryName).Select(x => x.PackageId).Distinct();
            var queryGroupSource = querieGroups.Where(x => possibleQueries.Contains(x.PackageId) && x.PackageType == CxAuditWebServiceV9.CxWSPackageTypeEnum.Cx).FirstOrDefault();
            var queryGroupSourceQuery = queryGroupSource.Queries.Where(x => x.Name.ToLower() == queryName).FirstOrDefault();

            //CxAuditWebServiceV9.CxWSQueryGroup queryGroupSource = querieGroups.Where(x => x.PackageId == 5).FirstOrDefault();
            //CxAuditWebServiceV9.CxWSQuery queryGroupSourceQuery = queries.Where(x => x.Name.ToLower() == queryName && x.PackageId == 5).FirstOrDefault();

            if (queryGroupSource == null || queryGroupSourceQuery == null)
                throw new Exception($"No query group found to override with the name {queryName}");

            // Object with the created query to update for each project
            CxAuditWebServiceV9.CxWSQueryGroup createdQueryGroup = null;
            foreach (var project in projects)
            {
                

                try
                {
                    // O QUE FAZER QUANDO JA EXISTE UMA PARA MESMO PROJECTO?
                    var alreadyCreatedQuery = querieGroups.Where(x => x.Name.ToLower() == "hardcoded_password" && x.ProjectId == project.Key).FirstOrDefault();
                    if (alreadyCreatedQuery != null)
                        continue;

                    // The first time will create, after that updates the query to run for the current project
                    if (createdQueryGroup == null)
                    {
                        var projConfig = clientV9.GetProjectConfiguration(project.Key);
                        var destinationTeamId = Convert.ToInt32(projConfig.ProjectSettings.AssociatedGroupID);

                        createdQueryGroup = InsertQueryForProject(queryGroupSource, queryGroupSourceQuery, project.Key, destinationTeamId, customQuery, QueryDescription);
                    }
                    else
                        UpdateQueryGroup(createdQueryGroup, project.Key, QueryDescription);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error inserting query into project [{project.Key}] {project.Value}");
                }
            }

            // Delete Created Query Groups by query description
            CleanCustomQueries(QueryDescription);

            var querieGroups3 = clientV9.GetQueries();
            var queries3 = querieGroups3.SelectMany(x => x.Queries);

            var querieGroupCounter3 = querieGroups3.Count();
            var querieCounter3 = queries3.Count();
        }

        [TestMethod]
        public void CleanCustomQueriesTest()
        {
            CleanCustomQueries(QueryDescription);
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
                PackageTypeName = $"CxProject_{projectId}",
                PackageFullName = $"{queryGroupSource.LanguageName}:CxProject_{projectId}:{queryGroupSource.Name}",
                ProjectId = projectId,
                Status = CxAuditWebServiceV9.QueryStatus.New,
                Queries = new CxAuditWebServiceV9.CxWSQuery[] { queryToUp }
            };

            clientV9.UploadQueries(new CxAuditWebServiceV9.CxWSQueryGroup[] { newQuerieGroup });

            // For some reason, the description is not added when creating. We need to update again with the description
            var querieGroupsRefresh = clientV9.GetAuditQueries();
            var createdQueryGroup = querieGroupsRefresh.FirstOrDefault(x => x.PackageType == CxAuditWebServiceV9.CxWSPackageTypeEnum.Project && x.ProjectId == projectId);

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

            if(queryGroupToDelete.Queries.All(x => x.Status == CxAuditWebServiceV9.QueryStatus.Deleted))
                queryGroupToDelete.Status = CxAuditWebServiceV9.QueryStatus.Deleted;

            clientV9.UploadQueries(new CxAuditWebServiceV9.CxWSQueryGroup[] { queryGroupToDelete });
        }
    }
}
