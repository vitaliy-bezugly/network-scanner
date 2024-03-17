using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace IpScanner.Domain.Contracts
{
    public interface IMacAddressRepository
    {
        Task<PhysicalAddress> GetMacAddressAsync(IPAddress destination, CancellationToken cancellationToken);
    }
}
