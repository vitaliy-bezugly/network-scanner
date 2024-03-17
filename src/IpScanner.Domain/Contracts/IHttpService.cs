using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IpScanner.Domain.Contracts
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken cancellationToken);
    }
}
