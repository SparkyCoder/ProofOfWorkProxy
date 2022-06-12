using System;
using System.Runtime.Remoting;
using ProofOfWorkProxy.Extensions;

namespace ProofOfWorkProxy.Models
{
    public class JsonRpcResult
    {
        //Based On https://www.jsonrpc.org/
        public dynamic Id { get; set; }
        public dynamic Result { get; set; }
        public dynamic Error { get; set; }

        public static explicit operator ObjectHandle(JsonRpcResult v)
        {
            throw new NotImplementedException();
        }

        public bool IsAcceptedResponse(JsonRpcResult jsonRpc)
        {
            var resultType = jsonRpc?.Result?.GetType()?.ToString() as string;

            return (resultType.IsBoolean() && jsonRpc?.Id != null && jsonRpc?.Result == true &&
                    jsonRpc?.Error == null);
        }
    }
}
