using System.Net;
using System.Threading.Tasks;

namespace IpScanner.Domain.Contracts
{
    public interface IHostRepository
    {
        Task<IPHostEntry> GetHostAsync(IPAddress address);
    }
}
