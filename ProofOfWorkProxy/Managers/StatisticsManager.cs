using System;
using System.Collections.Concurrent;
using System.Linq;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Managers
{
    public class StatisticsManager : IStatisticsManager
    {
        public ConcurrentDictionary<string, Statistics> MinerStatistics { get; }

        public StatisticsManager()
        {
            this.MinerStatistics = new ConcurrentDictionary<string, Statistics>();
        }

        public void AddNewlyConnectedMiner(string minerId)
        {
            var addSucceeded = MinerStatistics.TryAdd(minerId, new Statistics());

            if(!addSucceeded)
                throw new Exception("Adding Statistics For New Miner Failed!");
        }

        public Statistics GetCurrentStatistics(string minerId)
        {
            if (!DoesStatisticExist(minerId)) return null;

            var getSucceeded = MinerStatistics.TryGetValue(minerId, out var existingStatistics);

            if (!getSucceeded)
                throw new Exception("Updating Statistics Failed!");

            return existingStatistics;
        }

        public void AddOrUpdateStatistics(string minerId, Statistics updateStatistics)
        {
            if (!DoesStatisticExist(minerId)) return;

            var currentStatistics = GetCurrentStatistics(minerId);

            var updateSucceeded = MinerStatistics.TryUpdate(minerId, updateStatistics, currentStatistics);

            if (!updateSucceeded)
                throw new Exception("Updating Statistics Failed!");
        }

        public void RemoveMinerStatistics(string minerId)
        {
            if (!DoesStatisticExist(minerId)) return;

            var removalSucceeded = MinerStatistics.TryRemove(minerId, out _);

            if (!removalSucceeded)
                throw new Exception($"Could not remove statistics for  miner {minerId}.");
        }

        private bool DoesStatisticExist(string minerId)
        {
            return MinerStatistics.Any(statistics => statistics.Key == minerId);
            ;
        }
    }
}
