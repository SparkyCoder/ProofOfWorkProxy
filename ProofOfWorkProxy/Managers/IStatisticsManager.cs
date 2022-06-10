using System.Collections.Concurrent;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Managers
{
    public interface IStatisticsManager
    {
        ConcurrentDictionary<string, Statistics> MinerStatistics { get; }
        void AddNewlyConnectedMiner(string minerId);
        Statistics GetCurrentStatistics(string minerId);
        void AddOrUpdateStatistics(string minerId, Statistics updateStatistics);
        void RemoveMinerStatistics(string minerId);
    }
}
