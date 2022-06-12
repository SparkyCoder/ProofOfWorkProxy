using Newtonsoft.Json;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.Extensions;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.DataTransfer
{
    public class PoolToMinerTransfer : DataTransferBase<PoolToMinerTransfer>
    {
        private readonly IStatisticsManager statisticsManager;
        public PoolToMinerTransfer(IMessageManager messageManager, IStatisticsManager statisticsManager) : base(messageManager)
        {
            this.statisticsManager = statisticsManager;
        }

        public override void SendData(IConnection minerConnection, IConnection poolConnection)
        {
            while (ConnectionsAreValid(minerConnection, poolConnection))
            {
                var stratumResponse = poolConnection.Read();

                if(string.IsNullOrEmpty(stratumResponse)) continue;

                UpdateStatistics(minerConnection.Id, stratumResponse);

                DisplayTransfer(stratumResponse, minerConnection.Id, "Miner <--------------- Pool");

                minerConnection.Write(stratumResponse);
            }

            minerConnection.Dispose();
            poolConnection.Dispose();
        }

        private void UpdateStatistics(string minerId, string poolStratumResponse)
        {
            var jsonRpc = JsonConvert.DeserializeObject<JsonRpcResult>(poolStratumResponse);

            if (jsonRpc == null) return;

            var statisticsToUpdate = statisticsManager.GetCurrentStatistics(minerId);

            if(statisticsToUpdate == null) return;

            UpdateValues(statisticsToUpdate, jsonRpc);

            statisticsManager.AddOrUpdateStatistics(minerId, statisticsToUpdate);
        }

        private static void UpdateValues(Statistics statisticsToUpdate, JsonRpcResult jsonRpc)
        {
            statisticsToUpdate.PoolRespondedToMiner();

            if(jsonRpc?.Error != null)
                statisticsToUpdate.PoolRespondedWithAnError();

            if (IsAcceptedResponse(jsonRpc))
                statisticsToUpdate.ShareWasAccepted(jsonRpc?.Id);
        }

        private static bool IsAcceptedResponse(JsonRpcResult jsonRpc)
        {
            var resultType = jsonRpc?.Result?.GetType()?.ToString() as string;

            return (resultType.IsBoolean() && jsonRpc?.Id != null && jsonRpc?.Result == true &&
                    jsonRpc?.Error == null) ;
        }
    }
}
