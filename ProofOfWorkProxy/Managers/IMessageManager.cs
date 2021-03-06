using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Managers
{
    public interface IMessageManager
    {
        public void AddMessage(ConsoleMessage message);
        public void StartTimerDisplayMessagesFromQueue();
        void DisplayCriticalError(string criticalMessage);
    }
}
