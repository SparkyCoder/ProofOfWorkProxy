using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using ProofOfWorkProxy.Exceptions;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Managers
{
    public class StatisticsManager : IStatisticsManager
    {
        private readonly Settings settings;
        public ConcurrentDictionary<string, Statistics> MinerStatistics { get; }
        public long TotalCriticalErrorCount { get; private set; }
        public long TotalDisconnectCount { get; private set; }

        public StatisticsManager(Settings settings)
        {
            this.settings = settings;
            MinerStatistics = new ConcurrentDictionary<string, Statistics>();
        }

        public void AddNewlyConnectedMiner(string minerId)
        {
            var addSucceeded = MinerStatistics.TryAdd(minerId, new Statistics());

            if (!addSucceeded)
                throw new CouldNotTakeActionOnCollectionException("TryAdd", "Statistics", settings);
        }

        public Statistics GetCurrentStatistics(string minerId)
        {
            if (!DoesStatisticExist(minerId)) return null;

            var getSucceeded = MinerStatistics.TryGetValue(minerId, out var existingStatistics);

            if (!getSucceeded)
                throw new CouldNotTakeActionOnCollectionException("TryGetValue", "Statistics", settings);

            return existingStatistics;
        }

        public void AddOrUpdateStatistics(string minerId, Statistics updateStatistics)
        {
            if (!DoesStatisticExist(minerId)) return;

            var currentStatistics = GetCurrentStatistics(minerId);

            var updateSucceeded = MinerStatistics.TryUpdate(minerId, updateStatistics, currentStatistics);

            if (!updateSucceeded)
                throw new CouldNotTakeActionOnCollectionException("TryUpdate", "Statistics", settings);
        }

        public void RemoveMinerStatistics(string minerId)
        {
            if (!DoesStatisticExist(minerId)) return;

            var removalSucceeded = MinerStatistics.TryRemove(minerId, out _);

            if (!removalSucceeded)
                throw new CouldNotTakeActionOnCollectionException("TryRemove", $"{minerId} Statistics", settings);
        }

        private bool DoesStatisticExist(string minerId)
        {
            return MinerStatistics.Any(statistics => statistics.Key == minerId);
        }

        public void AddToTotalCriticalErrorCount()
        {
            var totalCriticalErrorCount = TotalCriticalErrorCount;
            Interlocked.Add(ref totalCriticalErrorCount, 1);

            TotalCriticalErrorCount = totalCriticalErrorCount;
        }

        public void AddToTotalDisconnectCount()
        {
            var totalMinerDisconnectCount = TotalDisconnectCount;
            Interlocked.Add(ref totalMinerDisconnectCount, 1);

            TotalDisconnectCount = totalMinerDisconnectCount;
        }
    }
}
