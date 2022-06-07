using System;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Extensions;

namespace ProofOfWorkProxy.Decorators
{
    public class ConnectionDecorator : IConnection
    {
        private readonly IConnection wrappedConnection;

        public bool IsTerminated
        {
            get => wrappedConnection.IsTerminated;
            set => wrappedConnection.IsTerminated = value;
        }

        public string Id => wrappedConnection.Id;

        public ConnectionDecorator(IConnection wrappedConnection)
        {
            this.wrappedConnection = wrappedConnection;
        }

        public IConnection Initialize()
        {
            return wrappedConnection.Initialize();
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
            var className = wrappedConnection.GetType().ToString().GetClassName();

            $"{className} {Id} Terminated!".Display(ConsoleColor.Red);

            if(wrappedConnection.IsTerminated) return;

            wrappedConnection.IsTerminated = true;
            IsTerminated = true;

            wrappedConnection.Dispose();
        }
    }
}
