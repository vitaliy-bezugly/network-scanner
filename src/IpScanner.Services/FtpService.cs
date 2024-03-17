using System.Net;
using System.Threading.Tasks;
using IpScanner.Helpers.Configurations;
using IpScanner.Services.Abstract;

namespace IpScanner.Services
{
    public class FtpService : IFtpService
    {
        public async Task<bool> ConnectAsync(FtpConfiguration configuration)
        {
            try
            {
                Task<bool> sendRequestTask = SendRequest(configuration);
                Task timeoutTask = Task.Delay(configuration.Timeout);

                return await DetermineConnectionStatusAsync(sendRequestTask, timeoutTask);
            }
            catch (WebException ex)
            {
                return IsAuthenticationError(ex);
            }
        }

        private async Task<bool> SendRequest(FtpConfiguration configuration)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(configuration.FtpAddress);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(configuration.Username, configuration.Password);

            using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
            {
                if (response.StatusCode == FtpStatusCode.OpeningData ||
                    response.StatusCode == FtpStatusCode.AccountNeeded)
                {
                    return true;
                }
            }

            return false;
        }

        private static async Task<bool> DetermineConnectionStatusAsync(Task<bool> sendRequestTask, Task timeoutTask)
        {
            var completedTask = await Task.WhenAny(sendRequestTask, timeoutTask);
            return completedTask == sendRequestTask && await AttemptConnectionAsync(sendRequestTask);
        }

        private static async Task<bool> AttemptConnectionAsync(Task<bool> sendRequestTask)
        {
            try
            {
                return await sendRequestTask;
            }
            catch (WebException)
            {
                return false;
            }
        }

        private bool IsAuthenticationError(WebException exception)
        {
            if (exception.Status == WebExceptionStatus.ProtocolError)
            {
                FtpWebResponse response = exception.Response as FtpWebResponse;
                if (response?.StatusCode == FtpStatusCode.NotLoggedIn)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
