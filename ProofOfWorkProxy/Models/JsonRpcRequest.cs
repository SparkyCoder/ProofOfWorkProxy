namespace ProofOfWorkProxy.Models
{
    public class JsonRpcRequest
    {
        //Based On https://www.jsonrpc.org/
        public dynamic Id { get; set; }
        public dynamic Method { get; set; }
        public dynamic[] Params { get; set; } 

    }
}
