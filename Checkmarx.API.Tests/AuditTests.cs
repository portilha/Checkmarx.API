using Checkmarx.API.SCA;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Checkmarx.API.Tests;

[TestClass]
public class AuditTests
{
    public static IConfigurationRoot Configuration { get; private set; }

    private static CxClient _sastClient;

    [ClassInitialize]
    public static void InitializeTest(TestContext testContext)
    {
        var builder = new ConfigurationBuilder()
           .AddUserSecrets<AuditTests>();

        Configuration = builder.Build();

        string url = Configuration["V93:URL"];
        if (!string.IsNullOrWhiteSpace(url))
        {
            _sastClient =
                 new CxClient(new Uri(url),
                 Configuration["V93:Username"],
                 new NetworkCredential("", Configuration["V93:Password"]).Password);
        }
    }

    [TestMethod]
    public void GetQueriesTest()
    {
        var queries = _sastClient.GetAuditQueries().ToArray();

        foreach (var queryGroup in queries)
        {
            foreach (var query in queryGroup.Queries)
            {
                if (query.IsExecutable && queryGroup.PackageType == CxAuditWebServiceV9.CxWSPackageTypeEnum.Corporate)
                    Trace.WriteLine($"----------------------" +
                        $"\nName: {query.Name} " +
                        $"\nPackageFullName: {queryGroup.PackageFullName} " +
                        $"\nSeverity: {query.Severity} " +
                        $"\nEngineMetadata: {query.EngineMetadata}" +
                        $"\nTeam: {queryGroup.OwningTeam}" +
                        $"\nPackageTypeName: {queryGroup.PackageTypeName}" +
                        $"\nProjectId: {queryGroup.ProjectId}" +
                        $"\nSource:\n {query.Source}\n");

            }

        }

    }


    [TestMethod]
    public void LowerSeverityTEst()
    {
        // ask just once.
        var queryGroups = _sastClient.GetAuditQueries().ToArray();

        var corpGroups = queryGroups.Where(x => x.PackageType == CxAuditWebServiceV9.CxWSPackageTypeEnum.Corporate)
                        .ToDictionary(y => $"{y.LanguageName}:{y.Name}");

        foreach (var queryGroup in queryGroups.Where(x => x.PackageType == CxAuditWebServiceV9.CxWSPackageTypeEnum.Cx))
        {
            var queryGroupName = $"{queryGroup.LanguageName}:{queryGroup.Name}";

            foreach (var query in queryGroup.Queries.Where(x => x.Severity == (int)CxClient.Severity.Medium))
            {
                if (!query.IsExecutable)
                    continue;

                if (corpGroups.ContainsKey(queryGroupName))
                {
                    var overriden = corpGroups[queryGroupName].Queries.SingleOrDefault(x => x.Name == query.Name);
                    if (overriden!= null)
                    {
                        Trace.WriteLine($"An override already exists for {queryGroupName} {query.Name} with the severity {CxClient.toSeverityToString(overriden.Severity)}, skipping downgrade.");
                        continue;
                    }
                }

                // check if the query is already downgraded
                Trace.WriteLine($"Overriding {query.Name} in {queryGroup.PackageFullName}");

                try
                {
                    _sastClient.OverrideQueryInCorporate(
                        queryGroup,
                        query,
                        $"result = base.{query.Name}();",
                        CxClient.Severity.High,
                        "Downgrade of query to High");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error downgrading query {query.Name} ({query.QueryId}) severity. Reason: {ex.Message}");
                }
            }
        }
    }


    [TestMethod]
    public void DeleteQueriesTest()
    {
        // ask just once.
        var queryGroups = _sastClient.GetAuditQueries().ToArray();

        var corpGroups = queryGroups.Where(x => x.PackageType == CxAuditWebServiceV9.CxWSPackageTypeEnum.Corporate);

        foreach (var item in corpGroups)
        {
            Trace.WriteLine(item.PackageFullName);

            foreach (var query in item.Queries)
            {
                Trace.WriteLine(query.Name);

                query.Status = CxAuditWebServiceV9.QueryStatus.Deleted;
            }
        }

        _sastClient.UploadQueries(queryGroups);
       

    }
}
