using System;

namespace IpScanner.Helpers.Contracts
{
    public interface ILogger<T>
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message, Exception exception = null);
        void LogCritical(string message, Exception exception = null);
    }
}
