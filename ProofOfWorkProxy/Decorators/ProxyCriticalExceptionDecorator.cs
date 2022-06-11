using System;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Proxy;

namespace ProofOfWorkProxy.Decorators
{
    public class ProxyCriticalExceptionDecorator : IProxy
    {
        private readonly IMessageManager messageManager;
        private readonly IProxy wrappedProxy;

        public ProxyCriticalExceptionDecorator(IMessageManager messageManager, IProxy wrappedProxy)
        {
            this.messageManager = messageManager;
            this.wrappedProxy = wrappedProxy;
        }

        public void Start()
        {
            try
            {
                wrappedProxy.Start();
            }
            catch(Exception criticalException)
            {
                var criticalMessage = criticalException.Message;
                messageManager.DisplayCriticalError(criticalMessage);
            }
        }
    }
}
