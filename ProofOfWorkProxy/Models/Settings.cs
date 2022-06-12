using System.Configuration;
using ProofOfWorkProxy.Extensions;

namespace ProofOfWorkProxy.Models
{
    public static class Settings
    {
        private const string DebugOnKey = "DebugOn";
        private const string ProxyPortKey = "ProxyPort";
        private const string MiningPoolDomainKey = "MiningPoolDomain";
        private const string MiningPoolPortKey = "MiningPoolPort";

        public static int ProxyListeningPort => ConfigurationManager.AppSettings[ProxyPortKey].ToInteger();
        public static string MiningPoolDomain => ConfigurationManager.AppSettings[MiningPoolDomainKey];
        public static int MiningPoolPort => ConfigurationManager.AppSettings[MiningPoolPortKey].ToInteger();
        public static bool DebugOn => ConfigurationManager.AppSettings[DebugOnKey].ToBool();
        public static int RetryDelayInSecondsForWhenInternetGoesDown => 10;
        public static int ErrorMessageDisplayTime => 5;
        public static string GitHubIssuesUrl => "https://github.com/SparkyCoder/ProofOfWorkProxy/issues";

        public static string ApplicationTitle = @"
 ______   ______     __     __        ______   ______     ______     __  __     __  __    
/\  == \ /\  __ \   /\ \  _ \ \      /\  == \ /\  == \   /\  __ \   /\_\_\_\   /\ \_\ \   
\ \  _-/ \ \ \/\ \  \ \ \/ "".\ \     \ \  _-/ \ \  __<   \ \ \/\ \  \/_/\_\/_  \ \____ \  
 \ \_\    \ \_____\  \ \__/"".~\_\     \ \_\    \ \_\ \_\  \ \_____\   /\_\/\_\  \/\_____\ 
  \/_/     \/_____/   \/_/   \/_/      \/_/     \/_/ /_/   \/_____/   \/_/\/_/   \/_____/ 
                                                                                         
";
    }
}
