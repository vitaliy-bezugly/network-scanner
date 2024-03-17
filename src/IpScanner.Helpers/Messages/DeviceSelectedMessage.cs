using IpScanner.Models;

namespace IpScanner.Helpers.Messages
{
    public class DeviceSelectedMessage
    {
        public DeviceSelectedMessage(Device selectedDevice)
        {
            Device = selectedDevice;
        }

        public Device Device { get; }
    }
}
