using System;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.DataTransfer;
using ProofOfWorkProxy.Managers;

namespace ProofOfWorkProxy.Decorators
{
    public class DataTransferExceptionDecorator<T> : IDataTransfer<T>
    {
        private readonly IMessageManager messageManager;
        private readonly IDataTransfer<T> wrappedDataTransfer;

        public DataTransferExceptionDecorator(IMessageManager messageManager, IDataTransfer<T> wrappedDataTransfer)
        {
            this.messageManager = messageManager;
            this.wrappedDataTransfer = wrappedDataTransfer;
        }

        public void SendData(IConnection minerConnection, IConnection poolConnection)
        {
            try
            {
                wrappedDataTransfer.SendData(minerConnection, poolConnection);
            }
            catch (Exception exception)
            {
                var exceptionMessage = exception.Message;
                messageManager.DisplayCriticalError(exceptionMessage);
            }
        }
    }
}
