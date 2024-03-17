using System.Net;

namespace IpScanner.Helpers.Configurations
{
    public class RdpConfiguration
    {
        public RdpConfiguration(IPAddress ipAddress)
        {
            IpAddress = ipAddress;
        }

        public IPAddress IpAddress { get; }
    }
}
