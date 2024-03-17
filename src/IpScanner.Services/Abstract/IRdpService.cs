using IpScanner.Helpers.Configurations;

namespace IpScanner.Services.Abstract
{
    public interface IRdpService
    {
        void Connect(RdpConfiguration configuration);
    }
}
