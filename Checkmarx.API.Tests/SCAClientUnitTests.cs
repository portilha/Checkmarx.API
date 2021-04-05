using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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

        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<CxClientUnitTests>();

            Configuration = builder.Build();
            Username = Configuration["SCA:Username"];
            Password = Configuration["SCA:Password"];
            Tenant = Configuration["SCA:Tenant"];
        }

        [TestMethod]
        public void ConnectionTest()
        {
            var client = new SCAClient(Tenant, Username, Password);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetProject()
        {
            var client = new SCAClient(Tenant, Username, Password);
            if (client.Connected)
            {
                var project = client.ClientSCA.Projects3Async(new Guid("cbec47bd-67b6-468b-8ade-f621fc0eaa17")).Result;
                Assert.IsNotNull(project);
            }
        }

        [TestMethod]
        public void GetScan()
        {
            var client = new SCAClient(Tenant, Username, Password);
            if (client.Connected)
            {
                var scan = client.ClientSCA.ScansAsync(new Guid("fb1ad6e0-c26c-401b-8b24-2295cc5fb9e9")).Result;
                Assert.IsNotNull(scan);
            }
        }
        [TestMethod]
        public void MyTestMethod()
        {
            var client = new SCAClient(Tenant, Username, Password);
            if (client.Connected)
            {
                var vulns = client.ClientSCA.VulnerabilitiesAsync(new Guid("b1f5e52c-17f6-44e6-86b6-273cb122b090")).Result;
                Assert.IsNotNull(vulns);
            }
        }
    }
}


