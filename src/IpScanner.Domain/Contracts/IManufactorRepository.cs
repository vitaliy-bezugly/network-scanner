using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IpScanner.Domain.Contracts
{
    public interface IManufactorRepository
    {
        Task<string> GetManufacturerOrEmptyStringAsync(PhysicalAddress macAddress);
    }
}
