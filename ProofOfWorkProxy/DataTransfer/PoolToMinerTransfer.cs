using System;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Extensions;

namespace ProofOfWorkProxy.DataTransfer
{
    public class PoolToMinerTransfer : DataTransferBase<PoolToMinerTransfer>
    {
        public override void SendData(IConnection minerConnection, IConnection poolConnection)
        {
            while (ConnectionsAreValid(minerConnection, poolConnection))
            {
                var stratumResponse = poolConnection.Read();

                if(string.IsNullOrEmpty(stratumResponse)) continue;

                minerConnection.Write(stratumResponse);

                $" {minerConnection.Id} Miner <--------------- Pool".Display(ConsoleColor.Green);
                stratumResponse.Display(ConsoleColor.White);
            }
        }
    }
}
