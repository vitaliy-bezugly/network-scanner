using System.Linq;
using System.Net;

namespace IpScanner.Helpers.Extensions
{
    public static class IPAddressExtensions
    {
        public static int GetHostId(this IPAddress ipAddress)
        {
            return int.Parse(ipAddress.ToString().Split('.').Last());
        }

        public static string GetNetworkId(this IPAddress ip)
        {
            var ipParts = ip.ToString().Split('.');

            string networkId = string.Join(".", ipParts.Take(3)) + ".";
            return networkId;
        }

        public static bool IsClassAorB(this IPAddress address)
        {
            byte[] bytes = address.GetAddressBytes();
            byte firstOctet = bytes[0];

            return (firstOctet >= 0 && firstOctet <= 127)       // Class A
                || (firstOctet >= 128 && firstOctet <= 191);    // Class B
        }
    }
}
