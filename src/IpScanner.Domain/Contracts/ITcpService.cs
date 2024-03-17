using System.Net;
using System.Threading.Tasks;
using IpScanner.Helpers.Configurations;

namespace IpScanner.Domain.Contracts
{
    public interface ITcpService
    {
        Task<bool> CheckPortAvailabilityAsync(IPAddress address, TcpConfiguration configuration);
        Task<string> GetResponseAsync(IPAddress address, TcpConfiguration configuration);
    }
}
