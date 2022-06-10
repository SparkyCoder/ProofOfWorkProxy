using Microsoft.Extensions.DependencyInjection;
using ProofOfWorkProxy.Connections.Listener;
using ProofOfWorkProxy.DataTransfer;
using ProofOfWorkProxy.Decorators;
using ProofOfWorkProxy.Managers;
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
                .AddSingleton<IStatisticsManager>(provider => new StatisticsManagerRetryDecorator(new StatisticsManager()))
                .AddSingleton<IMessageManager, MessageManager>()
                .AddTransient<IDataTransfer<PoolToMinerTransfer>, PoolToMinerTransfer>()
                .AddTransient<IDataTransfer<MinerToPoolTransfer>, MinerToPoolTransfer>()
                .BuildServiceProvider();

            return serviceCollection.GetService<IProxy>();
        }
    }
}
