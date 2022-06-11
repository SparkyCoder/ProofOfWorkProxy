using System;

namespace ProofOfWorkProxy.Connections.Listener
{
    public interface IProxyListener : IDisposable
    {
        void Listen();
    }
}
