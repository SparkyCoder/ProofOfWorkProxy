using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ProofOfWorkProxy.DataTransfer;
using ProofOfWorkProxy.Decorators;
using ProofOfWorkProxy.Extensions;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Connections.Listener
{
    public class ProxyListener : IProxyListener
    {
        private readonly IDataTransfer<MinerToPoolTransfer> minerToPoolTransfer;
        private readonly IDataTransfer<PoolToMinerTransfer> poolToMinerTransfer;
        private readonly IMessageManager messageManager;

        public ProxyListener(IDataTransfer<MinerToPoolTransfer> minerToPoolTransfer, IDataTransfer<PoolToMinerTransfer> poolToMinerTransfer, IMessageManager messageManager)
        {
            this.minerToPoolTransfer = minerToPoolTransfer;
            this.poolToMinerTransfer = poolToMinerTransfer;
            this.messageManager = messageManager;
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
                var waitingForConnectionsMessage = new ConsoleMessage($"Waiting for new mining connections on {proxyListener.LocalEndpoint}...", ConsoleColor.White);
                messageManager.AddMessage(waitingForConnectionsMessage);

                var minerClient = proxyListener.AcceptTcpClient();

                var minerConnection = GetDecoratedConnection(new MinerConnection(minerClient));
                var poolConnection = GetDecoratedConnection(new MinerPoolConnection());

                var newMinerMessage = new ConsoleMessage(welcomeMessage(minerConnection), ConsoleColor.DarkMagenta);
                messageManager.AddMessage(newMinerMessage);

                QueueWork(() => minerToPoolTransfer.SendData(minerConnection, poolConnection));
                QueueWork(() => poolToMinerTransfer.SendData(minerConnection, poolConnection));
            }
        }

        private IConnection GetDecoratedConnection(IConnection connection)
        {
            connection = connection.Initialize();
            return new ConnectionDecorator(connection, messageManager);
        }

        private static void QueueWork(Action onNewMinerConnected)
        {
            ThreadPool.QueueUserWorkItem(state => onNewMinerConnected());
        }
    }
}
