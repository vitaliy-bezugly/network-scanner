using IpScanner.Helpers.Configurations;
using System.Threading.Tasks;

namespace IpScanner.Services.Abstract
{
    public interface IFtpService
    {
        Task<bool> ConnectAsync(FtpConfiguration configuration);
    }
}
