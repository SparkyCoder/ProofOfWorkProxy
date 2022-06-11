using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using ProofOfWorkProxy.Exceptions;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Managers
{
    public class MessageManager : IMessageManager
    {
        private readonly IStatisticsManager statisticsManager;
        private readonly ConcurrentQueue<ConsoleMessage> messageQueue;
        private Timer messageTimer;

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
            messageTimer = new Timer(state => { Display(); }, null, 0, 1000);
        }

        private void Display()
        {
            if (Settings.DebugOn)
            {
                IterateThroughQueuedDebugMessages();
            }
            else
            {
                ShowTitleOnClearedScreen();
                DisplayStatistics();
            }
        }

        private static void ShowTitleOnClearedScreen()
        {
            Console.Clear();
            new ConsoleMessage(Settings.ApplicationTitle).DisplayMessage();
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

            if (message == null)
                DisplayCriticalError(new CouldNotTakeActionOnCollectionException("TryDequeue", "Message").Message);
            
            message.DisplayMessage();
        }

        public void DisplayCriticalError(string criticalMessage)
        {
            messageTimer.Dispose();

                Task.Delay(500)
                    .ContinueWith(_ =>
                    {
                        ShowTitleOnClearedScreen();
                        new ConsoleMessage(criticalMessage, ConsoleColor.Red).DisplayMessage();
                    }).Wait();
        }
    }
}