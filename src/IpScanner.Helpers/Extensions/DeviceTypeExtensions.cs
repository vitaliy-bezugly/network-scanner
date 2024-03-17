using IpScanner.Models.Enums;

namespace IpScanner.Helpers.Extensions
{
    public static class DeviceTypeExtensions
    {
        public static DeviceType ConvertStringToDeviceType(this string deviceTypeString)
        {
            switch (deviceTypeString)
            {
                case "Software":
                    return DeviceType.Software;
                case "Router":
                    return DeviceType.Router;
                case "Phone":
                    return DeviceType.Mobile;
                case "TV":
                    return DeviceType.Tv;
                case "PC":
                    return DeviceType.Computer;
                case "Printer":
                    return DeviceType.Printer;
                case "IoT":
                    return DeviceType.Iot;
                case "Undefined":
                    return DeviceType.Undefined;
                default:
                    return DeviceType.Undefined;
            }
        }
    }
}
