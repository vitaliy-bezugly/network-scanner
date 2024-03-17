using IpScanner.Models.Enums;

namespace IpScanner.Infrastructure.Entities
{
    public class ServiceEntity
    {
        public ServiceType Type { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
    }
}
