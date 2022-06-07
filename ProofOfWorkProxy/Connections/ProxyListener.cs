using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ProofOfWorkProxy.DataTransfer;
using ProofOfWorkProxy.Decorators;
using ProofOfWorkProxy.Extensions;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Connections
{
    public class ProxyListener : IProxyListener
    {
        private readonly IDataTransfer<MinerToPoolTransfer> minerToPoolTransfer;
        private readonly IDataTransfer<PoolToMinerTransfer> poolToMinerTransfer;

        public ProxyListener(IDataTransfer<MinerToPoolTransfer> minerToPoolTransfer, IDataTransfer<PoolToMinerTransfer> poolToMinerTransfer)
        {
            this.minerToPoolTransfer = minerToPoolTransfer;
            this.poolToMinerTransfer = poolToMinerTransfer;
        }

        public void Listen(Func<IConnection, string> welcomeMessage)
        {
            var proxyListener = CreateProxyListener();

            StartListeningForNewConnections(proxyListener);

            HandleNewConnections(proxyListener, welcomeMessage);
        }

        private static TcpListener CreateProxyListener()
        {
            var localIp = IPAddress.Loopback;
            var localPort = Settings.ProxyListeningPort;

            return new TcpListener(localIp, localPort);
        }

        private static void StartListeningForNewConnections(TcpListener proxyListener)
        {
            proxyListener.Start();
        }

        private void HandleNewConnections(TcpListener proxyListener, Func<IConnection, string> welcomeMessage)
        {
            while (!Environment.HasShutdownStarted)
            {
                $"Waiting for new mining connections on {proxyListener.LocalEndpoint.ToString()}...".Display(ConsoleColor.White);

                var minerClient = proxyListener.AcceptTcpClient();

                var minerConnection = GetDecoratedConnection(new MinerConnection(minerClient));
                var poolConnection = GetDecoratedConnection(new MinerPoolConnection());

                welcomeMessage(minerConnection).Display(ConsoleColor.DarkMagenta);

                QueueWork(() => minerToPoolTransfer.SendData(minerConnection, poolConnection));
                QueueWork(() => poolToMinerTransfer.SendData(minerConnection, poolConnection));
            }
        }

        private static IConnection GetDecoratedConnection(IConnection connection)
        {
            connection = connection.Initialize();
            return new ConnectionDecorator(connection);
        }

        private static void QueueWork(Action onNewMinerConnected)
        {
            ThreadPool.QueueUserWorkItem(state => onNewMinerConnected());
        }
    }
}
