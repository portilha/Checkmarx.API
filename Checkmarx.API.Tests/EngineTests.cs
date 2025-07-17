using Checkmarx.API.SAST;
using Humanizer;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using static Humanizer.ToQuantityExtensions;

namespace Checkmarx.API.Tests;

[TestClass]
public class EngineTests
{
    public static IConfigurationRoot Configuration { get; private set; }

    private static CxClient _sastClient;

    [ClassInitialize]
    public static void InitializeTest(TestContext testContext)
    {
        var builder = new ConfigurationBuilder()
           .AddUserSecrets<EngineTests>();

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
    public void ListEngines()
    {
        _sastClient.SASTClientV5.EngineServers_GetV5Async()
            .ContinueWith(task =>
            {
                Assert.IsTrue(task.Result.Count > 0, "No engines found");
                foreach (var engine in task.Result.GroupBy(x => x.Status.Value))
                {
                    Trace.WriteLine($"Engine Status: {engine.Key} - {engine.Count()}");
                    engine.OrderByDescending(x => x.MaxLoc).ToList().ForEach(e =>
                    {
                        string min = getFormatedValue(e.MinLoc);
                        string max = getFormatedValue(e.MaxLoc);

                        Trace.WriteLine($"\tEngine: {e.Id} = \"{e.Name}\" - {e.Status.Value} - MaxScans:{e.MaxScans} - [{min};{max}]");

                        //if (e.Dedications.Any())
                        //{
                        //    e.Dedications.ToList().ForEach(d =>
                        //                    {
                        //                        Trace.WriteLine($"\t\t + {d.ItemName}");
                        //                    }); 
                        //}

                    });


                }
            })
            .Wait();
    }

    public string getFormatedValue(long value)
    {
        if (value == 0)
            return value.ToString();
        else if (value < 1000000)
            return (value / 1000).ToString() + "k";
        else
            return (value / 1000000).ToString() + "M";
    }


    [TestMethod]
    public void ListQueueTest()
    {
        var task = _sastClient.GetScansQueue();

        Assert.IsTrue(task.Count > 0, "No scans in queue");
        foreach (var scan in task.GroupBy(x => x.Stage.Value))
        {
            Trace.WriteLine($"Scan Stage: {scan.Key} - {scan.Count()}");

            scan.ToList().ForEach(s =>
            {
                string text = scan.Key == "Scanning" ? $"Engine: {s.EngineId}" : $"Position: {s.QueuePosition} on {s.QueuedOn}";

                Trace.WriteLine($"\tScan ID: {s.Id} - {s.Project.Name} - {s.Stage.Value} - {text} - {s.Loc.ToString("N0")} - {s.Origin}");
            });
        }
    }

    [TestMethod]
    public void ScanPerEngineTest()
    {
        var task = _sastClient.GetScansQueue();

        var engines = _sastClient.SASTClientV5.EngineServers_GetV5Async().Result.ToDictionary(x => x.Id);

        Assert.IsTrue(task.Count > 0, "No scans in queue");

        IEnumerable<ScanQueue> scanPerGroup = task.Where(x => x.Stage.Value == "Scanning");

        var scanPerEngine = scanPerGroup.GroupBy(x => x.EngineId.Value).OrderBy(y => y.Key);

        Trace.WriteLine($"# Scans: {scanPerGroup.Count()}");

        foreach (var engine in scanPerEngine)
        {
            Trace.WriteLine($"Engine: \"{engines[engine.Key].Name}\" - [{engine.Count()}/{engines[engine.Key].MaxScans}]");

            engine.ToList().ForEach(s =>
            {
                Trace.WriteLine($"\tScan ID: {s.Id} - {s.Project.Name} - {s.Stage.Value} - {s.Loc.ToString("N0")} - {s.Origin}");
            });
        }


    }


    [TestMethod]
    public void GetCapacityTest()
    {
        // upper limit to number of concurrent scans
        Dictionary<long, int> capacities = new Dictionary<long, int>();


    }


    [TestMethod]
    public void SetMinConfigurationInEngineTest()
    {
        // Remove the lower limit to allow more engines to scan lower loc projects
        var engines = _sastClient.SASTClient.EngineServersV1_PatchByidengineServerAsync(1, new EngineServerPatchModel
        {
            MinLoc = 0
        }).Result;
    }


    [TestMethod]
    public void RestorePreviousConfigurationTest()
    {

    }


    [TestMethod]
    public void PostponeTest()
    {
        long scanID = 0; // Replace with a valid scan ID

        _sastClient.PortalSOAP.PostponeScanAsync(string.Empty, scanID.ToString()).Wait();
    }

}
