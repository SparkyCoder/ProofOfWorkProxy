using System;
using ProofOfWorkProxy.Connections;

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
            var welcomeMessage = GetWelcomeMessage();

            proxyListener.Listen(welcomeMessage);
        }

        private static Func<IConnection, string> GetWelcomeMessage()
        {
            return miner => $@"----- Welcome to the party miner {miner.Id} -----";
        }
    }
}