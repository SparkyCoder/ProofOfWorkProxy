using System;

namespace ProofOfWorkProxy.Connections
{
    public interface IProxyListener
    {
        void Listen(Func<IConnection, string> welcomeMessage);
    }
}
