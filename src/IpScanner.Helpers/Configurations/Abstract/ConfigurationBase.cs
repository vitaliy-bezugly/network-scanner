namespace IpScanner.Helpers.Configurations.Abstract
{
    public abstract class ConfigurationBase
    {
        protected ConfigurationBase(int timeout)
        {
            Timeout = timeout;
        }

        public int Timeout { get; }
    }
}
