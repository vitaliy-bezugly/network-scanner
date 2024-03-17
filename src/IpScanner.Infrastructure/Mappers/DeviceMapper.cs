using IpScanner.Infrastructure.Entities;
using IpScanner.Models;
using System.Net;
using System.Net.NetworkInformation;

namespace IpScanner.Infrastructure.Mappers
{
    public static class DeviceMapper
    {
        public static DeviceEntity ToEntity(this Device domain)
        {
            return new DeviceEntity
            {
                Status = domain.Status,
                Name = domain.Name,
                Ip = domain.Ip.ToString(),
                Manufacturer = domain.Manufacturer,
                MacAddress = domain.MacAddress.ToString(),
                Comments = domain.Comments,
                Favorite = domain.Favorite,
                ScannedDate = domain.ScannedTime,
                Service = domain.Service?.ToEntity(),
                Type = domain.Type
            };
        }

        public static Device ToDomain(this DeviceEntity entity)
        {
            PhysicalAddress macAddress = ParseMacAddress(entity.MacAddress);
            IPAddress ipAddress = IPAddress.Parse(entity.Ip);

            return new Device(entity.Status, entity.Name, ipAddress, entity.Manufacturer, macAddress, 
                entity.Comments, entity.Favorite, entity.Type, entity.ScannedDate, entity.Service?.ToModel());
        }

        private static PhysicalAddress ParseMacAddress(string macAddress)
        {
            return PhysicalAddress.None.ToString() == macAddress
                ? PhysicalAddress.None
                : PhysicalAddress.Parse(macAddress);
        }
    }
}
