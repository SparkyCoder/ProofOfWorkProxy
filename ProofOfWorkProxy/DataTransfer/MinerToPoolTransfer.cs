using System.Linq;
using Newtonsoft.Json;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.DataTransfer
{
    public class MinerToPoolTransfer : DataTransferBase<MinerToPoolTransfer>
    {
        private readonly IStatisticsManager statisticsManager;
        public MinerToPoolTransfer(IMessageManager messageManager, IStatisticsManager statisticsManager) : base(messageManager)
        {
            this.statisticsManager = statisticsManager;
        }

        public override void SendData(IConnection minerConnection, IConnection poolConnection)
        {
            while (ConnectionsAreValid(minerConnection, poolConnection))
            {
                var stratumRequest = minerConnection.Read();

                if (string.IsNullOrEmpty(stratumRequest)) continue;

                UpdateStatistics(minerConnection.Id, stratumRequest);

                DisplayTransfer(stratumRequest, minerConnection.Id, "Miner ---------------> Pool");

                poolConnection.Write(stratumRequest);
            }

            minerConnection.Dispose();
            poolConnection.Dispose();
        }

        private void UpdateStatistics(string minerId, string minerStratumRequest)
        {
            var jsonRpc = JsonConvert.DeserializeObject<JsonRpcRequest>(minerStratumRequest);

            if (jsonRpc == null || jsonRpc.Method == null) return;

            var statisticsToUpdate = statisticsManager.GetCurrentStatistics(minerId);

            if (statisticsToUpdate == null) return;

            UpdateValues(statisticsToUpdate, jsonRpc);
            
            statisticsManager.AddOrUpdateStatistics(minerId, statisticsToUpdate);
        }

        private static void UpdateValues(Statistics statisticsToUpdate, JsonRpcRequest jsonRpc)
        {
            statisticsToUpdate.MinerMadeRequestToPool();

            var methodName = jsonRpc.Method as string;

            if (IsMethodAShareSubmittedRequest(methodName))
                statisticsToUpdate.ShareWasSubmittedToPool();
        }

        private static bool IsMethodAShareSubmittedRequest(string methodName)
        {
            return MethodTypes.Submit.Any(method => methodName.ToUpper().Contains(method));
        }
    }
}
