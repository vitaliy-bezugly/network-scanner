using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using IpScanner.Models.Enums;
using IpScanner.Models;
using IpScanner.Helpers.Extensions;
using System;
using System.Net.Http;
using IpScanner.Helpers;

namespace IpScanner.Domain.Abstract
{
    public abstract class NetworkScannerBase
    {
        protected readonly AppSettings settings;
        private TaskCompletionSource<bool> pauseTcs = new TaskCompletionSource<bool>();

        public NetworkScannerBase(ISettingsService settingsService)
        {
            pauseTcs.SetResult(true);
            settings = settingsService.Settings;
        }

        public virtual void Pause()
        {
            pauseTcs = new TaskCompletionSource<bool>();
        }

        public virtual void Resume()
        {
            pauseTcs.SetResult(true);
        }

        protected abstract Task<string> GetResponseAsync(IPAddress address, CancellationToken cancellationToken);

        protected abstract Task<bool> IsDeviceAvailableInWanAsync(IPAddress destination);

        protected abstract Task<bool> IsDeviceAvailableInLanAsync(IPAddress destination);

        protected abstract Task<PhysicalAddress> GetMacAddressAsync(IPAddress destination, CancellationToken cancellation);

        protected abstract Task<string> GetManufacturerAsync(PhysicalAddress macAddress);

        protected abstract Task<string> GetHostnameAsync(IPAddress destination);

        protected abstract Task<DeviceType> GetTypeAsync(Device device);

        protected abstract Task<HttpResponseMessage> ExploreWithHttpAsync(Uri address, CancellationToken cancellationToken);

        protected async Task<Device> ScanSpecificIpAsync(IPAddress destination, CancellationToken cancellationToken)
        {
            if (destination.IsClassAorB())
            {
                return await ScanIpInWanAsync(destination, cancellationToken);
            }

            return await ScanIpInLanAsync(destination, cancellationToken);
        }

        private async Task<Device> ScanIpInWanAsync(IPAddress destination, CancellationToken cancellationToken)
        {
            bool isAvailableInWan = await IsDeviceAvailableInWanAsync(destination);
            if (isAvailableInWan == false)
                return new Device(destination);

            string name = await GetHostnameAsync(destination);
            if (await WaitOrGetCancelIfRequestedAsync(cancellationToken)) return null;

            string response = await GetResponseAsync(destination, cancellationToken);
            if (await WaitOrGetCancelIfRequestedAsync(cancellationToken)) return null;

            var service = new Service(ServiceType.Http, name, new Uri($"http://{destination}"));
            string manufacturer = FetchManufacturer(response);
            string comments = string.IsNullOrEmpty(response) ? string.Empty : "HTTP";

            return new Device(DeviceStatus.Online, name, destination, manufacturer, PhysicalAddress.None, comments, DateTime.UtcNow, service)
            {
                Type = DeviceType.Software
            };
        }

        private async Task<Device> ScanIpInLanAsync(IPAddress destination, CancellationToken cancellationToken)
        {
            PhysicalAddress macAddress = await GetMacAddressAsync(destination, cancellationToken);
            if (await WaitOrGetCancelIfRequestedAsync(cancellationToken)) return null;

            if (ShouldAbortScanning(macAddress))
                return new Device(destination);

            DeviceStatus status = await DetermineDeviceStatusAsync(destination, macAddress); 
            if (await WaitOrGetCancelIfRequestedAsync(cancellationToken)) return null;

            if (status == DeviceStatus.Unknown) return new Device(destination);

            string manufacturer = await FetchManufacturer(macAddress);
            if (await WaitOrGetCancelIfRequestedAsync(cancellationToken)) return null;

            string name = await GetHostnameAsync(destination);
            if (await WaitOrGetCancelIfRequestedAsync(cancellationToken)) return null;

            Service service = await FetchService(destination, cancellationToken);
            return await CreateDevice(status, name, destination, manufacturer, macAddress, service, cancellationToken);
        }

        private async Task<DeviceStatus> DetermineDeviceStatusAsync(IPAddress address, PhysicalAddress macAddress)
        {
            if (macAddress == PhysicalAddress.None)
            {
                var isAvailableInLan = await IsDeviceAvailableInLanAsync(address);
                if (isAvailableInLan == false)
                {
                    return DeviceStatus.Unknown;
                }

                return DeviceStatus.Offline;
            }

            return DeviceStatus.Online;
        }

        private async Task<Device> CreateDevice(DeviceStatus status, string name, IPAddress destination, string manufacturer, PhysicalAddress macAddress, Service service, CancellationToken cancellationToken)
        {
            var device = new Device(status, name, destination, manufacturer, macAddress, string.Empty, DateTime.UtcNow, service);
            if (settings.EnableClassificationMachineLearning)
            {
                device.Type = await GetTypeAsync(device);
            }

            if (await WaitOrGetCancelIfRequestedAsync(cancellationToken)) return null;

            return device;
        }

        private async Task<bool> WaitOrGetCancelIfRequestedAsync(CancellationToken cancellationToken)
        {
            await pauseTcs.Task;
            return cancellationToken.IsCancellationRequested;
        }

        private bool ShouldAbortScanning(PhysicalAddress macAddress)
        {
            return macAddress == PhysicalAddress.None && !settings.HighAccuracy;
        }

        private async Task<string> FetchManufacturer(PhysicalAddress macAddress)
        {
            return macAddress != PhysicalAddress.None
                   ? await GetManufacturerAsync(macAddress)
                   : string.Empty;
        }

        private async Task<Service> FetchService(IPAddress destination, CancellationToken cancellationToken)
        {
            if (!settings.ScanHttp) return null;

            var uri = new Uri($"http://{destination}");
            HttpResponseMessage response = await ExploreWithHttpAsync(uri, cancellationToken);

            return response.IsSuccessStatusCode
                   ? new Service(ServiceType.Http, destination.ToString(), uri)
                   : null;
        }

        private static string FetchManufacturer(string responseAsString)
        {
            var response = string.IsNullOrEmpty(responseAsString) ? new HttpResponse() : HttpResponse.Parse(responseAsString);
            var manufacturer = response.Headers.ContainsKey("Server") ? response.Headers["Server"] : string.Empty;

            return manufacturer;
        }
    }
}
