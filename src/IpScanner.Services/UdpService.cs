using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using IpScanner.Domain.Contracts;
using IpScanner.Helpers.Configurations;

namespace IpScanner.Services
{
    public class UdpService : IUdpService
    {
        public async Task<bool> CheckPortAvailabilityAsync(IPAddress address, UdpConfiguration configuration)
        {
            using (var client = new UdpClient())
            {
                return await IsPortAvailable(client, address, configuration);
            }
        }

        private static async Task<bool> IsPortAvailable(UdpClient client, IPAddress address, UdpConfiguration configuration)
        {
            try
            {
                await SendUdpProbe(client, address, configuration.Port);

                var isReceived = await IsResponseReceivedWithinTimeout(client, configuration.Timeout);
                return isReceived;
            }
            catch (SocketException ex) when (IsConnectionResetOrAborted(ex))
            {
                return false;
            }
        }

        private static async Task SendUdpProbe(UdpClient client, IPAddress address, int port)
        {
            var remoteEndPoint = new IPEndPoint(address, port);
            await client.SendAsync(new byte[0], 0, remoteEndPoint);
        }

        private static async Task<bool> IsResponseReceivedWithinTimeout(UdpClient client, int timeout)
        {
            var receiveTask = client.ReceiveAsync();
            var delayTask = Task.Delay(timeout);

            var completedTask = await Task.WhenAny(receiveTask, delayTask);
            return completedTask == receiveTask;
        }

        private static bool IsConnectionResetOrAborted(SocketException ex)
        {
            return ex.SocketErrorCode == SocketError.ConnectionReset 
                || ex.SocketErrorCode == SocketError.ConnectionAborted;
        }
    }
}
