using Checkmarx.API.SCA;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Checkmarx.API.Tests.SCA
{
    [TestClass]
    public class SCAClientUnitTests
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static string Username;
        private static string Password;
        private static string Tenant;
        private static SCAClient _client;
        //private static string AC = "https://platform.checkmarx.net";
        //private static string APIURL = "https://api-sca.checkmarx.net";

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<CxClientUnitTests>();

            Configuration = builder.Build();
            Username = Configuration["SCA:Username"];
            Password = Configuration["SCA:Password"];
            Tenant = Configuration["SCA:Tenant"];

            _client = new SCAClient(Tenant, Username, Password);
        }

        [TestMethod]
        public void ConnectionTest()
        {
            Assert.IsTrue(_client.Connected);
        }

        private Guid TestProject = new Guid("001266f4-7917-4d0d-bacd-3f85933d56b0");
        private Guid TestScan = new Guid("d056c85b-759c-4eea-8188-001baa505a70");

        [TestMethod]
        public void ListAllProjects()
        {
            foreach (var project in _client.ClientSCA.GetProjectsAsync().Result)
            {
                Trace.WriteLine(project.Id + "  " + project.Name);
            }
        }

        [TestMethod]
        public void GEtAllScanFromProject()
        {
            foreach (var scan in _client.ClientSCA.ScansAllAsync(TestProject).Result)
            {
                Trace.WriteLine(scan.ScanId);
            }
        }

        [TestMethod]
        public void GetProject()
        {
            var project = _client.ClientSCA.GetProjectAsync(TestProject).Result;
            Assert.IsNotNull(project);
        }

        [TestMethod]
        public void GetScan()
        {
            var scan = _client.ClientSCA.GetScanAsync(TestScan).Result;
            Assert.IsNotNull(scan);
        }

        [TestMethod]
        public void GetVulnerabilitiesFromScanTest()
        {
            var vulns = _client.ClientSCA.VulnerabilitiesAsync(TestScan).Result;

            foreach (var vulnerabiltity in vulns)
            {
                Trace.WriteLine(vulnerabiltity.CveName);
            }

            Assert.IsNotNull(vulns);

        }

        [TestMethod]
        public void GetReportRisk()
        {
            var riskReport = _client.ClientSCA.RiskReportsAsync(TestProject, null).Result;
            Assert.IsNotNull(riskReport);

        }

        [TestMethod]
        public void GetExploitablePathTest()
        {
            foreach (var project in _client.ClientSCA.GetProjectsAsync(string.Empty).Result)
            {
                var settings = _client.ClientSCA.GetProjectsSettingsAsync(project.Id).Result;
                Trace.WriteLine(project.Name + " -> " + settings.EnableExploitablePath);
            }
        }

        [TestMethod]
        public void EnableExploitablePathForAllTest()
        {
            foreach (var project in _client.ClientSCA.GetProjectsAsync(string.Empty).Result)
            {
                try
                {
                    _client.ClientSCA.UpdateProjectsSettingsAsync(project.Id,
                                new API.SCA.ProjectSettings { EnableExploitablePath = true }).Wait();

                    var settings = _client.ClientSCA.GetProjectsSettingsAsync(project.Id).Result;
                    Assert.IsTrue(settings.EnableExploitablePath);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(project.Name + " " + ex.Message);
                }
            }
        }
    }
}


