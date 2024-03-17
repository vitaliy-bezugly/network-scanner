using System;
using IpScanner.Domain.Abstract;
using IpScanner.Models.Args;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace IpScanner.Domain.Factories
{
    internal class NetworkScannerFactory : INetworkScannerFactory
    {
        private readonly IServiceProvider serviceProvider;

        public NetworkScannerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public INetworkScanner Create(EventHandler scanFinished = null, EventHandler<DeviceScannedEventArgs> deviceScanned = null)
        {
            var networkScanner = serviceProvider.GetService<NetworkScanner>();

            if (scanFinished != null)
            {
                networkScanner.ScanningFinished += scanFinished;
            }

            if (deviceScanned != null)
            {
                networkScanner.DeviceScanned += deviceScanned;
            }

            return networkScanner;
        }

        public List<INetworkScanner> CreateMany(int count, EventHandler<DeviceScannedEventArgs> deviceScanned = null, EventHandler scanFinished = null)
        {
            var scanners = new List<INetworkScanner>();
            for (int i = 0; i < count; i++)
            {
                INetworkScanner scanner = Create(scanFinished, deviceScanned);
                scanners.Add(Create(scanFinished, deviceScanned));
            }

            return scanners;
        }
    }
}
