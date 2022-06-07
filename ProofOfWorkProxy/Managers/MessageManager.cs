using System;
using System.Collections.Concurrent;
using System.Threading;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Managers
{
    public class MessageManager : IMessageManager
    {
        private readonly ConcurrentQueue<ConsoleMessage> messageQueue;

        public MessageManager()
        {
            messageQueue = new ConcurrentQueue<ConsoleMessage>();
        }

        public void AddMessage(ConsoleMessage message)
        {
            messageQueue.Enqueue(message);
        }

        public void StartTimerDisplayMessagesFromQueue()
        {
            new Timer(state => { IterateThroughCurrentQueue(); }, null, 0, 500);
        }

        private void IterateThroughCurrentQueue()
        {
            for (var messageIndex = 0; messageIndex < messageQueue.Count; messageIndex++)
            {
                DisplayMessage();
            }
        }

        private void DisplayMessage()
        {
            messageQueue.TryDequeue(out var message);

            message ??= new ConsoleMessage($"Could not access pending message queue.", ConsoleColor.Red);

            message.DisplayMessage();
        }
    }
}
