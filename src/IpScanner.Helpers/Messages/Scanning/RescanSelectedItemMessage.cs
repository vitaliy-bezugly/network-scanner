using IpScanner.Helpers.Enums;

namespace IpScanner.Helpers.Messages.Scanning
{
    public class RescanSelectedItemMessage
    {
        public RescanSelectedItemMessage(ScanningSource source)
        {
            Source = source;
        }

        public ScanningSource Source { get; }
    }
}
