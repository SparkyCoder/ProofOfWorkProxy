using System.Threading;
using ProofOfWorkProxy.Connections.Listener;
using ProofOfWorkProxy.Managers;

namespace ProofOfWorkProxy.Proxy
{
    public class Proxy : IProxy
    {
        private readonly IProxyListener proxyListener;

        public Proxy(IProxyListener proxyListener)
        {
            this.proxyListener = proxyListener;
        }

        public void Start()
        {
            proxyListener.Listen();
        }
    }
}