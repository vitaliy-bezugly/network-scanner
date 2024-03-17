using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using IpScanner.Helpers.Configurations;
using IpScanner.Domain.Contracts;

namespace IpScanner.Services
{
    internal class TcpService : ITcpService
    {
        public async Task<bool> CheckPortAvailabilityAsync(IPAddress address, TcpConfiguration configuration)
        {
            using (var tcpClient = new TcpClient())
            {
                Task<TcpClient> connectionTask = ConnectToServerAsync(address, configuration.Port);
                var timeoutTask = GetTimeoutTask(configuration.Timeout);

                return await DetermineConnectionStatusAsync(connectionTask, timeoutTask);
            }
        }

        public async Task<string> GetResponseAsync(IPAddress address, TcpConfiguration configuration)
        {
            using (TcpClient tcpClient = await ConnectToServerAsync(address, configuration.Port))
            {
                return await ReadHttpResponseAsync(tcpClient.GetStream(), address);
            }
        }

        private static Task GetTimeoutTask(int timeout)
        {
            return Task.Delay(timeout);
        }

        private static async Task<TcpClient> ConnectToServerAsync(IPAddress address, int port)
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(address, port);

            return tcpClient;
        }

        private static async Task<string> ReadHttpResponseAsync(NetworkStream networkStream, IPAddress address)
        {
            await SendHttpGetRequestAsync(networkStream, address);
            return await ReceiveResponseAsync(networkStream);
        }

        private static async Task SendHttpGetRequestAsync(NetworkStream networkStream, IPAddress address)
        {
            var getRequest = $"GET / HTTP/1.1\r\nHost: {address}\r\nConnection: Close\r\n\r\n";
            var getRequestBytes = Encoding.ASCII.GetBytes(getRequest);
            await networkStream.WriteAsync(getRequestBytes, 0, getRequestBytes.Length);
        }

        private static async Task<string> ReceiveResponseAsync(NetworkStream networkStream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer, 0, bytesRead);
        }

        private static async Task<bool> DetermineConnectionStatusAsync(Task connectTask, Task delayTask)
        {
            var completedTask = await Task.WhenAny(connectTask, delayTask);
            return completedTask == connectTask && await AttemptConnectionAsync(connectTask);
        }

        private static async Task<bool> AttemptConnectionAsync(Task connectTask)
        {
            try
            {
                await connectTask;
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}
