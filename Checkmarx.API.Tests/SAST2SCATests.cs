using Checkmarx.API.SAST;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Checkmarx.API.Tests
{
    [TestClass]
    public class SAST2SCATests
    {
        public static IConfigurationRoot Configuration { get; private set; }

        private static CxClient _sastClient;
        private static SCAClient _scaClient;

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            var builder = new ConfigurationBuilder()
               .AddUserSecrets<SAST2SCATests>();

            Configuration = builder.Build();
            string Username = Configuration["SCA:Username"];
            string Password = Configuration["SCA:Password"];
            string Tenant = Configuration["SCA:Tenant"];

            string AC_Server = Configuration["SCA:AC_Server"];
            string API_URL = Configuration["SCA:API_URL"];


            Assert.IsNotNull(Username, "Please define the Username in the Secrets file");
            Assert.IsNotNull(Password, "Please define the Password in the Secrets file");
            Assert.IsNotNull(Tenant, "Please define the Tenant in the Secrets file");

            _scaClient = new SCAClient(Tenant, Username, Password, AC_Server, API_URL);

            _sastClient =
                new CxClient(new Uri(Configuration["V9:URL"]),
                Configuration["V9:Username"],
                new NetworkCredential("", Configuration["V9:Password"]).Password);
        }


        [TestMethod]
        public void ListSCAorSASTTeam()
        {
            Trace.WriteLine("\r\n--- SCA ---");

            foreach (var team in _scaClient.AC.TeamsAllAsync().Result)
            {
                Trace.WriteLine(team.FullName);
            }

            Trace.WriteLine("\r\n--- SAST ---");

            foreach (var team in _sastClient.AC.TeamsAllAsync().Result)
            {
                Trace.WriteLine(team.FullName);
            }
        }


        [TestMethod]
        public void SyncTeams()
        {
            var sastTeams = _sastClient.AC.TeamsAllAsync().Result;

            foreach (var item in sastTeams)
            {
                _scaClient.AC.GetOrCreateTeam(item.FullName);
            }
        }

        [TestMethod]
        public void SASTProjectIntoSCATest()
        {
            var listOfSCAProjects = _scaClient.ClientSCA.GetProjectsAsync().Result.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var item in _sastClient.GetAllProjectsDetails())
            {
                string zipPath = Path.ChangeExtension(Path.GetTempFileName(), ".zip");

                try
                {
                    string teamFullPath = _sastClient.GetProjectTeamName(item.TeamId);

                    var scanId = _sastClient.GetLastScan(item.Id).Id;

                    File.WriteAllBytes(zipPath, _sastClient.GetSourceCode(scanId));

                    _scaClient.AC.GetOrCreateTeam(teamFullPath);

                    string scaProjectName = item.Name;

                    // If the project already exists.

                    #region Project Name Collision Handling

                    API.SCA.Project scaProject = null;

                    bool skipCreation = false;
                    if (listOfSCAProjects.ContainsKey(scaProjectName))
                    {
                        scaProject = listOfSCAProjects[scaProjectName];

                        if (!scaProject.AssignedTeams.Contains(teamFullPath))// Colision
                            scaProjectName = teamFullPath.Split("/", StringSplitOptions.RemoveEmptyEntries).Last() + "_" + scaProjectName;
                        else
                            skipCreation = true;
                    }

                    #endregion

                    if (!skipCreation)
                    {
                        scaProject = _scaClient.ClientSCA.CreateProjectAsync(new API.SCA.CreateProject
                        {
                            Name = scaProjectName,
                            AssignedTeams = new string[] { teamFullPath }
                        }).Result;

                        listOfSCAProjects.Add(scaProjectName, scaProject);
                    }

                    _scaClient.ScanWithSourceCode(scaProject.Id, zipPath);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
                finally
                {
                    if (File.Exists(zipPath))
                        File.Delete(zipPath);
                }
            }
        }
    }
}
