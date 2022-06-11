using System;

namespace ProofOfWorkProxy.Exceptions
{
    public  class ConnectionToPoolFailedException : Exception
    {
        public ConnectionToPoolFailedException() : base(
            @"Error: Can't connect to mining pool. 

Fixes: 
   1) Verify pool is operational. 
   2) Check your 'MiningPoolDomain' and 'MiningPoolPort' settings in the App.Config file.")
        {

        }
    }
}
