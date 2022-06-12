using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using ProofOfWorkProxy.Exceptions;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Managers
{
    public class StatisticsManager : IStatisticsManager
    {
        public ConcurrentDictionary<string, Statistics> MinerStatistics { get; }
        public long TotalCriticalErrorCount { get; private set; }
        public long TotalMinerDisconnectCount { get; private set; }

        public StatisticsManager()
        {
            MinerStatistics = new ConcurrentDictionary<string, Statistics>();
        }

        public void AddNewlyConnectedMiner(string minerId)
        {
            var addSucceeded = MinerStatistics.TryAdd(minerId, new Statistics());

            if (!addSucceeded)
                throw new CouldNotTakeActionOnCollectionException("TryAdd", "Statistics");
        }

        public Statistics GetCurrentStatistics(string minerId)
        {
            if (!DoesStatisticExist(minerId)) return null;

            var getSucceeded = MinerStatistics.TryGetValue(minerId, out var existingStatistics);

            if (!getSucceeded)
                throw new CouldNotTakeActionOnCollectionException("TryGetValue", "Statistics");

            return existingStatistics;
        }

        public void AddOrUpdateStatistics(string minerId, Statistics updateStatistics)
        {
            if (!DoesStatisticExist(minerId)) return;

            var currentStatistics = GetCurrentStatistics(minerId);

            var updateSucceeded = MinerStatistics.TryUpdate(minerId, updateStatistics, currentStatistics);

            if (!updateSucceeded)
                throw new CouldNotTakeActionOnCollectionException("TryUpdate", "Statistics");
        }

        public void RemoveMinerStatistics(string minerId)
        {
            if (!DoesStatisticExist(minerId)) return;

            var removalSucceeded = MinerStatistics.TryRemove(minerId, out _);

            if (!removalSucceeded)
                throw new CouldNotTakeActionOnCollectionException("TryRemove", $"{minerId} Statistics");
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
            var totalMinerDisconnectCount = TotalMinerDisconnectCount;
            Interlocked.Add(ref totalMinerDisconnectCount, 1);

            TotalMinerDisconnectCount = totalMinerDisconnectCount;
        }
    }
}
