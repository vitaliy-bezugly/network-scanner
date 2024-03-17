using IpScanner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.Repositories
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<Device>> GetDevicesOrNullAsync();
        Task SaveDevicesAsync(IEnumerable<Device> devices);
        Task AddDeviceAsync(Device device);
        Task RemoveDeviceAsync(Device device);
        Task UpdateDeviceAsync(Device device);
    }
}
