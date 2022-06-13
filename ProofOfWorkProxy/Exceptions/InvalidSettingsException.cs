using System;

namespace ProofOfWorkProxy.Exceptions
{
    public class InvalidSettingsException : Exception
    {
        public InvalidSettingsException(string message) : base(
            $@"Error: Invalid ProxySettings.

Details:
{message}

Fixes: 
   1) Fix values in your ProxySettings.json file and restart PoW Proxy.")
        {

        }
    }
}
