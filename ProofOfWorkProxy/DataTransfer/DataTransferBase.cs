using System;
using ProofOfWorkProxy.Connections;

namespace ProofOfWorkProxy.DataTransfer
{
    public abstract class DataTransferBase<T> : IDataTransfer<T>
    {
        public bool ConnectionsAreValid(IConnection miner, IConnection pool)
        {
            return !miner.IsTerminated && !pool.IsTerminated;
        }
        public virtual void SendData(IConnection minerConnection, IConnection poolConnection)
        {
            throw new NotImplementedException();
        }
    }
}
