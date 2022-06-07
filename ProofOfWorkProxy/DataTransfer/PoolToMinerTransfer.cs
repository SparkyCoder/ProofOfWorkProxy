using System;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Extensions;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.DataTransfer
{
    public class PoolToMinerTransfer : DataTransferBase<PoolToMinerTransfer>
    {
        public PoolToMinerTransfer(IMessageManager messageManager) : base(messageManager) { }

        public override void SendData(IConnection minerConnection, IConnection poolConnection)
        {
            while (ConnectionsAreValid(minerConnection, poolConnection))
            {
                minerConnection.CheckIfConnectionIsAlive();

                var stratumResponse = poolConnection.Read();

                if(string.IsNullOrEmpty(stratumResponse)) continue;

                DisplayTransfer(stratumResponse, minerConnection.Id, "Miner <--------------- Pool");

                minerConnection.Write(stratumResponse);
            }
        }



        
    }
}
