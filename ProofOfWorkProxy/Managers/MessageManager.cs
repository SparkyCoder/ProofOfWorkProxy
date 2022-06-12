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
        private readonly object lockObject = new object();

        public MessageManager(IStatisticsManager statisticsManager)
        {
            this.statisticsManager = statisticsManager;
            messageQueue = new ConcurrentQueue<ConsoleMessage>();
        }

        public void AddMessage(ConsoleMessage message)
        {
            lock (lockObject)
            {
                messageQueue.Enqueue(message);
            }
        }

        public void StartTimerDisplayMessagesFromQueue()
        {
            lock (lockObject)
            {
                StartTimer();
            }
        }

        private void StartTimer()
        {
            TimerCallback callback = (state) => { DisplayOrError(); };

            messageTimer = new Timer(callback, null, 0, 1000);
        }

        private void DisplayOrError()
        {
            try
            {
                Display();
            }
            catch (Exception ex)
            {
                DisplayCriticalError(ex.Message);
            }
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
            new ConsoleMessage(Settings.ApplicationTitle).DisplayMessage(addNewLine:false);
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
            DisplayApplicationStats();

            if (statisticsManager.MinerStatistics.Count == 0)
                new ConsoleMessage("Waiting for connections......").DisplayMessage();

            foreach (var (minerId, minerStatistics) in statisticsManager.MinerStatistics)
            {
                new ConsoleMessage(
                        $"======================================================================================================== {Environment.NewLine}| Miner: {minerId}   Connected: {minerStatistics.ConnectedDateTime}    Last Updated: {minerStatistics.LastUpdated} {Environment.NewLine}| Submitted Shares: {minerStatistics.SharesSubmitted.Count}   Shares Accepted: {minerStatistics.GetAcceptedShares()} {Environment.NewLine}| Requests: {minerStatistics.Requests}   Responses: {minerStatistics.Responses}   Errors: {minerStatistics.Errors} {Environment.NewLine}========================================================================================================")
                    .DisplayMessage(addNewLine:false);
            }
        }

        private void DisplayApplicationStats()
        {
            new ConsoleMessage($"Critical Errors: {statisticsManager.TotalCriticalErrorCount}").DisplayMessage(addNewLine:false);
            new ConsoleMessage($"Total Miner Disconnects: {statisticsManager.TotalMinerDisconnectCount}").DisplayMessage();
        }

        private void DisplayMessage()
        {
            messageQueue.TryDequeue(out var message);

            if (message == null)
                DisplayCriticalError(new CouldNotTakeActionOnCollectionException("TryDequeue", "Message").Message);

            message?.DisplayMessage();
        }

        public void DisplayCriticalError(string criticalMessage)
        {
            lock (lockObject)
            {
                statisticsManager.AddToTotalCriticalErrorCount();

                messageTimer?.Dispose();

                Task.Delay(500)
                    .ContinueWith(_ =>
                    {
                        ShowTitleOnClearedScreen();
                        new ConsoleMessage(criticalMessage, ConsoleColor.Red).DisplayMessage();
                    }).Wait();

                var delay = TimeSpan.FromSeconds(Settings.ErrorMessageDisplayTime);
                Thread.Sleep(delay);
                StartTimer();
            }
        }
    }
}