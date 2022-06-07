using System.Net.Sockets;

namespace ProofOfWorkProxy.Connections
{
    public class MinerConnection : BaseConnection
    {
        private readonly TcpClient minerClient;
        public MinerConnection(TcpClient minerClient)
        {
            this.minerClient = minerClient;
        }

        protected override TcpClient GetClient()
        {
            return minerClient;
        }
    }
}
