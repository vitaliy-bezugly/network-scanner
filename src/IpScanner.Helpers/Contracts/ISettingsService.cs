using IpScanner.Helpers;

namespace IpScanner.Helpers
{
    public interface ISettingsService
    {
        void SaveSettings();
        AppSettings Settings { get; }
    }
}
