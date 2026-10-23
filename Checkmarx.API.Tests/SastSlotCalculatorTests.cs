using Checkmarx.API.SAST;
using Checkmarx.API.SASTV5;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Checkmarx.API.Tests;

[TestClass]
public class SastSlotCalculatorTests
{
    private static EngineServerResponse5Dto engine(long id, int minLoc, int maxLoc, int maxScans,
        EngineServerResponse5DtoStatus status = EngineServerResponse5DtoStatus.Idle, params EngineServerDedicationResponseDto[] dedications) => new()
        {
            Id = id,
            MinLoc = minLoc,
            MaxLoc = maxLoc,
            MaxScans = maxScans,
            Status = new Status { Value = status },
            Dedications = dedications
        };

    private static ScanQueue queued(long id, string stage, long loc = 0, long? engineId = null) => new()
    {
        Id = id,
        Stage = new Stage { Value = stage },
        Loc = loc,
        EngineId = engineId
    };

    [TestMethod]
    public void OnlyEnginesWithTheLocInRangeCount()
    {
        var engines = new[] { engine(1, 0, 100_000, 2), engine(2, 100_001, 1_000_000, 3) };

        var result = SastSlotCalculator.Calculate(engines, [], 1, null, 50_000, locMargin: 0);

        Assert.AreEqual(2, result.Capacity);
        CollectionAssert.AreEquivalent(new long[] { 1 }, result.EligibleEngineIds.ToArray());
    }

    [TestMethod]
    public void LocGrowthMarginIncludesTheNextEngine()
    {
        var engines = new[] { engine(1, 0, 100_000, 2), engine(2, 100_001, 1_000_000, 3) };

        // 90k + 25% = 112.5k reaches the second engine.
        var result = SastSlotCalculator.Calculate(engines, [], 1, null, 90_000);

        Assert.AreEqual(5, result.Capacity);
    }

    [TestMethod]
    public void OfflineEngineHasNoCapacityButIsStillPotential()
    {
        var engines = new[] { engine(1, 0, 100_000, 2, EngineServerResponse5DtoStatus.Offline) };

        var result = SastSlotCalculator.Calculate(engines, [], 1, null, 10_000);

        Assert.AreEqual(0, result.Capacity);
        Assert.AreEqual(2, result.PotentialCapacity);
        Assert.AreEqual(0, result.Free);
    }

    [TestMethod]
    public void LocBiggerThanEveryEngineHasNoPotentialCapacity()
    {
        var engines = new[] { engine(1, 0, 100_000, 2) };

        var result = SastSlotCalculator.Calculate(engines, [], 1, null, 500_000, locMargin: 0);

        Assert.AreEqual(0, result.PotentialCapacity);
    }

    [TestMethod]
    public void DedicatedEnginesOnlyAcceptTheirTeamsAndProjects()
    {
        var dedicated = engine(1, 0, 0, 2, EngineServerResponse5DtoStatus.Idle,
            new EngineServerDedicationResponseDto { ItemType = "Team", ItemId = 1, ItemName = "/CxServer/A" },
            new EngineServerDedicationResponseDto { ItemType = "Project", ItemId = 77 });

        Assert.AreEqual(2, SastSlotCalculator.Calculate([dedicated], [], 5, "/CxServer/A/Sub", 1000).Capacity);
        Assert.AreEqual(2, SastSlotCalculator.Calculate([dedicated], [], 77, "/CxServer/B", 1000).Capacity);
        Assert.AreEqual(0, SastSlotCalculator.Calculate([dedicated], [], 5, "/CxServer/B", 1000).Capacity);
    }

    [TestMethod]
    public void QueueScansTakeSlotsOnlyWhenTheyCompeteForTheEligibleEngines()
    {
        var engines = new[] { engine(1, 0, 100_000, 4), engine(2, 100_001, 1_000_000, 4) };

        var queue = new[]
        {
            queued(10, "Scanning", 50_000, engineId: 1),   // running on an eligible engine
            queued(11, "Scanning", 500_000, engineId: 2),  // running on another engine
            queued(12, "Queued", 20_000),                  // waiting, would land on engine 1
            queued(13, "Queued", 500_000),                 // waiting, would land on engine 2
            queued(14, "Finished", 50_000, engineId: 1),   // done
        };

        var result = SastSlotCalculator.Calculate(engines, queue, 1, null, 50_000, reserve: 1, locMargin: 0);

        Assert.AreEqual(4, result.Capacity);
        Assert.AreEqual(2, result.InUse);
        Assert.AreEqual(1, result.Free);
    }

    [TestMethod]
    public void ReserveNeverTakesTheLastSlot()
    {
        var engines = new[] { engine(1, 0, 0, 1) };

        var result = SastSlotCalculator.Calculate(engines, [], 1, null, 10, reserve: 1);

        Assert.AreEqual(1, result.Free);
    }

    [TestMethod]
    public void OneEngineWithThreeSlotsAndReserveOneAllowsTwo()
    {
        var engines = new[] { engine(1, 0, 0, 3) };

        Assert.AreEqual(2, SastSlotCalculator.Calculate(engines, [], 1, null, 10, reserve: 1).Free);
        Assert.AreEqual(1, SastSlotCalculator.Calculate(engines, [queued(1, "Scanning", 10, 1)], 1, null, 10, reserve: 1).Free);
    }

