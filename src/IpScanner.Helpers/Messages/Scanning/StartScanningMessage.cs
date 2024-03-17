using IpScanner.Helpers.Enums;

namespace IpScanner.Helpers.Messages.Scanning
{
    public class StartScanningMessage<TFile>
        where TFile : class
    {
        public StartScanningMessage(ScanningSource source, TFile file = null)
        {
            Source = source;
            File = file;
        }

        public ScanningSource Source { get; }
        public TFile File { get; }
    }
}
