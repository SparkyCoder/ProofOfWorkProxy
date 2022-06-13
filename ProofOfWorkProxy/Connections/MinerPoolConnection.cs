using System.Net.Sockets;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Connections
{
    public class MinerPoolConnection : BaseConnection
    {
        private readonly Settings settings;

        public MinerPoolConnection(Settings settings)
        {
            this.settings = settings;
        }

        protected override TcpClient GetClient()
        {
            return new TcpClient(settings.MiningPool.Domain, settings.MiningPool.Port);
        }
    }
}
