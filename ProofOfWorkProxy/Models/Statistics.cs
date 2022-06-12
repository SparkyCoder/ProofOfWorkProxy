using System;
using System.Collections.Concurrent;
using System.Linq;

namespace ProofOfWorkProxy.Models
{
    public class Statistics
    {
        public ConcurrentBag<SubmittedShare> SharesSubmitted { get; }
        public long Requests { get; private set; }
        public long Responses { get; private set; }
        public long Errors { get; private set; }
        public DateTime ConnectedDateTime { get; }
        public DateTime LastUpdated { get; private set; }

        public Statistics()
        {
            ConnectedDateTime = DateTime.Now;
            LastUpdated = DateTime.Now;
            SharesSubmitted = new ConcurrentBag<SubmittedShare>();
            Requests = 0;
            Responses = 0;
            Errors = 0;
        }

        public void ShareWasSubmittedToPool(dynamic shareId)
        {
            if (shareId == null) return;

            var share = new SubmittedShare(shareId);
            SharesSubmitted.Add(share);
            LastUpdated = DateTime.Now;
        }

        public void ShareWasAccepted(dynamic shareId)
        {
            if(shareId == null) return;

            var share = SharesSubmitted.FirstOrDefault(submittedShare => submittedShare.ShareId == shareId);

            if (share == null) return;

            share.MarkShareAsAccepted();
            LastUpdated = DateTime.Now;
        }

        public int GetAcceptedShares()
        {
            return SharesSubmitted.Count(share => share.Accepted);
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