    [TestMethod]
    public void AFullEngineDoesNotBlockAProjectThatFitsAnotherEngine()
    {
        // Engine 1 takes up to 100k LoC and is full (3 running + 2 waiting from other users), engine 2 takes more and is empty.
        var engines = new[] { engine(1, 0, 100_000, 3), engine(2, 100_001, 1_000_000, 3) };

        var queue = new[]
        {
            queued(1, "Scanning", 10_000, engineId: 1),
            queued(2, "Scanning", 20_000, engineId: 1),
            queued(3, "Scanning", 30_000, engineId: 1),
            queued(4, "Queued", 15_000),
            queued(5, "Queued", 25_000),
        };

        var small = SastSlotCalculator.Calculate(engines, queue, 1, null, 10_000, reserve: 1, locMargin: 0);
        var big = SastSlotCalculator.Calculate(engines, queue, 2, null, 500_000, reserve: 1, locMargin: 0);

        Assert.AreEqual(0, small.Free);
        Assert.AreEqual(2, big.Free);
    }

    [TestMethod]
    public void UsersWaitingScansTakeTheSlotsBeforeTheRunSubmitsAnything()
    {
        // 1 engine with 3 slots: our scan is running and 50 users' scans arrive and wait.
        var engines = new[] { engine(1, 0, 0, 3) };
        var queue = new List<ScanQueue> { queued(1, "Scanning", 10_000, engineId: 1) };
        for (int i = 0; i < 50; i++)
            queue.Add(queued(100 + i, "Queued", 10_000));

        Assert.AreEqual(0, SastSlotCalculator.Calculate(engines, queue, 1, null, 10_000, reserve: 1).Free);

        // The run submits again only once the users' queue has drained below the free capacity.
        var drained = queue.Take(1).ToList();
        Assert.AreEqual(1, SastSlotCalculator.Calculate(engines, drained, 1, null, 10_000, reserve: 1).Free);
    }

    [TestMethod]
    public void EnginesWithOverlappingRangesAreOneGroup()
    {
        var engines = new[]
        {
            engine(1, 0, 100_000, 3),
            engine(2, 50_000, 300_000, 2),            // overlaps the first: same pool
            engine(3, 400_000, 0, 2),                 // no upper limit, separate pool
            engine(4, 0, 0, 5, EngineServerResponse5DtoStatus.Offline),   // offline: no slots now
        };

        var groups = SastSlotCalculator.GetEngineGroups(engines);

        Assert.AreEqual(2, groups.Count);
        Assert.AreEqual(5, groups[0].Capacity);
        Assert.AreEqual(2, groups[0].EngineNames.Count);
        Assert.AreEqual(300_000, groups[0].MaxLoc);
        Assert.AreEqual(2, groups[1].Capacity);
        Assert.IsNull(groups[1].MaxLoc);
    }

    [TestMethod]
    public void ADedicatedEngineIsAGroupOnItsOwn()
    {
        var engines = new[]
        {
            engine(1, 0, 0, 4),
            engine(2, 0, 0, 2, EngineServerResponse5DtoStatus.Idle, new EngineServerDedicationResponseDto { ItemType = "Team", ItemId = 1, ItemName = "/CxServer/A" }),
        };

        var groups = SastSlotCalculator.GetEngineGroups(engines);

        Assert.AreEqual(2, groups.Count);
        Assert.AreEqual(1, groups.Count(x => x.Dedicated));
        Assert.AreEqual(2, groups.Single(x => x.Dedicated).Capacity);
        Assert.AreEqual(4, groups.Single(x => !x.Dedicated).Capacity);
    }

    [TestMethod]
    public void OnlyScansOfOthersThatAreWaitingCount()
    {
        var queue = new[]
        {
            queued(1, "Queued", 10),                  // ours, waiting
            queued(2, "Scanning", 10, engineId: 1),   // ours, running
            queued(3, "Queued", 10),                  // other, waiting
            queued(4, "New", 10),                     // other, waiting
            queued(5, "Scanning", 10, engineId: 1),   // other, running: nothing to yield to
            queued(6, "PreScan", 10, engineId: 1),    // other, running
            queued(7, "Finished", 10, engineId: 1),   // other, done
            queued(8, "Canceled", 10),                // other, done
        };

        var waiting = SastSlotCalculator.GetWaitingScansOfOthers(queue, new HashSet<long> { 1, 2 });

        CollectionAssert.AreEquivalent(new long[] { 3, 4 }, waiting.Select(x => x.Id).ToArray());
    }

    [TestMethod]
    public void NoScansOfOthersWaitingMeansNothingToYieldTo()
    {
        var queue = new[] { queued(1, "Queued", 10), queued(2, "Scanning", 10, engineId: 1) };

        Assert.AreEqual(0, SastSlotCalculator.GetWaitingScansOfOthers(queue, new HashSet<long> { 1, 2 }).Count);
    }

    [TestMethod]
    public void FreeSlotsAreNeverNegative()
    {
        var engines = new[] { engine(1, 0, 0, 1) };
        var queue = new[] { queued(1, "Scanning", 10, 1), queued(2, "Queued", 10) };

        var result = SastSlotCalculator.Calculate(engines, queue, 1, null, 10, reserve: 2);

        Assert.AreEqual(0, result.Free);
    }
}
