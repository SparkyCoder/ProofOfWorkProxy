namespace ProofOfWorkProxy.Models
{
    public class Plugins
    {
        public string AwsKey { get; set; }
        public string AwsSecret { get; set; }
        public bool SnsEnabledForDisconnects { get; set; }
    }
}
