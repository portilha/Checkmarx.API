using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace Checkmarx.API.Tests
{
    [TestClass]
    public class ACTests
    {
        public static IConfigurationRoot Configuration { get; private set; }

        private static CxClient clientV89;
        private static CxClient clientV9;
        private static CxClient clientV93;


        [ClassInitialize]
        public static void InitializeTest(TestContext testContext)
        {
            // TODO REMOVE
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<ACTests>();

            Configuration = builder.Build();

            string v8 = Configuration["V89:URL"];

            if (!string.IsNullOrWhiteSpace(v8))
            {
                clientV89 =
                        new CxClient(new Uri(v8),
                        Configuration["V89:Username"],
                        new NetworkCredential("", Configuration["V89:Password"]).Password);
                var version = clientV89.Version;
            }

            string v9 = Configuration["V9:URL"];
            if (!string.IsNullOrWhiteSpace(v9))
            {
                clientV9 =
                    new CxClient(new Uri(v9),
                    Configuration["V9:Username"],
                    new NetworkCredential("", Configuration["V9:Password"]).Password);
                var _ = clientV9.Version;
            }

            string v93 = Configuration["V93:URL"];
            if (!string.IsNullOrWhiteSpace(v93))
            {
                clientV93 =
                     new CxClient(new Uri(v93),
                     Configuration["V93:Username"],
                     new NetworkCredential("", Configuration["V93:Password"]).Password);
            }

        }


        [TestMethod]
        public void UpdateExpirationDateTest()
        {
            AccessControlClient accessControlClient = clientV9.AC;

            foreach (var user in accessControlClient.GetAllUsersDetailsAsync().Result.Where(x => x.Active))
            {
                Trace.WriteLine(user.UserName + " " + user.ExpirationDate);

                //accessControlClient.UpdateUserDetails(user.Id,
                //    new UpdateUserModel
                //    {
                //        FirstName = user.FirstName,
                //        LastName = user.LastName,
                //        AllowedIpList = user.AllowedIpList,
                //        CellPhoneNumber = user.CellPhoneNumber,
                //        Country = user.Country,
                //        Email = user.Email,
                //        JobTitle = user.JobTitle,
                //        LocaleId = user.LocaleId,
                //        Other = user.Other,
                //        RoleIds = user.RoleIds,
                //        TeamIds = user.TeamIds,
                //        PhoneNumber = user.PhoneNumber,
                //        Active = user.Active,
                //        ExpirationDate = new DateTimeOffset(new DateTime(2025, 1, 10))
                //    }).Wait();
            }
        }

        [TestMethod]
        public void ListCheckmarxUsers()
        {
            AccessControlClient accessControlClient = clientV9.AC;

            var roles = accessControlClient.RolesAllAsync().Result.ToDictionary(x => x.Id);
            var teams = accessControlClient.TeamsAllAsync().Result.ToDictionary(x => x.Id);

            foreach (var user in accessControlClient.GetAllUsersDetailsAsync().Result)
            {
                if (user.Email.EndsWith("@checkmarx.com"))
                {
                    Trace.WriteLine(user.Email + string.Join(";", user.TeamIds.Select(x => teams[x].FullName)) +" " + user.LastLoginDate);

                    foreach (var role in user.RoleIds.Select(x => roles[x].Name))
                    {
                        Trace.WriteLine("+ " + role);
                    }
                }
            }
        }

        [TestMethod]
        public void ResetPasswordTest()
        {
            AccessControlClient accessControlClient = clientV93.AC;

            var userID = accessControlClient.GetAllUsersDetailsAsync().Result.First(x => x.UserName == "pedro.portilha@checkmarx.com").Id;

            var result = accessControlClient.ResetPassword2Async(userID).Result;

            Trace.WriteLine(result.GeneratedPassword);

            Assert.IsNotNull(result.GeneratedPassword);
        }


        [TestMethod]
        public void CreateUserTest()
        {
            AccessControlClient accessControlClient = clientV9.AC;

            ICollection<int> cxTamRoles = new int[] {
                accessControlClient.RolesAllAsync().Result.First(x => x.Name == "SAST Admin").Id
            };

            ICollection<int> cxTeamIds = new int[] {
                accessControlClient.TeamsAllAsync().Result.First(x => x.FullName == "/CxServer").Id
            };

            int localeID = accessControlClient.SystemLocalesAsync().Result.First(x => x.Code == "en-US").Id;

            CreateUserModel user = new CreateUserModel
            {
                FirstName = "firstname",
                LastName = "lastname",
                UserName = "email@checkmarx.com",
                Email = "email@checkmarx.com",
                Password = "randomPassword",
                ExpirationDate = DateTimeOffset.UtcNow + TimeSpan.FromDays(1000),
                Active = true,

                Country = "Portugal",
                JobTitle = "The World Greatest",

                AuthenticationProviderId = accessControlClient.AuthenticationProvidersAsync().Result.First(X => X.Name == "Application").Id, // Application User

                LocaleId = localeID,
                RoleIds = cxTamRoles,
                TeamIds = cxTeamIds,

            };

            accessControlClient.CreatesNewUser(user).Wait();
        }

        [TestMethod]
        public void ListTeamsTest()
        {
            AccessControlClient accessControlClient = clientV9.AC;
            foreach (var item in accessControlClient.TeamsAllAsync().Result)
            {
                Trace.WriteLine($"{item.Id} = {item.FullName}");
            }
        }

        [TestMethod]
        public void ListLocalsTest()
        {
            AccessControlClient accessControlClient = clientV9.AC;
            foreach (var item in accessControlClient.SystemLocalesAsync().Result)
            {
                Trace.WriteLine($"{item.Id} = {item.Code} = {item.DisplayName}");
            }
        }

        [TestMethod]
        public void ListAuthTest()
        {
            AccessControlClient accessControlClient = clientV93.AC;
            foreach (var item in accessControlClient.AuthenticationProvidersAsync().Result)
            {
                Trace.WriteLine($"{item.Id} = {item.Name} = {item.ProviderType}");
            }
        }



        [TestMethod]
        public void ListRolesTest()
        {
            AccessControlClient accessControlClient = clientV9.AC;
            foreach (var item in accessControlClient.RolesAllAsync().Result)
            {
                Trace.WriteLine($"{item.Id} = {item.Name}");
            }
        }
    }
}
