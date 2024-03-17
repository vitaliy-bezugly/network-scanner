using System;
using IpScanner.Domain.Abstract;
using IpScanner.Models.Args;
using System.Collections.Generic;

namespace IpScanner.Domain.Factories
{
    public interface INetworkScannerFactory
    {
        INetworkScanner Create(EventHandler scanFinished = null, EventHandler<DeviceScannedEventArgs> deviceScanned = null);
        List<INetworkScanner> CreateMany(int count, EventHandler<DeviceScannedEventArgs> deviceScanned = null, EventHandler scanFinished = null);
    }
}
