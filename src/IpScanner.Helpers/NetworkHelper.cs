using System.Linq;
using Windows.Networking.Connectivity;

namespace IpScanner.Helpers
{
    public static class NetworkHelper
    {
        private const string MaxIpOctet = "255";

        public static string GetLocalIpRange()
        {
            string result = null;
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter != null)
            {
                var hostname = NetworkInformation.GetHostNames()
                    .FirstOrDefault(hn =>
                        hn.IPInformation?.NetworkAdapter != null &&
                        hn.IPInformation.NetworkAdapter.NetworkAdapterId == icp.NetworkAdapter.NetworkAdapterId);

                if (hostname != null)
                {
                    string localIP = hostname.CanonicalName;
                    string baseIP = localIP.Substring(0, localIP.LastIndexOf('.') + 1);
                    result = $"{baseIP}0-{MaxIpOctet}";
                }
            }

            return result;
        }
    }
}
