using IpScanner.Models;
using IpScanner.Models.Enums;
using System.Threading.Tasks;

namespace IpScanner.Domain.Contracts
{
    public interface IDeviceClassifier
    {
        Task<DeviceType> ClassifyAsync(Device device);
    }
}
