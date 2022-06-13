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
        private readonly Settings settings;
        private TcpListener proxyListener;
        private IConnection minerConnection;
        private IConnection poolConnection;

        public ProxyListener(IDataTransfer<MinerToPoolTransfer> minerToPoolTransfer, IDataTransfer<PoolToMinerTransfer> poolToMinerTransfer, IMessageManager messageManager, IStatisticsManager statisticsManager, Settings settings)
        {
            this.minerToPoolTransfer = minerToPoolTransfer;
            this.poolToMinerTransfer = poolToMinerTransfer;
            this.messageManager = messageManager;
            this.statisticsManager = statisticsManager;
            this.settings = settings;
        }

        public void Listen()
        {
            proxyListener = CreateProxyListener();

            StartListeningForNewConnections();

            HandleNewConnections();
        }

        private TcpListener CreateProxyListener()
        {
            var localIp = IPAddress.Loopback;
            var localPort = settings.Proxy.Port;

            return new TcpListener(localIp, localPort);
        }

        private void StartListeningForNewConnections()
        {
            proxyListener.Start();
        }

        private void HandleNewConnections()
        {
            while (!Environment.HasShutdownStarted)
            {
                if (settings.Proxy.DebugOn)
                    WriteWaitingForConnectionDebugLog();

                var minerClient = proxyListener.AcceptTcpClient();

                SetConnections(minerClient);

                if (settings.Proxy.DebugOn)
                    WriteNewMinerConnectedForDebugLog();

                statisticsManager.AddNewlyConnectedMiner(minerConnection.Id);

                StartDataTransfersOnNewThreads();
            }
        }

        private void WriteWaitingForConnectionDebugLog()
        {
            var waitingForConnectionsMessage =
                new ConsoleMessage($"Waiting for new mining connections on {proxyListener.LocalEndpoint}...");
            messageManager.AddMessage(waitingForConnectionsMessage);
        }

        private void SetConnections(TcpClient minerClient)
        {
            minerConnection = GetDecoratedConnection(new MinerConnection(minerClient));
            poolConnection = GetDecoratedConnection(new MinerPoolConnection(settings));
        }

        private void WriteNewMinerConnectedForDebugLog()
        {
            var newMinerMessage = new ConsoleMessage($"Miner {minerConnection.Id} connected.");
            messageManager.AddMessage(newMinerMessage);
        }

        private void StartDataTransfersOnNewThreads()
        {
            QueueWork(() => minerToPoolTransfer.SendData(minerConnection, poolConnection));
            QueueWork(() => poolToMinerTransfer.SendData(minerConnection, poolConnection));
        }

        private IConnection GetDecoratedConnection(IConnection connection)
        {
            return new ConnectionDecorator(connection, messageManager, statisticsManager, settings).Initialize();
        }

        private static void QueueWork(Action onNewMinerConnected)
        {
            ThreadPool.QueueUserWorkItem(state => onNewMinerConnected());
        }

        public void Dispose()
        {
            minerConnection?.Dispose();
            poolConnection?.Dispose();
            proxyListener?.Stop();
        }
    }
}
