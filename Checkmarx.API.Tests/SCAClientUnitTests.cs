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

        [TestMethod]
        public void GetProject()
        {
            var project = _client.ClientSCA.Projects3Async(new Guid("cbec47bd-67b6-468b-8ade-f621fc0eaa17")).Result;
            Assert.IsNotNull(project);
        }

        [TestMethod]
        public void GetScan()
        {
            var scan = _client.ClientSCA.ScansAsync(new Guid("fb1ad6e0-c26c-401b-8b24-2295cc5fb9e9")).Result;
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


