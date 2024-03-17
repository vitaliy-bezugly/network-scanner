using FluentResults;
using System.Net;

namespace IpScanner.Services.Abstract
{
    public interface ITelnetService
    {
        Result OpenTelnetSession(IPAddress address);
    }
}
