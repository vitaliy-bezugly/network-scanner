using IpScanner.Helpers.Configurations.Abstract;

namespace IpScanner.Helpers.Configurations
{
    public class UdpConfiguration : ConfigurationBase
    {
        public UdpConfiguration(int port, int timeout) : base(timeout)
        {
            Port = port;
        }

        public int Port { get; }
    }
}
