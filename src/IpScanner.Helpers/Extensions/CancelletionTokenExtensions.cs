using System;
using System.Threading;

namespace IpScanner.Helpers.Extensions
{
    public static class CancelletionTokenExtensions
    {
        public static CancellationToken CreateLinkedCancellationTokenWithTimeout(this CancellationToken cancellationToken, TimeSpan timeout)
        {
            CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);

            return cts.Token;
        }
    }
}
