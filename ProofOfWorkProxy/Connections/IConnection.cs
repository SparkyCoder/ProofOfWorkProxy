using System;

namespace ProofOfWorkProxy.Connections
{
    public interface IConnection : IDisposable
    {
        public bool IsTerminated { get; set; }
        public string Id { get; }
        void Write(string stratumJson);
        string Read();
        IConnection Initialize();
        void CheckIfConnectionIsAlive();
    }
}
