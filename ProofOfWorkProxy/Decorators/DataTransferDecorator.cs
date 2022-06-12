using System;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.DataTransfer;
using ProofOfWorkProxy.Managers;

namespace ProofOfWorkProxy.Decorators
{
    public class DataTransferDecorator<T> : IDataTransfer<T>
    {
        private readonly IDataTransfer<T> wrappedDataTransfer;
        private readonly IMessageManager messageManager;

        public DataTransferDecorator(IDataTransfer<T> wrappedDataTransfer, IMessageManager messageManager)
        {
            this.wrappedDataTransfer = wrappedDataTransfer;
            this.messageManager = messageManager;
        }

        public void SendData(IConnection minerConnection, IConnection poolConnection)
        {
            try
            {
                wrappedDataTransfer.SendData(minerConnection, poolConnection);
            }
            catch (Exception exception)
            {
                messageManager.DisplayCriticalError(exception.Message);
            }
        }
    }
}
