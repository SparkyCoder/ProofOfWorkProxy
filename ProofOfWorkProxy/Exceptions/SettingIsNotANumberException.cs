using System;

namespace ProofOfWorkProxy.Exceptions
{
    public  class ToggleValueIsNotValidException : Exception
    {
        public ToggleValueIsNotValidException(string invalidValue) : base(
            $@"Error: Value '{invalidValue}' is not a valid toggle. 

Fixes: 
   1) Your App.config boolean true/false values are not valid. Please update them and restart.")
        {

        }
    }
}
