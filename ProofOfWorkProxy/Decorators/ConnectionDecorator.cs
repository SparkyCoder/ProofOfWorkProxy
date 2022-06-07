using System;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Extensions;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Decorators
{
    public class ConnectionDecorator : IConnection
    {
        private readonly IConnection wrappedConnection;
        private readonly IMessageManager messageManager;

        public bool IsTerminated
        {
            get => wrappedConnection.IsTerminated;
            set => wrappedConnection.IsTerminated = value;
        }

        public string Id => wrappedConnection.Id;

        public ConnectionDecorator(IConnection wrappedConnection, IMessageManager messageManager)
        {
            this.wrappedConnection = wrappedConnection;
            this.messageManager = messageManager;
        }

        public IConnection Initialize()
        {
            return wrappedConnection.Initialize();
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

            var className = wrappedConnection.GetType().ToString().GetClassName();
            var terminatedMessage = new ConsoleMessage($"{className} {Id} Terminated!", ConsoleColor.Red);
            messageManager.AddMessage(terminatedMessage);

            wrappedConnection.IsTerminated = true;
            IsTerminated = true;

            wrappedConnection.Dispose();
        }
    }
}
