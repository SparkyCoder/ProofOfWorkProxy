using Microsoft.Extensions.DependencyInjection;
using ProofOfWorkProxy.Connections;
using ProofOfWorkProxy.DataTransfer;
using ProofOfWorkProxy.Proxy;

namespace ProofOfWorkProxy.Startup
{
    public static class ApplicationStart
    {
        public static IProxy InitializeProxy()
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IProxy, Proxy.Proxy>()
                .AddTransient<IProxyListener, ProxyListener>()
                .AddTransient<IDataTransfer<PoolToMinerTransfer>, PoolToMinerTransfer>()
                .AddTransient<IDataTransfer<MinerToPoolTransfer>, MinerToPoolTransfer>()
                .BuildServiceProvider();

            return serviceCollection.GetService<IProxy>();
        }
    }
}
