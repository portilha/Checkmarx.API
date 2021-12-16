using Checkmarx.API.SCA;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private Guid TestProject = Guid.Empty;
        private Guid TestScan = Guid.Empty;

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

            Assert.IsNotNull(Username, "Please define the Username in the Secrets file");
            Assert.IsNotNull(Password, "Please define the Password in the Secrets file");
            Assert.IsNotNull(Tenant, "Please define the Tenant in the Secrets file");

            _client = new SCAClient(Tenant, Username, Password);
        }

        [TestInitialize]
        public void InitiateTestGuid()
        {
            string testGuid = Configuration["SCA:TestProject"];
            if (!string.IsNullOrWhiteSpace(testGuid))
                TestProject = new Guid(testGuid);

            string testScan = Configuration["SCA:TestScan"];
            if (!string.IsNullOrWhiteSpace(testScan))
                TestScan = new Guid(testScan);
        }


        [TestMethod]
        public void ConnectionTest()
        {
            Assert.IsTrue(_client.Connected);
        }

        [TestMethod]
        public void SCA_AC_Test()
        {
            AccessControlClient accessControlClient = _client.AC;
            foreach (var item in accessControlClient.TeamsAllAsync().Result.ToDictionary(x => x.FullName, StringComparer.OrdinalIgnoreCase))
            {
                Trace.WriteLine($"{item.Key} - {item.Value.Id} = {item.Value.Name} - {item.Value.ParentId}");
            }
        }


        [TestMethod]
        public void CreateTeamTest()
        {
            string newTeam = @"/CxServer/SCA-PM/Champions/ASA/Test/MyAttempt/2Level/3Level/4Level/5KEve/LOL/ASDF";

            var accessControlClient = _client.AC;

            int teamId = accessControlClient.GetOrCreateTeam(newTeam);

            Assert.IsTrue(teamId > 0);
        }


        [TestMethod]
        public void DEleteTEamTEst()
        {
            // _client.AC.DeleteTeamAsync(110437);

        }

        [TestMethod]
        public void SCA_List_Users_Test()
        {
            AccessControlClient accessControlClient = _client.AC;

            var roles = accessControlClient.RolesAllAsync().Result.ToDictionary(x => x.Id);
            var teams = accessControlClient.TeamsAllAsync().Result.ToDictionary(x => x.Id);

            foreach (var user in accessControlClient.GetAllUsersDetailsAsync().Result)
            {
                if (user.Email.EndsWith("@checkmarx.com"))
                {
                    Trace.WriteLine(user.Email + string.Join(";", user.TeamIds.Select(x => teams[x].FullName)) + " " + user.LastLoginDate);

                    foreach (var role in user.RoleIds.Select(x => roles[x].Name))
                    {
                        Trace.WriteLine("+ " + role);
                    }
                }
            }
        }

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
            Assert.IsTrue(TestProject != Guid.Empty, "Please define a TestProjectGuid in the secrets file");

            foreach (var scan in _client.ClientSCA.GetScansForProjectAsync(TestProject).Result.Where(x => x.Status.Name == "Done"))
            {
                Trace.WriteLine(scan.ScanId + " " + scan.Status.Name);

                foreach (var package in _client.ClientSCA.PackagesAsync(scan.ScanId).Result)
                {
                }
            }
        }

        [TestMethod]
        public void GetProject()
        {
            Assert.IsTrue(TestProject != Guid.Empty, "Please define a TestProjectGuid in the secrets file");

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
        public void GetPackagesTest()
        {
            foreach (var item in _client.ClientSCA.PackagesAsync(TestScan).Result)
            {
                Trace.WriteLine(item.Id);
            }
        }


        [TestMethod]
        public void GetAllDevPackagesTest()
        {
            foreach (var project in _client.ClientSCA.GetProjectsAsync().Result)
            {
                Trace.WriteLine("+" + project.Name);

                var scan = _client.ClientSCA.GetScansForProjectAsync(project.Id).Result.FirstOrDefault();

                if (scan == null || scan.Status.Name != "Done")
                    continue;

                try
                {
                    foreach (var package in _client.ClientSCA.PackagesAsync(scan.ScanId).Result)
                    {
                        if (package.IsDevelopment)
                        {
                            Trace.WriteLine("\t-" + package.Id);
                        }

                        foreach (var dep in package.DependencyPaths)
                        {
                            foreach (var depdep in dep)
                            {
                                if (depdep.IsDevelopment)
                                    Trace.WriteLine(depdep.Id);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("ERROR " + ex.Message);

                }
            }
        }


        [TestMethod]
        public void GetVulnerabilitiesFromScanTest()
        {

            var vulns = _client.ClientSCA.VulnerabilitiesAsync(TestScan).Result;

            foreach (var vulnerabiltity in vulns)
            {
                Trace.WriteLine(vulnerabiltity.CveName);
            }


        }

        [TestMethod]
        public void GetReportRisk()
        {

            var riskReport = _client.ClientSCA.RiskReportsAsync(TestProject, null).Result;

            Assert.IsNotNull(riskReport);

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
            foreach (var project in _client.ClientSCA.GetProjectsAsync().Result)
            {
                try
                {
                    // Uncomment to execute the action
                    //_client.ClientSCA.UpdateProjectsSettingsAsync(project.Id,
                    //            new API.SCA.ProjectSettings { EnableExploitablePath = true }).Wait();

                    var settings = _client.ClientSCA.GetProjectsSettingsAsync(project.Id).Result;

                    Assert.IsTrue(settings.EnableExploitablePath);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(project.Name + " " + ex.Message);
                }
            }
        }

        [TestMethod]
        public void CreateProjectTest()
        {
            var project = _client.ClientSCA.CreateProjectAsync(new CreateProject
            {
                AssignedTeams = new string[] { "/CxServer/SCA-PM/Champions/UK" },
                Name = "MyFirstTest3"
            }).Result;

            Assert.IsNotNull(_client.ClientSCA.GetProjectAsync(project.Id).Result);
        }

        [TestMethod]
        public void GetProjectAndScanTest()
        {
            var result = _client.ClientSCA.CreateProjectAsync(new CreateProject
            {
                AssignedTeams = new string[] { "/CxServer/SCA-PM/Champions/UK" },
                Name = "MyFirstTest4"
            }).Result;

            // Assert.IsNotNull(_client.ClientSCA.GetProjectAsync(project.Id).Result);

            // var result = _client.ClientSCA.GetProjectAsync("MyFirstTest4").Result;

            Assert.IsNotNull(result);

            // Run Scan.

            _client.ScanWithSourceCode(result.Id, @"C:\Users\pedropo\Downloads\WebGoat-develop.zip");


        }
    }
}


