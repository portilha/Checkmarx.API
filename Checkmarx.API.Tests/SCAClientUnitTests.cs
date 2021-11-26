using Checkmarx.API.SCA;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public Guid ProjectTest = new Guid("001266f4-7917-4d0d-bacd-3f85933d56b0");
        public Guid ScanTest = new Guid("fb1ad6e0-c26c-401b-8b24-2295cc5fb9e9");

        [TestMethod]
        public void ListProjectTest()
        {
            foreach (var item in _client.ClientSCA.GetProjectsAsync().Result)
            {
                Trace.WriteLine(item.Id + " " + item.Name);
            }
        }

        [TestMethod]
        public void GetSingleProject()
        {
            var project = _client.ClientSCA.Projects3Async(ProjectTest).Result;



            Assert.IsNotNull(project);
        }

        [TestMethod]
        public void ListScansTest()
        {
            var scan = _client.ClientSCA.ScansAsync(ProjectTest).Result;

        }

        [TestMethod]
        public void GetScan()
        {
            var scan = _client.ClientSCA.ScansAsync(ScanTest).Result;



            Assert.IsNotNull(scan);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var vulns = _client.ClientSCA.VulnerabilitiesAsync(new Guid("b1f5e52c-17f6-44e6-86b6-273cb122b090")).Result;

            Assert.IsNotNull(vulns);

        }

        [TestMethod]
        public void GetReportRisk()
        {
            var riskReport = _client.ClientSCA.RiskReportsAsync(new Guid("9cba1ce7-f8d9-47be-a898-34e2f0c1562d"), null).Result;


            Assert.IsNotNull(riskReport);

        }


        [TestMethod]
        public void GetPackagesTest()
        {
            var packages = _client.ClientSCA.PackagesAsync(ScanTest).Result;

            if (packages.First().IsDevelopment)
            {

            }
        }

        [TestMethod]
        public void IgnoreVulnerabilityTest()
        {
            _client.ClientSCA.IgnoreVulnerabilityAsync(new IgnoreVulnerability
            {

            });
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


