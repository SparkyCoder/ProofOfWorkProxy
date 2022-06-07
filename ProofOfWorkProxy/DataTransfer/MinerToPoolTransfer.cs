using System;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Extensions;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.DataTransfer
{
    public class MinerToPoolTransfer : DataTransferBase<MinerToPoolTransfer>
    {
        public MinerToPoolTransfer(IMessageManager messageManager) : base(messageManager) { }

        public override void SendData(IConnection minerConnection, IConnection poolConnection)
        {
            while (ConnectionsAreValid(minerConnection, poolConnection))
            {
                var stratumRequest = minerConnection.Read();

                if (string.IsNullOrEmpty(stratumRequest)) continue;

                DisplayTransfer(stratumRequest, minerConnection.Id, "Miner ---------------> Pool");

                poolConnection.Write(stratumRequest);
            }
        }
    }
}
