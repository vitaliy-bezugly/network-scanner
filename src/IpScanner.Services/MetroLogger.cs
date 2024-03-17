using IpScanner.Helpers.Contracts;
using MetroLog;
using System;

namespace IpScanner.Services
{
    internal class MetroLogger<T> : ILogger<T>
    {
        private readonly ILogger logger;

        public MetroLogger(ILogManager logManager)
        {
            logger = logManager.GetLogger<T>();
        }

        public void LogCritical(string message, Exception exception = null)
        {
            logger.Fatal(message, exception);
        }

        public void LogError(string message, Exception exception = null)
        {
            logger.Error(message, exception);
        }

        public void LogInformation(string message)
        {
            logger.Info(message);
        }

        public void LogWarning(string message)
        {
            logger.Warn(message);
        }
    }
}
