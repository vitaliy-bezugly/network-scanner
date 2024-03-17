using Windows.Storage;
using IpScanner.Models;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.ContentFormatters.Factories;
using IpScanner.Infrastructure.ContentCreators.Factories;

namespace IpScanner.Infrastructure.Repositories.Factories
{
    public class DeviceRepositoryFactory : IDeviceRepositoryFactory 
    {
        private readonly IContentCreatorFactory<Device> contentCreatorFactory;
        private readonly IContentFormatterFactory<DeviceEntity> contentFormatterFactory;

        public DeviceRepositoryFactory(IContentCreatorFactory<Device> contentCreatorFactory, IContentFormatterFactory<DeviceEntity> contentFormatterFactory)
        {
            this.contentCreatorFactory = contentCreatorFactory;
            this.contentFormatterFactory = contentFormatterFactory;
        }

        public IDeviceRepository CreateWithFile(StorageFile file)
        {
            return new DeviceRepository(file, contentCreatorFactory, contentFormatterFactory);
        }
    }
}
