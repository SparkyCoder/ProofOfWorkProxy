using System;
using System.Net.Sockets;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Exceptions;
using ProofOfWorkProxy.Extensions;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Decorators
{
    public class ConnectionDecorator : IConnection
    {
        private IConnection wrappedConnection;
        private readonly IMessageManager messageManager;
        private readonly IStatisticsManager statisticsManager;
        private readonly Settings settings;

        public bool IsTerminated
        {
            get => wrappedConnection.IsTerminated;
            set => wrappedConnection.IsTerminated = value;
        }

        public string Id => wrappedConnection.Id;

        public ConnectionDecorator(IConnection wrappedConnection, IMessageManager messageManager, IStatisticsManager statisticsManager, Settings settings)
        {
            this.wrappedConnection = wrappedConnection;
            this.messageManager = messageManager;
            this.statisticsManager = statisticsManager;
            this.settings = settings;
        }

        public IConnection Initialize()
        {
            try
            {
                wrappedConnection =  wrappedConnection.Initialize();
                return this;

            }
            catch (SocketException)
            {
                Dispose();
                throw new ConnectionToPoolFailedException();
            }
        }

        public void CheckIfConnectionIsAlive()
        {
            try
            {
                wrappedConnection.CheckIfConnectionIsAlive();
            }
            catch
            {
                Dispose();
            }
        }

        public void Write(string stratumJson)
        {
            try
            {
                wrappedConnection.Write(stratumJson);
            }
            catch
            {
                Dispose();
            }
        }

        public string Read()
        {
            try
            {
                return wrappedConnection.Read();
            }
            catch
            {
                Dispose();
                return string.Empty;
            }
        }

        public void Dispose()
        {
            if(wrappedConnection.IsTerminated) return;

            if (settings.Proxy.DebugOn)
            {
                var className = wrappedConnection.GetType().ToString().GetClassName();
                var terminatedMessage = new ConsoleMessage($"{className} {Id} Terminated!", ConsoleColor.Red);
                messageManager.AddMessage(terminatedMessage);
            }

            statisticsManager.RemoveMinerStatistics(Id);
            statisticsManager.AddToTotalDisconnectCount();

            wrappedConnection.IsTerminated = true;
            IsTerminated = true;

            wrappedConnection.Dispose();
        }
    }
}
