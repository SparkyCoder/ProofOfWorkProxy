namespace ProofOfWorkProxy.Models
{
    public class Proxy
    {
        public string SectionName => "ProxySettings";
        public int Port { get; set; }
        public bool DebugOn { get; set; }
    }
}
