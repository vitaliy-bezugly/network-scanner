using IpScanner.Helpers.Configurations.Abstract;

namespace IpScanner.Helpers.Configurations
{
    public class TcpConfiguration : ConfigurationBase
    {
        public TcpConfiguration(int port, int timeout) : base(timeout)
        {
            Port = port;
        }

        public int Port { get; }
    }
}
