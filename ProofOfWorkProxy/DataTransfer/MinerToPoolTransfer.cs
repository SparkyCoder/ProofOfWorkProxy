using System;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Extensions;

namespace ProofOfWorkProxy.DataTransfer
{
    public class MinerToPoolTransfer : DataTransferBase<MinerToPoolTransfer>
    {
        public override void SendData(IConnection minerConnection, IConnection poolConnection)
        {
            while (ConnectionsAreValid(minerConnection, poolConnection))
            {
                var stratumRequest = minerConnection.Read();

                if (string.IsNullOrEmpty(stratumRequest)) continue;

                poolConnection.Write(stratumRequest);

                $"{minerConnection.Id} Miner  ---------------> Pool".Display(ConsoleColor.Green);
                stratumRequest.Display(ConsoleColor.White);
            }
        }
    }
}
