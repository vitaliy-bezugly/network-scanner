using System.Diagnostics;
using IpScanner.Helpers.Configurations;
using IpScanner.Services.Abstract;

namespace IpScanner.Services
{
    public class RdpService : IRdpService
    {
        public void Connect(RdpConfiguration configuration)
        {
            string arguments = $"/v:{configuration.IpAddress}";
            Process.Start("mstsc.exe", arguments);
        }
    }
}
