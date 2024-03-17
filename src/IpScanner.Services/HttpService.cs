using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IpScanner.Domain.Contracts;

namespace IpScanner.Services
{
    internal class HttpService : IHttpService
    {
        public async Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken cancellationToken)
        {
			try
			{
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(uri, cancellationToken);
                    return response;
                }
            }
			catch (HttpRequestException)
			{
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
			}
        }
    }
}
