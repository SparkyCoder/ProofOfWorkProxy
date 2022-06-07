using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ProofOfWorkProxy.Connections
{
    public abstract class BaseConnection : IConnection
    {
        public bool IsTerminated { get; set; }
        public string Id { get; private set; }
        private TcpClient client;
        private NetworkStream networkStream;
        private StreamReader streamReader;
        private StreamWriter streamWriter;

        public IConnection Initialize()
        {
            client = GetClient();
            Id = client.Client.RemoteEndPoint.ToString();
            networkStream = client.GetStream();
            streamReader = new StreamReader(networkStream, Encoding.UTF8);
            streamWriter = new StreamWriter(networkStream) { AutoFlush = true }; ;
            return this;
        }

        public void CheckIfConnectionIsAlive()
        {
            var ping = Encoding.ASCII.GetBytes(" ");
            networkStream.Write(ping);
        }

        protected virtual TcpClient GetClient()
        {
            throw new NotImplementedException();
        }

        public void Write(string stratumJson)
        {
            streamWriter.WriteLine(stratumJson);
        }

        public string Read()
        {
            return streamReader.ReadLine();
        }

        public void Dispose()
        {
            streamReader.Dispose();
            streamWriter.Dispose();
            networkStream.Dispose();
            client.Dispose();
        }
    }
}
