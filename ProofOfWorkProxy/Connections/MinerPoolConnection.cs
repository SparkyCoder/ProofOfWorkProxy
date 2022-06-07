using System.Net.Sockets;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Connections
{
    public class MinerPoolConnection : BaseConnection
    {
        protected override TcpClient GetClient()
        {
            return new TcpClient(Settings.MiningPoolDomain, Settings.MiningPoolPort);
        }
    }
}
