using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using IpScanner.Models.Args;

namespace IpScanner.Domain.Abstract
{
    public interface INetworkScanner
    {
        event EventHandler ScanningFinished;

        event EventHandler<DeviceScannedEventArgs> DeviceScanned;

        Task StartAsync(IEnumerable<IPAddress> addresses, CancellationToken cancellationToken);

        void Pause();

        void Resume();
    }
}
