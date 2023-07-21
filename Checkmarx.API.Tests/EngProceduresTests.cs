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
        public void InsertDeleteQueryTest()
        {
            // NOTES
            // 1 - Is the query to create always the same? It can be something different? Or can it be more than one?
            // 2 - Creating and saving the created object to delete does not work -> Probably because the object does not have a PackageId.
            // I need to search for the created QueryGroups and then Delete them -> This is a problem
            // 5 - What do i do when there is already a project with the same name for the same project?


            var projects = clientV9.GetProjects().ToList();

            var queryFile = "D:\\Users\\bruno.vilela\\OneDrive - Checkmarx\\Documents\\query.txt";
            string customQuery = File.ReadAllText(queryFile);

            // Get querie and query group
            var querieGroups = clientV9.GetAuditQueries();
            var queries = querieGroups.SelectMany(x => x.Queries);

            var querieGroupCounter = querieGroups.Count();
            var querieCounter = queries.Count();

            CxAuditWebServiceV9.CxWSQueryGroup queryGroupSource = querieGroups.Where(x => x.PackageId == 5).FirstOrDefault();
            CxAuditWebServiceV9.CxWSQuery queryGroupSourceQuery = queries.Where(x => x.Name.ToLower() == "hardcoded_password" && x.PackageId == 5).FirstOrDefault();

            foreach (var project in projects)
            {
                var projConfig = clientV9.GetProjectConfiguration(project.Key);
                var destinationTeamId = Convert.ToInt32(projConfig.ProjectSettings.AssociatedGroupID);

                // O QUE FAZER QUANDO JA EXISTE UMA PARA MESMO PROJECTO? -> Verificar antes
                UploadQueryForProject(queryGroupSource, queryGroupSourceQuery, project.Key, destinationTeamId, customQuery);

                var querieGroups2 = clientV9.GetQueries();
                var queries2 = querieGroups2.SelectMany(x => x.Queries);

                var querieGroupCounter2 = querieGroups2.Count();
                var querieCounter2 = queries2.Count();
            }

            // Delete Created Query Groups -> TENHO DE FAZER MELHOR FILTRO -> A ENCONTRAR MAIS QUERIES
            CleanCustomQueries("hardcoded_password");

            var querieGroups3 = clientV9.GetQueries();
            var queries3 = querieGroups3.SelectMany(x => x.Queries);

            var querieGroupCounter3 = querieGroups3.Count();
            var querieCounter3 = queries3.Count();
        }

        [TestMethod]
        public void CleanCustomQueriesTest()
        {
            CleanCustomQueries("hardcoded_password");
        }

        public void CleanCustomQueries(string queryName)
        {
            // Get querie and query group
            var querieGroups = clientV9.GetAuditQueries();
            var queries = querieGroups.SelectMany(x => x.Queries);

            // NAO CHEGA! -> Encontra outras queries
            var queryGroupSourceQuery = queries.Where(x => x.Name.ToLower() == queryName).ToList();
            var queryGroupSource = querieGroups.Where(x => queryGroupSourceQuery.Select(x => x.PackageId).Contains(x.PackageId) 
                                                        && x.PackageType == CxAuditWebServiceV9.CxWSPackageTypeEnum.Project).ToList();


            // Delete Query Group
            if (queryGroupSource.Any())
            {
                foreach (var createdQueryGroup in queryGroupSource)
                    DeleteQueryGroup(createdQueryGroup);

                var querieGroups3 = clientV9.GetQueries();
                var queries3 = querieGroups3.SelectMany(x => x.Queries);

                var querieGroupCounter3 = querieGroups3.Count();
                var querieCounter3 = queries3.Count();
            }
        }

        public CxAuditWebServiceV9.CxWSQueryGroup UploadQueryForProject(CxAuditWebServiceV9.CxWSQueryGroup queryGroupSource, CxAuditWebServiceV9.CxWSQuery queryGroupSourceQuery, long projectId, int teamId, string customQuery)
        {
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
                Description = queryGroupSource.Description,
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

            return newQuerieGroup;
        }

        public void UpdateQueryGroup(CxAuditWebServiceV9.CxWSQueryGroup queryGroupToUpdate, long projectId, int teamId)
        {
            queryGroupToUpdate.Status = CxAuditWebServiceV9.QueryStatus.Edited;
            queryGroupToUpdate.PackageTypeName = $"CxProject_{projectId}";
            queryGroupToUpdate.PackageFullName = $"{queryGroupToUpdate.LanguageName}:CxProject_{projectId}:{queryGroupToUpdate.Name}";
            queryGroupToUpdate.ProjectId = projectId;

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
