using IpScanner.Models.Enums;

namespace IpScanner.Helpers.Messages.Scanning
{
    public class UpdateScanningStatusMessage
    {
        public UpdateScanningStatusMessage(ScanningStatus status)
        {
            Status = status;
        }

        public ScanningStatus Status { get; }
    }
}
