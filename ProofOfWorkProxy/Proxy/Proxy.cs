using System.Threading;
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
            messageManager.StartTimerDisplayMessagesFromQueue();

            proxyListener.Listen();
        }
    }
}