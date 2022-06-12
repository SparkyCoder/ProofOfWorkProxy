namespace ProofOfWorkProxy.Models
{
    public class SubmittedShare
    {
        public dynamic ShareId { get; }
        public bool Accepted { get; private set; }

        public SubmittedShare(dynamic shareId)
        {
            ShareId = shareId;
        }

        public void MarkShareAsAccepted()
        {
            Accepted = true;
        }
    }
}
