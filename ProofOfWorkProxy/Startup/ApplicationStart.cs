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
                .AddTransient<IDataTransfer<PoolToMinerTransfer>>(provider =>
                {
                    var messageManager = provider.GetService<IMessageManager>();
                    var statisticsManager = provider.GetService<IStatisticsManager>();
                    var poolToMiner = new PoolToMinerTransfer(messageManager, statisticsManager);
                    return new DataTransferDecorator<PoolToMinerTransfer>(poolToMiner, messageManager);
                })
                .AddTransient<IDataTransfer<MinerToPoolTransfer>>(provider =>
                {
                    var messageManager = provider.GetService<IMessageManager>();
                    var statisticsManager = provider.GetService<IStatisticsManager>();
                    var poolToMiner = new MinerToPoolTransfer(messageManager, statisticsManager);
                    return new DataTransferDecorator<MinerToPoolTransfer>(poolToMiner, messageManager);
                })
                .AddTransient<IProxy, Proxy.Proxy>()
                .BuildServiceProvider();

            return serviceCollection.GetService<IProxy>();
        }
    }
}
