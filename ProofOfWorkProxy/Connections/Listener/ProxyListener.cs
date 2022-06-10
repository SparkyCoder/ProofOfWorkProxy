using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ProofOfWorkProxy.DataTransfer;
using ProofOfWorkProxy.Decorators;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Connections.Listener
{
    public class ProxyListener : IProxyListener
    {
        private readonly IDataTransfer<MinerToPoolTransfer> minerToPoolTransfer;
        private readonly IDataTransfer<PoolToMinerTransfer> poolToMinerTransfer;
        private readonly IMessageManager messageManager;
        private readonly IStatisticsManager statisticsManager;

        public ProxyListener(IDataTransfer<MinerToPoolTransfer> minerToPoolTransfer, IDataTransfer<PoolToMinerTransfer> poolToMinerTransfer, IMessageManager messageManager, IStatisticsManager statisticsManager)
        {
            this.minerToPoolTransfer = minerToPoolTransfer;
            this.poolToMinerTransfer = poolToMinerTransfer;
            this.messageManager = messageManager;
            this.statisticsManager = statisticsManager;
        }

        public void Listen()
        {
            var proxyListener = CreateProxyListener();

            StartListeningForNewConnections(proxyListener);

            HandleNewConnections(proxyListener);
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

        private void HandleNewConnections(TcpListener proxyListener)
        {
            while (!Environment.HasShutdownStarted)
            {
                if (Settings.DebugOn)
                {
                    var waitingForConnectionsMessage =
                        new ConsoleMessage($"Waiting for new mining connections on {proxyListener.LocalEndpoint}...");
                    messageManager.AddMessage(waitingForConnectionsMessage);
                }

                var minerClient = proxyListener.AcceptTcpClient();

                var minerConnection = GetDecoratedConnection(new MinerConnection(minerClient));
                var poolConnection = GetDecoratedConnection(new MinerPoolConnection());

                if (Settings.DebugOn)
                {
                    var newMinerMessage = new ConsoleMessage($"Miner {minerConnection.Id} connected.");
                    messageManager.AddMessage(newMinerMessage);
                }

                statisticsManager.AddNewlyConnectedMiner(minerConnection.Id);

                QueueWork(() => minerToPoolTransfer.SendData(minerConnection, poolConnection));
                QueueWork(() => poolToMinerTransfer.SendData(minerConnection, poolConnection));
            }
        }

        private IConnection GetDecoratedConnection(IConnection connection)
        {
            connection = connection.Initialize();
            return new ConnectionDecorator(connection, messageManager, statisticsManager);
        }

        private static void QueueWork(Action onNewMinerConnected)
        {
            ThreadPool.QueueUserWorkItem(state => onNewMinerConnected());
        }
    }
}
