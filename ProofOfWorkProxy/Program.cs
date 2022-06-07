using ProofOfWorkProxy.Startup;

namespace ProofOfWorkProxy
{
    internal class Program
    {
        public static void Main()
        {
            var proxy = ApplicationStart.InitializeProxy();
            proxy.Start();
        }
    }
}
