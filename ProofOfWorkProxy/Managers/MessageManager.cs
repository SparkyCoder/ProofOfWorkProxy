using System;
using System.Collections.Concurrent;
using System.Threading;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Managers
{
    public class MessageManager : IMessageManager
    {
        private readonly IStatisticsManager statisticsManager;
        private readonly ConcurrentQueue<ConsoleMessage> messageQueue;

        public MessageManager(IStatisticsManager statisticsManager)
        {
            this.statisticsManager = statisticsManager;
            messageQueue = new ConcurrentQueue<ConsoleMessage>();
        }

        public void AddMessage(ConsoleMessage message)
        {
            messageQueue.Enqueue(message);
        }

        public void StartTimerDisplayMessagesFromQueue()
        {
            new Timer(state => { Display(); }, null, 0, 1000);
        }

        private void Display()
        {
            if (Settings.DebugOn)
            {
                IterateThroughQueuedDebugMessages();
            }
            else
            {
                Console.Clear();
                new ConsoleMessage(Settings.ApplicationTitle).DisplayMessage();
                DisplayStatistics();
            }
        }

        private void IterateThroughQueuedDebugMessages()
        {
            for (var messageIndex = 0; messageIndex < messageQueue.Count; messageIndex++)
            {
                DisplayMessage();
            }
        }

        private void DisplayStatistics()
        {
            if (statisticsManager.MinerStatistics.Count == 0)
            {
                new ConsoleMessage("Waiting for connections......").DisplayMessage();
            }

            foreach (var (minerId, minerStatistics) in statisticsManager.MinerStatistics)
            {
                new ConsoleMessage(
                        $"======================================================================================================== {Environment.NewLine}| Miner: {minerId} {Environment.NewLine}| Submitted Shares: {minerStatistics.SharesSubmitted} {Environment.NewLine}| Requests: {minerStatistics.Requests} {Environment.NewLine}| Responses: {minerStatistics.Responses} {Environment.NewLine}| Errors: {minerStatistics.Errors} {Environment.NewLine}| Last Updated: {minerStatistics.LastUpdated} {Environment.NewLine}========================================================================================================")
                    .DisplayMessage();
            }
        }

        private void DisplayMessage()
        {
            messageQueue.TryDequeue(out var message);

            message ??= new ConsoleMessage("Could not access pending message queue.", ConsoleColor.Red);

            message.DisplayMessage();
        }
    }
}