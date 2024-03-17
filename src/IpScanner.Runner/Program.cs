using System.Diagnostics;
using Windows.Storage;

var ipAddress = ApplicationData.Current.LocalSettings.Values["RdpAddress"] as string;
if (!string.IsNullOrEmpty(ipAddress))
{
    string arguments = $"/v:{ipAddress}";
    Process.Start("mstsc.exe", arguments);
}