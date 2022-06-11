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
                .AddSingleton<IMessageManager, MessageManager>()
                .AddTransient<IProxyListener>(provider =>
                {
                    var messageManager = provider.GetService<IMessageManager>();
                    var statisticsManager = provider.GetService<IStatisticsManager>();
                    var poolToMiner = provider.GetService<IDataTransfer<PoolToMinerTransfer>>();
                    var minerToPool = provider.GetService<IDataTransfer<MinerToPoolTransfer>>();

                    return new ProxyCriticalExceptionDecorator(messageManager,
                        new ProxyListener(minerToPool, poolToMiner, messageManager, statisticsManager));
                })
                .AddSingleton<IStatisticsManager>(provider => new StatisticsManagerRetryDecorator(new StatisticsManager()))
                .AddTransient<IDataTransfer<PoolToMinerTransfer>, PoolToMinerTransfer>()
                .AddTransient<IDataTransfer<MinerToPoolTransfer>, MinerToPoolTransfer>()
                .AddTransient<IProxy, Proxy.Proxy>()
                .BuildServiceProvider();

            return serviceCollection.GetService<IProxy>();
        }
    }
}
