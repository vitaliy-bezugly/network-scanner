using System;
using System.Linq;
using FluentResults;
using Windows.Storage;
using System.Threading.Tasks;
using IpScanner.Models.Enums;
using System.Collections.Generic;
using IpScanner.Models;
using IpScanner.Infrastructure.ContentFormatters;
using IpScanner.Infrastructure.ContentFormatters.Factories;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Mappers;
using IpScanner.Helpers.Extensions;
using IpScanner.Infrastructure.ContentCreators.Factories;
using IpScanner.Infrastructure.ContentCreators;

namespace IpScanner.Infrastructure.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly StorageFile file;
        private readonly IContentCreatorFactory<Device> contentCreatorFactory;
        private readonly IContentFormatterFactory<DeviceEntity> contentFormatterFactory;

        public DeviceRepository(StorageFile file, 
            IContentCreatorFactory<Device> contentCreatorFactory,
            IContentFormatterFactory<DeviceEntity> contentFormatterFactory)
        {
            this.file = file;
            this.contentCreatorFactory = contentCreatorFactory;
            this.contentFormatterFactory = contentFormatterFactory;
        }

        public async Task<IEnumerable<Device>> GetDevicesOrNullAsync()
        {
            string content = await FileIO.ReadTextAsync(file);
            if(string.IsNullOrEmpty(content))
            {
                return Enumerable.Empty<Device>();
            }

            IContentFormatter<DeviceEntity> formatter = CreateContentFormatter(file.FileType);
            return DeserializeContent(content, formatter);
        }

        public async Task SaveDevicesAsync(IEnumerable<Device> devices)
        {
            string content = GenerateFileContent(devices, file.FileType);
            await FileIO.WriteTextAsync(file, content);
        }

        public async Task AddDeviceAsync(Device device)
        {
            List<Device> currentDevices = (await GetDevicesOrNullAsync()).ToList();
            currentDevices.Add(device);

            await SaveDevicesAsync(currentDevices);
        }

        public async Task RemoveDeviceAsync(Device device)
        {
            List<Device> currentDevices = (await GetDevicesOrNullAsync()).ToList();

            Device deviceToRemove = currentDevices.FirstOrDefault(d => d.Ip.Equals(device.Ip)) 
                ?? throw new ArgumentException("Device not found");

            currentDevices.Remove(deviceToRemove);
            await SaveDevicesAsync(currentDevices);
        }

        public async Task UpdateDeviceAsync(Device device)
        {
            List<Device> currentDevices = (await GetDevicesOrNullAsync()).ToList();

            Device destination = currentDevices.FirstOrDefault(d => d.Ip.Equals(device.Ip))
                ?? throw new ArgumentException("Device not found");

            currentDevices.Remove(destination);
            currentDevices.Add(device);

            await SaveDevicesAsync(currentDevices);
        }

        private IContentFormatter<DeviceEntity> CreateContentFormatter(string fileType)
        {
            ContentFormat format = fileType.ParseToContentFormat();
            IContentFormatter<DeviceEntity> formatter = contentFormatterFactory.Create(format);

            return formatter;
        }

        private static IEnumerable<Device> DeserializeContent(string content, IContentFormatter<DeviceEntity> contentFormatter)
        {
            IResult<IEnumerable<DeviceEntity>> result = contentFormatter.FormatContentAsCollection(content);
            if (result.IsFailed)
            {
                return null;
            }

            IEnumerable<DeviceEntity> scannedDevices = result.Value;
            return scannedDevices.Select(x => x.ToDomain());
        }

        private string GenerateFileContent(IEnumerable<Device> devices, string fileType)
        {
            ContentFormat format = fileType.ParseToContentFormat();
            IContentCreator<Device> contentCreator = contentCreatorFactory.Create(format);

            return contentCreator.CreateContent(devices);
        }
    }
}
