using System;
using System.Collections.Concurrent;
using Polly;
using Polly.Retry;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Decorators
{
    public class StatisticsManagerRetryDecorator : IStatisticsManager
    {
        private readonly IStatisticsManager wrappedStatisticsManager;
        private readonly RetryPolicy retryPolicy;
        public ConcurrentDictionary<string, Statistics> MinerStatistics => wrappedStatisticsManager.MinerStatistics;

        public StatisticsManagerRetryDecorator(IStatisticsManager wrappedStatisticsManager)
        {
            retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(10, attempt => TimeSpan.FromSeconds(5 * attempt));
            this.wrappedStatisticsManager = wrappedStatisticsManager;
        }

        public void AddNewlyConnectedMiner(string minerId)
        {
            retryPolicy.Execute(() => { wrappedStatisticsManager.AddNewlyConnectedMiner(minerId); });
        }

        public Statistics GetCurrentStatistics(string minerId)
        {
            return retryPolicy.Execute(() => wrappedStatisticsManager.GetCurrentStatistics(minerId));
        }

        public void AddOrUpdateStatistics(string minerId, Statistics updateStatistics)
        {
            retryPolicy.Execute(() => { wrappedStatisticsManager.AddOrUpdateStatistics(minerId, updateStatistics); });

        }

        public void RemoveMinerStatistics(string minerId)
        {
            retryPolicy.Execute(() => { wrappedStatisticsManager.RemoveMinerStatistics(minerId); });
        }
    }
}
