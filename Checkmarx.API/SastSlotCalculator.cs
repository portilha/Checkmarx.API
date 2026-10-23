using Checkmarx.API.SAST;
using Checkmarx.API.SASTV5;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkmarx.API
{
    /// <summary>
    /// Result of a slot calculation for a given project / LoC.
    /// </summary>
    public class SlotAvailability
    {
        /// <summary>Engines that can take the scan (online, LoC range and dedications match).</summary>
        public IReadOnlyCollection<long> EligibleEngineIds { get; init; } = Array.Empty<long>();

        /// <summary>Sum of the MaxScans of the eligible engines.</summary>
        public int Capacity { get; init; }

        /// <summary>
        /// Same as <see cref="Capacity"/> but ignoring whether the engines are online right now.
        /// Zero means that no engine will ever take this scan (LoC out of every range or dedicated to others).
        /// </summary>
        public int PotentialCapacity { get; init; }

        /// <summary>Non-terminal scans in the queue that compete for the eligible engines.</summary>
        public int InUse { get; init; }

        /// <summary>Slots held back for other users' scans (at most Capacity - 1).</summary>
        public int Reserve { get; init; }

        /// <summary>Slots that can be used right now. Never negative.</summary>
        public int Free => Math.Max(0, Capacity - InUse - Reserve);
    }

    /// <summary>
    /// Engines that share the scans of a LoC range: their slots are one pool. A dedicated engine is a group on its own.
    /// </summary>
    public class EngineGroup
    {
        public long MinLoc { get; init; }

        /// <summary>Null when there is no upper limit.</summary>
        public long? MaxLoc { get; init; }

        public IReadOnlyList<string> EngineNames { get; init; } = Array.Empty<string>();

        public int Capacity { get; init; }

        public bool Dedicated { get; init; }

        public override string ToString()
        {
            string names = string.Join(", ", EngineNames);

            return Dedicated
                ? $"dedicated engine {names}"
                : $"engine(s) {names} for {MinLoc:N0} to {(MaxLoc.HasValue ? MaxLoc.Value.ToString("N0") : "no limit of")} LoC";
        }
    }

    /// <summary>
    /// Works out how many scan slots are free for a project, based on the SAST engines and the current queue.
    /// SAST does not let the caller pick the engine server, so this is a capacity estimate used to throttle
    /// how many scans we trigger, not a routing decision.
    /// </summary>
    public static class SastSlotCalculator
    {
        /// <summary>
        /// The LoC of a project can grow since its last scan and end up in a different engine,
        /// so the eligible engines cover the range [loc, loc * (1 + margin)].
        /// </summary>
        public const double DefaultLocMargin = 0.25;

        private static readonly HashSet<string> _terminalStages = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            SAST.Status.Finished, SAST.Status.Canceled, SAST.Status.Failed, "Deleted"
        };

        private static readonly HashSet<EngineServerResponse5DtoStatus> _unavailableEngineStatus = new HashSet<EngineServerResponse5DtoStatus>
        {
            EngineServerResponse5DtoStatus.Offline, EngineServerResponse5DtoStatus.Blocked, EngineServerResponse5DtoStatus.ScanningAndBlocked
        };

        // Stages where the scan is already executing on an engine: it can no longer be postponed.
        private static readonly HashSet<string> _runningStages = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Scanning", "SourcePulling", "SourcePullingAndDeployment", "PreScan", "PostScan", "Generating Report"
        };

        public static bool IsTerminalStage(string stage) => stage != null && _terminalStages.Contains(stage);

        public static bool IsRunningStage(string stage) => stage != null && _runningStages.Contains(stage);

        /// <summary>
        /// The scans in the queue that were not triggered by us and have not started yet, so they are waiting for an engine.
        /// Ours are told apart by scan id, not by origin: another tool or job may use the same origin.
        /// </summary>
        /// <param name="queue">Current queue snapshot (all origins).</param>
        /// <param name="ourScanIds">Ids of the scans triggered by this run.</param>
        public static IReadOnlyList<ScanQueue> GetWaitingScansOfOthers(IEnumerable<ScanQueue> queue, ISet<long> ourScanIds)
        {
            if (queue == null)
                throw new ArgumentNullException(nameof(queue));

            if (ourScanIds == null)
                throw new ArgumentNullException(nameof(ourScanIds));

            return queue
                .Where(x => !ourScanIds.Contains(x.Id) && !IsTerminalStage(x.Stage?.Value) && !IsRunningStage(x.Stage?.Value))
                .ToList();
        }

        /// <param name="engines">Engine servers of the SAST instance that will run the scan.</param>
        /// <param name="queue">Current queue snapshot (all origins). Scans triggered after the snapshot can be added as synthetic entries.</param>
        /// <param name="projectId">Project being scanned (dedications by project).</param>
        /// <param name="teamName">Full team name of the project (dedications by team).</param>
        /// <param name="loc">Lines of code of the project's last scan.</param>
        /// <param name="reserve">Slots to keep free for other users.</param>
        /// <param name="locMargin">Growth tolerance on <paramref name="loc"/>.</param>
        public static SlotAvailability Calculate(
            IEnumerable<EngineServerResponse5Dto> engines,
            IEnumerable<ScanQueue> queue,
            long projectId,
            string teamName,
            long loc,
            int reserve = 0,
            double locMargin = DefaultLocMargin)
        {
            if (engines == null)
                throw new ArgumentNullException(nameof(engines));

            long maxLoc = (long)Math.Ceiling(loc * (1 + Math.Max(0, locMargin)));

            var candidates = engines
                .Where(e => locOverlaps(e, loc, maxLoc) && isAllowed(e, projectId, teamName))
                .ToArray();

            var eligible = candidates.Where(IsOnline).ToArray();

            var eligibleIds = eligible.Select(e => e.Id).ToHashSet();
            int capacity = eligible.Sum(e => Math.Max(0, e.MaxScans));

            int inUse = 0;
            if (queue != null)
            {
                foreach (var scan in queue)
                {
                    if (IsTerminalStage(scan.Stage?.Value))
                        continue;

                    if (scan.EngineId.HasValue && scan.EngineId.Value > 0)
                    {
                        // Already on an engine: it only counts against that engine.
                        if (eligibleIds.Contains(scan.EngineId.Value))
                            inUse++;
                    }
                    else if (scan.Loc <= 0 || eligible.Any(e => locFits(e, scan.Loc)))
                    {
                        // Waiting to be assigned: it competes for any engine it could land on.
                        inUse++;
                    }
                }
            }

            return new SlotAvailability
            {
                EligibleEngineIds = eligibleIds,
                Capacity = capacity,
                PotentialCapacity = candidates.Sum(e => Math.Max(0, e.MaxScans)),
                InUse = inUse,
                // Never reserve everything: with one slot in total the scan would wait forever.
                Reserve = Math.Min(Math.Max(0, reserve), Math.Max(0, capacity - 1))
            };
        }

        public static bool IsOnline(EngineServerResponse5Dto engine)
        {
            // Engines coming from the legacy API may not report a status.
            return engine.Status == null || !_unavailableEngineStatus.Contains(engine.Status.Value);
        }

        /// <summary>
        /// The pools of slots of the online engines: the engines whose LoC ranges overlap share one, and a dedicated engine has its own.
        /// Used to tell the user what a number of slots to keep free means in each pool.
        /// </summary>
        public static IReadOnlyList<EngineGroup> GetEngineGroups(IEnumerable<EngineServerResponse5Dto> engines)
        {
            if (engines == null)
                throw new ArgumentNullException(nameof(engines));

            var online = engines.Where(e => IsOnline(e) && e.MaxScans > 0).ToList();
            var groups = new List<EngineGroup>();

            // Dedicated engines only take their projects and teams, so their slots are not shared with the others.
            foreach (var engine in online.Where(e => e.Dedications?.Any(d => d.IsDeprecated != true) == true))
            {
                groups.Add(new EngineGroup
                {
                    MinLoc = engine.MinLoc,
                    MaxLoc = upper(engine) == long.MaxValue ? null : upper(engine),
                    EngineNames = [engine.Name],
                    Capacity = engine.MaxScans,
                    Dedicated = true
                });
            }

            // The others: ranges that overlap are one pool.
            var open = online.Where(e => e.Dedications?.Any(d => d.IsDeprecated != true) != true).OrderBy(e => e.MinLoc).ToList();

            var names = new List<string>();
            long groupMin = 0, groupMax = 0;
            int capacity = 0;

            void close()
            {
                if (names.Count > 0)
                    groups.Add(new EngineGroup { MinLoc = groupMin, MaxLoc = groupMax == long.MaxValue ? null : groupMax, EngineNames = names.ToArray(), Capacity = capacity });

                names = new List<string>();
                capacity = 0;
            }

            foreach (var engine in open)
            {
                if (names.Count > 0 && engine.MinLoc > groupMax)
                    close();

                if (names.Count == 0)
                {
                    groupMin = engine.MinLoc;
                    groupMax = upper(engine);
                }

                groupMax = Math.Max(groupMax, upper(engine));
                names.Add(engine.Name);
                capacity += engine.MaxScans;
            }

            close();

            return groups;
        }

        // A MaxLoc <= 0 is taken as "no upper limit".
        private static long upper(EngineServerResponse5Dto e) => e.MaxLoc <= 0 ? long.MaxValue : e.MaxLoc;

        private static bool locFits(EngineServerResponse5Dto e, long loc) => loc >= e.MinLoc && loc <= upper(e);

        private static bool locOverlaps(EngineServerResponse5Dto e, long minLoc, long maxLoc) => minLoc <= upper(e) && maxLoc >= e.MinLoc;

        private static bool isAllowed(EngineServerResponse5Dto engine, long projectId, string teamName)
        {
            var dedications = engine.Dedications?.Where(d => d.IsDeprecated != true).ToArray();

            // Not dedicated: open to everyone.
            if (dedications == null || dedications.Length == 0)
                return true;

            return dedications.Any(d =>
                (string.Equals(d.ItemType, "Project", StringComparison.OrdinalIgnoreCase) && d.ItemId == projectId) ||
                (string.Equals(d.ItemType, "Team", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(teamName) && !string.IsNullOrEmpty(d.ItemName) &&
                 teamName.StartsWith(d.ItemName, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
