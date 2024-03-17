namespace IpScanner.Helpers.Messages.IpRange
{
    public class SaveIpRangeMessage
    {
        public SaveIpRangeMessage(Models.IpRange range)
        {
            IpRange = range;
        }

        public Models.IpRange IpRange { get; }
    }
}
