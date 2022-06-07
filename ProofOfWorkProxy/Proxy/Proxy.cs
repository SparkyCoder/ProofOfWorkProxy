using System;
using System.Threading;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Connections.Listener;
using ProofOfWorkProxy.Managers;

namespace ProofOfWorkProxy.Proxy
{
    public class Proxy : IProxy
    {
        private readonly IProxyListener proxyListener;
        private readonly IMessageManager messageManager;

        public Proxy(IProxyListener proxyListener, IMessageManager messageManager)
        {
            this.proxyListener = proxyListener;
            this.messageManager = messageManager;
        }

        public void Start()
        {
            var welcomeMessage = GetWelcomeMessage();

            StartMessageListener();

            proxyListener.Listen(welcomeMessage);
        }

        private static Func<IConnection, string> GetWelcomeMessage()
        {
            return miner => $@"----- Welcome to the party miner {miner.Id} -----";
        }

        private void StartMessageListener()
        {
            ThreadPool.QueueUserWorkItem(state => messageManager.StartTimerDisplayMessagesFromQueue());
        }
    }
}