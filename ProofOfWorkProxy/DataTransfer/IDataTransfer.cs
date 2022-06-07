using ProofOfWorkProxy.Connections;

namespace ProofOfWorkProxy.DataTransfer
{
    public interface IDataTransfer<T>
    {
        public void SendData(IConnection minerConnection, IConnection poolConnection);
    }
}
