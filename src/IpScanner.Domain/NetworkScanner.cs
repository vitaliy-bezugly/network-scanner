using System;
using System.Net;
using System.Threading;
using IpScanner.Models;
using IpScanner.Models.Args;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using IpScanner.Domain.Contracts;
using IpScanner.Helpers.Configurations;
using IpScanner.Domain.Abstract;
using System.Net.Http;
using System.Linq;
using IpScanner.Models.Enums;
using IpScanner.Helpers;
using System.Diagnostics;

namespace IpScanner.Domain
{
    public class NetworkScanner : NetworkScannerBase, INetworkScanner
    {
        private readonly ITcpService tcpService;
        private readonly IHostRepository hostRepository;
        private readonly IManufactorRepository manufactorRepository;
        private readonly IMacAddressRepository macAddressRepository;
        private readonly IUdpService udpService;
        private readonly IHttpService httpService;
        private readonly IDeviceClassifier deviceClassifier;

        public NetworkScanner(IManufactorRepository manufactorRepository, 
            IMacAddressRepository macAddressRepository, 
            IHostRepository hostRepository,
            ITcpService tcpService,
            IUdpService udpService,
            IHttpService httpService, 
            IDeviceClassifier deviceClassifier,
            ISettingsService settings) : base(settings)
        {
            this.manufactorRepository = manufactorRepository;
            this.macAddressRepository = macAddressRepository;
            this.hostRepository = hostRepository;
            this.tcpService = tcpService;
            this.udpService = udpService;
            this.httpService = httpService;
            this.deviceClassifier = deviceClassifier;
        }

        public event EventHandler ScanningFinished;

        public event EventHandler<DeviceScannedEventArgs> DeviceScanned;

        public async Task StartAsync(IEnumerable<IPAddress> addresses, CancellationToken cancellationToken)
        {
            foreach (var address in addresses.Where(x => x != IPAddress.None))
            {
                await ScanAndHandleCancellationAsync(address, cancellationToken);
            }

            ScanningFinished?.Invoke(this, EventArgs.Empty);
        }

        protected override async Task<PhysicalAddress> GetMacAddressAsync(IPAddress destination, CancellationToken cancellation)
        {
            return await macAddressRepository.GetMacAddressAsync(destination, cancellation);
        }

        protected override async Task<string> GetManufacturerAsync(PhysicalAddress macAddress)
        {
            return await manufactorRepository.GetManufacturerOrEmptyStringAsync(macAddress);
        }

        protected override async Task<string> GetHostnameAsync(IPAddress destination)
        {
            IPHostEntry hostEntry = await hostRepository.GetHostAsync(destination);
            return hostEntry.HostName;
        }

        protected override async Task<bool> IsDeviceAvailableInWanAsync(IPAddress destination)
        {
            const int timeout = 1000;
            var configuration = new TcpConfiguration(settings.TcpPort, timeout);
            return await tcpService.CheckPortAvailabilityAsync(destination, configuration);
        }

        protected override async Task<string> GetResponseAsync(IPAddress address, CancellationToken cancellationToken)
        {
            const int timeout = 1000;
            var configuration = new TcpConfiguration(settings.TcpPort, timeout);
            return await tcpService.GetResponseAsync(address, configuration);
        }

        protected override async Task<bool> IsDeviceAvailableInLanAsync(IPAddress destination)
        {
            const int timeout = 2000;

            if(settings.ScanUdp)
            {
                var udpConfiguration = new UdpConfiguration(settings.UdpPort, timeout);
                return await udpService.CheckPortAvailabilityAsync(destination, udpConfiguration);
            }

            if(settings.ScanTcp)
            {
                var tcpConfiguration = new TcpConfiguration(settings.TcpPort, timeout);
                return await tcpService.CheckPortAvailabilityAsync(destination, tcpConfiguration);
            }

            return false;
        }

        protected override async Task<HttpResponseMessage> ExploreWithHttpAsync(Uri uri, CancellationToken cancellationToken)
        {
            return await httpService.GetAsync(uri, cancellationToken);
        }

        protected async override Task<DeviceType> GetTypeAsync(Device device)
        {
            return await deviceClassifier.ClassifyAsync(device);
        }

        private async Task ScanAndHandleCancellationAsync(IPAddress destination, CancellationToken cancellationToken)
        {
            try
            {
                Device scannedDevice = await ScanSpecificIpAsync(destination, cancellationToken);

                if (scannedDevice != null)
                {
                    DeviceScanned?.Invoke(this, new DeviceScannedEventArgs(scannedDevice));
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine($"Operation cancelled for {destination}");
            }
        }
    }
}
