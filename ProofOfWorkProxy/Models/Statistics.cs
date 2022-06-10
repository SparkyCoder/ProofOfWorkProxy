using System;

namespace ProofOfWorkProxy.Models
{
    public class Statistics
    {
        public long SharesSubmitted { get; private set; }
        public long Requests { get; private set; }
        public long Responses { get; private set; }
        public long Errors { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public Statistics()
        {
            LastUpdated = DateTime.Now;
            SharesSubmitted = 0;
            Requests = 0;
            Responses = 0;
            Errors = 0;
        }

        public void ShareWasSubmittedToPool()
        {
            SharesSubmitted++;
            LastUpdated = DateTime.Now;
        }

        public void MinerMadeRequestToPool()
        {
            Requests++;
            LastUpdated = DateTime.Now;
        }

        public void PoolRespondedToMiner()
        {
            Responses++;
            LastUpdated = DateTime.Now;
        }

        public void PoolRespondedWithAnError()
        {
            Errors++;
            LastUpdated = DateTime.Now;
        }
    }
}
