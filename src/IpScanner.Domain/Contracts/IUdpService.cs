using IpScanner.Helpers.Configurations;
using System.Net;
using System.Threading.Tasks;

namespace IpScanner.Domain.Contracts
{
    public interface IUdpService
    {
        Task<bool> CheckPortAvailabilityAsync(IPAddress address, UdpConfiguration configuration);
    }
}
