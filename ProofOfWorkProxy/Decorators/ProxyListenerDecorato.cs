using System;
using System.Threading;
using Polly;
using Polly.Retry;
using ProofOfWorkProxy.Connections.Listener;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Decorators
{
    public class ProxyCriticalExceptionDecorator : IProxyListener
    {
        private int attemptNumber;
        private readonly RetryPolicy retryPolicy;
        private readonly IMessageManager messageManager;
        private readonly IProxyListener wrappedProxyListener;

        public ProxyCriticalExceptionDecorator(IMessageManager messageManager, IProxyListener wrappedProxyListener)
        {
            retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryForever(attempt =>
                {
                    attemptNumber = attempt;
                    return TimeSpan.FromSeconds(Settings.RetryDelayInSecondsForWhenInternetGoesDown);
                });
            this.messageManager = messageManager;
            this.wrappedProxyListener = wrappedProxyListener;
        }

        public void Listen()
        {
            retryPolicy.Execute(() =>
            {
                try
                {
                    StartListening();
                }
                catch(Exception exception)
                {
                    HandleException(exception);
                }
            });
        }

        private void StartListening()
        {
            StartMessageListener();

            wrappedProxyListener.Listen();
        }

        private void HandleException(Exception exception)
        {
            Dispose();

            var criticalMessage = exception.Message;

            criticalMessage += $"{Environment.NewLine} {Environment.NewLine} Retry attempt: {attemptNumber}.";

            messageManager.DisplayCriticalError(criticalMessage);

            throw exception;
        }

        private void StartMessageListener()
        {
            ThreadPool.QueueUserWorkItem(state => messageManager.StartTimerDisplayMessagesFromQueue());
        }

        public void Dispose()
        {
            wrappedProxyListener.Dispose();
        }
    }
}
