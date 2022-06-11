using System;

namespace ProofOfWorkProxy.Exceptions
{
    public  class ConnectionToPoolFailedException : Exception
    {
        public ConnectionToPoolFailedException() : base(
            @"Error: Can't connect to mining pool. 

Fixes: 
   1) Check your internet connection. 
   2) Check your 'MiningPoolDomain' and 'MiningPoolPort' settings in the App.Config file.
   3) Verify your pool is not down for maintenance.")
        {

        }
    }
}
