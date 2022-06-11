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
                .AddTransient<IProxyListener, ProxyListener>()
                .AddSingleton<IStatisticsManager>(provider => new StatisticsManagerRetryDecorator(new StatisticsManager()))
                .AddTransient<IDataTransfer<PoolToMinerTransfer>>(provider =>
                {
                    var messageManager = provider.GetService<IMessageManager>();
                    var statisticsManager = provider.GetService<IStatisticsManager>();
                    return new DataTransferExceptionDecorator<PoolToMinerTransfer>(messageManager,
                        new PoolToMinerTransfer(messageManager, statisticsManager));
                })
                .AddTransient<IDataTransfer<MinerToPoolTransfer>>(provider =>
                {
                    var messageManager = provider.GetService<IMessageManager>();
                    var statisticsManager = provider.GetService<IStatisticsManager>();
                    return new DataTransferExceptionDecorator<MinerToPoolTransfer>(messageManager,
                        new MinerToPoolTransfer(messageManager, statisticsManager));
                })
                .AddTransient<IProxy>(provider =>
                {
                    var messageManager = provider.GetService<IMessageManager>();
                    var proxyListener = provider.GetService<IProxyListener>();
                    return new ProxyCriticalExceptionDecorator(messageManager,
                        new Proxy.Proxy(proxyListener, messageManager));
                })
                .BuildServiceProvider();

            return serviceCollection.GetService<IProxy>();
        }
    }
}
