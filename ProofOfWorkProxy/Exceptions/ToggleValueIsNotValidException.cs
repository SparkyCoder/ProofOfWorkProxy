using System;

namespace ProofOfWorkProxy.Exceptions
{
    public  class SettingIsNotANumberException : Exception
    {
        public SettingIsNotANumberException(string invalidValue) : base(
            $@"Error: Value '{invalidValue}' is not a valid number. 

Fixes: 
   1) Your App.config Port values are not valid. Please update them and restart.")
        {

        }
    }
}
