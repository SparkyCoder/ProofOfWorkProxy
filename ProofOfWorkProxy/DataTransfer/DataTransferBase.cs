using System;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.DataTransfer
{
    public abstract class DataTransferBase<T> : IDataTransfer<T>
    {
        private readonly IMessageManager messageManager;

        protected DataTransferBase(IMessageManager messageManager)
        {
            this.messageManager = messageManager;
        }

        protected void DisplayTransfer(string sentData, string minerId, string direction)
        {
            if (!Settings.DebugOn) return;

            var dataTransferMessage = new ConsoleMessage($"{minerId} {direction}", ConsoleColor.Green);
            messageManager.AddMessage(dataTransferMessage);

            var stratumJsonMessage = new ConsoleMessage(sentData, ConsoleColor.White);
            messageManager.AddMessage(stratumJsonMessage);
        }

        protected bool ConnectionsAreValid(IConnection miner, IConnection pool)
        {
            miner.CheckIfConnectionIsAlive();
            pool.CheckIfConnectionIsAlive();
            return !miner.IsTerminated && !pool.IsTerminated;
        }
        public virtual void SendData(IConnection minerConnection, IConnection poolConnection)
        {
            throw new NotImplementedException();
        }
    }
}
