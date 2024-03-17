using System;
using IpScanner.Models.Enums;

namespace IpScanner.Infrastructure.Entities
{
    public class DeviceEntity
    {
        public DeviceStatus Status { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Manufacturer { get; set; }
        public string MacAddress { get; set; }
        public string Comments { get; set; }
        public bool Favorite { get; set; }
        public DateTime ScannedDate { get; set; } = DateTime.UtcNow;
        public ServiceEntity Service { get; set; }
        public DeviceType Type { get; set; }
    }
}
