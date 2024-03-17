using System;
using IpScanner.Domain.Abstract;
using IpScanner.Domain.Factories;
using IpScanner.Helpers;
using IpScanner.Models;
using IpScanner.Models.Args;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace IpScanner.Domain
{
    public class ConcurrentThreadScanner : INetworkScanner
    {
        private readonly List<INetworkScanner> activeScanners;
        private readonly INetworkScannerFactory scannerFactory;
        private readonly AppSettings settings;

        public ConcurrentThreadScanner(INetworkScannerFactory scannerFactory, ISettingsService settingsService)
        {
            activeScanners = new List<INetworkScanner>();
            this.scannerFactory = scannerFactory;
            settings = settingsService.Settings;
        }

        public event EventHandler ScanningFinished;
        public event EventHandler<DeviceScannedEventArgs> DeviceScanned;

        public async Task StartAsync(IEnumerable<IPAddress> addresses, CancellationToken cancellationToken)
        {
            List<List<IPAddress>> chunks = IpRange.DivideAddressesIntoChunks(addresses, settings.ScanningSpeed);
            List<INetworkScanner> scanners = scannerFactory.CreateMany(chunks.Count, DeviceScanned);

            activeScanners.Clear();
            activeScanners.AddRange(scanners);

            List<Task> scanningTasks = InitializeScanningTasks(scanners, chunks, cancellationToken);
            await Task.WhenAll(scanningTasks);

            ScanningFinished?.Invoke(this, EventArgs.Empty);
            activeScanners.Clear();
        }

        public void Pause()
        {
            foreach (var scanner in activeScanners)
            {
                scanner.Pause();
            }
        }

        public void Resume()
        {
            foreach (var scanner in activeScanners)
            {
                scanner.Resume();
            }
        }

        private List<Task> InitializeScanningTasks(List<INetworkScanner> scanners, 
            List<List<IPAddress>> addresses, 
            CancellationToken cancellationToken)
        {
            return Enumerable.Range(0, addresses.Count)
                .Select(i => scanners[i].StartAsync(addresses[i], cancellationToken))
                .ToList();
        }
    }
}
