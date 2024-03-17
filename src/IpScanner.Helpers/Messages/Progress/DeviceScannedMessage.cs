using IpScanner.Models;

namespace IpScanner.Helpers.Messages.Progress
{
    public class DeviceScannedMessage
    {
        public DeviceScannedMessage(Device device)
        {
            Device = device;
        }

        public Device Device { get; }
    }
}
