namespace IpScanner.Helpers.Messages.Progress
{
    public class SetTotalCountOfScannedIpsMessage
    {
        public SetTotalCountOfScannedIpsMessage(int totalCountOfIps)
        {
            TotalCount = totalCountOfIps;
        }

        public int TotalCount { get; }
    }
}
