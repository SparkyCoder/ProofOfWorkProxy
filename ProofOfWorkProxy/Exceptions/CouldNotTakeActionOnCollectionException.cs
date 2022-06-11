using System;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Exceptions
{
    public class CouldNotTakeActionOnCollectionException : Exception
    {
        public CouldNotTakeActionOnCollectionException(string method, string collection) : base("Error: Could not perform "+ method +" on " + collection+ @" collection.

Fixes:
   1) This should never happen. Keep calm and Blockchain on. 
   2) Report incident in GitHub - " + Settings.GitHubIssuesUrl)
        {

        }
    }
}
