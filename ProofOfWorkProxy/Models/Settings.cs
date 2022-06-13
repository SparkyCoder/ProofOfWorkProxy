namespace ProofOfWorkProxy.Models
{
    public class Settings
    {
        public Proxy Proxy { get; set; }
        public MiningPool MiningPool { get; set; }
        public Plugins Plugins { get; set; }
        public int RetryDelayInSecondsForWhenInternetGoesDown => 10;
        public int ErrorMessageDisplayTime => 9;
        public string GitHubIssuesUrl => "https://github.com/SparkyCoder/ProofOfWorkProxy/issues";

        public string ApplicationTitle = @"
 ______   ______     __     __        ______   ______     ______     __  __     __  __    
/\  == \ /\  __ \   /\ \  _ \ \      /\  == \ /\  == \   /\  __ \   /\_\_\_\   /\ \_\ \   
\ \  _-/ \ \ \/\ \  \ \ \/ "".\ \     \ \  _-/ \ \  __<   \ \ \/\ \  \/_/\_\/_  \ \____ \  
 \ \_\    \ \_____\  \ \__/"".~\_\     \ \_\    \ \_\ \_\  \ \_____\   /\_\/\_\  \/\_____\ 
  \/_/     \/_____/   \/_/   \/_/      \/_/     \/_/ /_/   \/_____/   \/_/\/_/   \/_____/ 
                                                                                         
";
    }
}
