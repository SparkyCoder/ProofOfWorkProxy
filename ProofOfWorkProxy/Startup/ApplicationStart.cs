using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProofOfWorkProxy.Connections.Listener;
using ProofOfWorkProxy.DataTransfer;
using ProofOfWorkProxy.Decorators;
using ProofOfWorkProxy.Extensions;
using ProofOfWorkProxy.Managers;
using ProofOfWorkProxy.Models;
using ProofOfWorkProxy.Proxy;

namespace ProofOfWorkProxy.Startup
{
    public static class ApplicationStart
    {
        public static IProxy InitializeProxy()
        {
            var configurationBuilder = GetConfigurationBuilder();

            var serviceCollection = GetServiceCollection(configurationBuilder);

            return serviceCollection.GetService<IProxy>();
        }

        private static IConfigurationBuilder GetConfigurationBuilder()
        {
            return new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("ProxySettings.json", optional: false);
        }

        private static IServiceProvider GetServiceCollection(IConfigurationBuilder configurationBuilder)
        {
            return new ServiceCollection()
                .AddSingleton<IMessageManager, MessageManager>()
                .AddSingleton(provider => configurationBuilder.Build().GetSettings())
                .AddTransient<IProxyListener>(provider =>
                {
                    var messageManager = provider.GetService<IMessageManager>();
                    var statisticsManager = provider.GetService<IStatisticsManager>();
                    var poolToMiner = provider.GetService<IDataTransfer<PoolToMinerTransfer>>();
                    var minerToPool = provider.GetService<IDataTransfer<MinerToPoolTransfer>>();
                    var settings = provider.GetService<Settings>();

                    return new ProxyCriticalExceptionDecorator(messageManager,
                        new ProxyListener(minerToPool, poolToMiner, messageManager, statisticsManager, settings),
                        settings);
                })
                .AddSingleton<IStatisticsManager>(provider =>
                {
                    var settings = provider.GetService<Settings>();
                    return new StatisticsManagerRetryDecorator(new StatisticsManager(settings));
                })
                .AddTransient<IDataTransfer<PoolToMinerTransfer>>(provider =>
                {
                    var messageManager = provider.GetService<IMessageManager>();
                    var statisticsManager = provider.GetService<IStatisticsManager>();
                    var settings = provider.GetService<Settings>();
                    var poolToMiner = new PoolToMinerTransfer(messageManager, statisticsManager, settings);
                    return new DataTransferDecorator<PoolToMinerTransfer>(poolToMiner, messageManager);
                })
                .AddTransient<IDataTransfer<MinerToPoolTransfer>>(provider =>
                {
                    var messageManager = provider.GetService<IMessageManager>();
                    var statisticsManager = provider.GetService<IStatisticsManager>();
                    var settings = provider.GetService<Settings>();
                    var poolToMiner = new MinerToPoolTransfer(messageManager, statisticsManager, settings);
                    return new DataTransferDecorator<MinerToPoolTransfer>(poolToMiner, messageManager);
                })
                .AddTransient<IProxy, Proxy.Proxy>()
                .BuildServiceProvider();
        }
    }
}
