namespace IpScanner.Helpers
{
    public class AppSettings
    {
        public bool ShowUnknown { get; set; } = false;
        public bool ShowOffline { get; set; } = true;
        public bool ShowOnline { get; set; } = true;
        public bool ShowMiscellaneousToolbar { get; set; } = true;
        public bool ShowActionsToolbar { get; set; }  = true;
        public bool ShowDetails { get; set; } = false;
        public bool FavoritesSelected { get; set; } = false;
        public string IpRange { get; set; } = string.Empty;
        public string ColorTheme { get; set; } = "Default";
        public bool HighAccuracy { get; set; } = true;
        public int ScanningSpeed { get; set; } = 45;
        public bool ScanSharedFoldersAndPrinters { get; set; } = true;
        public bool ScanHttp { get; set; } = true;
        public bool ScanHttps { get; set; } = true;
        public bool ScanFtp { get; set; } = true;
        public bool ScanRdp { get; set; } = true;
        public bool ScanDateTime { get; set; } = false;
        public int UdpPort { get; set; } = 137;
        public int TcpPort { get; set; } = 80;
        public bool ScanTcp { get; set; } = true;
        public bool ScanUdp { get; set; } = true;
        public bool EnableValidationMachineLearning { get; set; } = false;
        public bool EnableClassificationMachineLearning { get; set; } = false;
        public bool MicrophoneEnabled { get; set; } = false;
        public bool EnableGpt { get; set; } = false;
        public string GptApiKey { get; set; } = string.Empty;
    }
}
