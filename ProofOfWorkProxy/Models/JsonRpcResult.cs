namespace ProofOfWorkProxy.Models
{
    public class JsonRpcResult
    {
        //Based On https://www.jsonrpc.org/
        public dynamic Id { get; set; }
        public dynamic Result { get; set; }
        public dynamic Error { get; set; }

    }
}
