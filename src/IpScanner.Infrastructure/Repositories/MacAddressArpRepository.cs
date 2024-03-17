using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IpScanner.Domain.Contracts;
using IpScanner.Helpers;
using IpScanner.Helpers.Extensions; 

namespace IpScanner.Infrastructure.Repositories
{
    public class MacAddressArpRepository : IMacAddressRepository
    {
        private const int TimeoutInSecondsWithLowAccuracy = 1000;
        private readonly AppSettings settings;

        public MacAddressArpRepository(ISettingsService settingsService)
        {
            settings = settingsService.Settings;
        }

        public async Task<PhysicalAddress> GetMacAddressAsync(IPAddress destination, CancellationToken cancellationToken)
        {
            TimeSpan delayUntilCancel = TimeSpan.FromMilliseconds(TimeoutInSecondsWithLowAccuracy);
            CancellationToken highSpeedToken = cancellationToken.CreateLinkedCancellationTokenWithTimeout(delayUntilCancel);

            CancellationToken token = settings.HighAccuracy ? cancellationToken : highSpeedToken;
            return await Task.Run(() => RetrieveMacAddress(destination), token);
        }

        private PhysicalAddress RetrieveMacAddress(IPAddress destination)
        {
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            int destIP = BitConverter.ToInt32(destination.GetAddressBytes(), 0);

            bool deviceFound = SendARP(destIP, 0, macAddr, ref macAddrLen) == 0;
            return deviceFound ? new PhysicalAddress(macAddr) : PhysicalAddress.None;
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);
    }
}
