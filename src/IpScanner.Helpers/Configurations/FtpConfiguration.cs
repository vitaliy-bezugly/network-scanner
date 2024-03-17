using IpScanner.Helpers.Configurations.Abstract;

namespace IpScanner.Helpers.Configurations
{
    public class FtpConfiguration : ConfigurationBase
    {
        public FtpConfiguration(string ftpAddress, string username, string password, int timeout) : base(timeout)
        {
            FtpAddress = ftpAddress;
            Username = username;
            Password = password;
        }

        public string FtpAddress { get; }
        public string Username { get; }
        public string Password { get; }
    }
}
