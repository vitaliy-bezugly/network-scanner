namespace IpScanner.Helpers.Messages.IpRange
{
    public class IpRangeUpdatedMessage
    {
        public IpRangeUpdatedMessage(string ipRange)
        {
            IpRange = ipRange;
        }

        public string IpRange { get; }
    }
}
