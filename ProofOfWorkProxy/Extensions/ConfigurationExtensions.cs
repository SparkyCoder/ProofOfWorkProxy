using System;
using Microsoft.Extensions.Configuration;
using ProofOfWorkProxy.Exceptions;
using ProofOfWorkProxy.Models;

namespace ProofOfWorkProxy.Extensions
{
    public static class ConfigurationExtensions
    {
        public static Settings GetSettings(this IConfiguration configuration)
        {
            try
            {
                return configuration.Get(typeof(Settings)) as Settings;
            }
            catch (Exception exception)
            {
                var invalidSetting = new InvalidSettingsException(exception.Message);
                
                new ConsoleMessage(invalidSetting.Message, ConsoleColor.Red).DisplayMessage();

                Console.ReadKey();
                Environment.Exit(0);
            }

            return new Settings();
        }
    }
}
