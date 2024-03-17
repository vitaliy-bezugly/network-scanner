using System;

namespace IpScanner.Models.Args
{
    public class DeviceScannedEventArgs : EventArgs
    {
        public DeviceScannedEventArgs(Device device)
        {
            ScannedDevice = device;
        }

        public Device ScannedDevice { get; }
    }
}
