using System;
using ProofOfWorkProxy.Startup;

namespace ProofOfWorkProxy
{
    internal class Program
    {
        public static void Main()
        {
            Console.CursorVisible = false;

            var proxy = ApplicationStart.InitializeProxy();

            proxy.Start();
        }
    }
}
