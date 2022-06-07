using System.Configuration;
using ProofOfWorkProxy.Extensions;

namespace ProofOfWorkProxy.Models
{
    public static class Settings
    {
        private const string ProxyPortKey = "ProxyPort";
        private const string MiningPoolDomainKey = "MiningPoolDomain";
        private const string MiningPoolPortKey = "MiningPoolPort";

        public static int ProxyListeningPort => ConfigurationManager.AppSettings[ProxyPortKey].ToInteger();
        public static string MiningPoolDomain => ConfigurationManager.AppSettings[MiningPoolDomainKey];
        public static int MiningPoolPort => ConfigurationManager.AppSettings[MiningPoolPortKey].ToInteger();
    }
}
