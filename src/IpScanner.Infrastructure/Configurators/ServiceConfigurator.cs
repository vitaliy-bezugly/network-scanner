using IpScanner.Models;
using IpScanner.Domain.Contracts;
using IpScanner.Infrastructure.ContentCreators;
using IpScanner.Infrastructure.ContentCreators.Factories;
using IpScanner.Infrastructure.ContentFormatters.Factories;
using IpScanner.Infrastructure.Entities;
using IpScanner.Infrastructure.Repositories;
using IpScanner.Infrastructure.Repositories.Factories;
using Microsoft.Extensions.DependencyInjection;
using IpScanner.Domain.Validators;
using IpScanner.Infrastructure.MachineLearning;
using IpScanner.Infrastructure.MachineLearning.Factories;
using IpScanner.Infrastructure.MachineLearning.Models;
using IpScanner.Infrastructure.Repositories.Abstract;

namespace IpScanner.Infrastructure.Configurators
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureInfrastructureLayer(this IServiceCollection services)
        {
            services.AddTransient<IManufactorRepository, ManufacturerCsvRepository>();

            services.AddTransient<IMacAddressRepository, MacAddressArpRepository>();

            services.AddTransient<IHostRepository, HostDnsRepository>();

            services.AddTransient<IDeviceRepositoryFactory, DeviceRepositoryFactory>();

            services.AddTransient<IContentCreatorFactory<Device>, ContentCreatorFactory>();

            services.AddTransient<IContentFormatterFactory<DeviceEntity>, ContentFormatterFactory<DeviceEntity>>();

            services.AddTransient<DevicesJsonContentCreator>();

            services.AddTransient<DevicesXmlContentCreator>();

            services.AddTransient<DevicesCsvContentCreator>();

            services.AddTransient<DevicesHtmlContentCreator>();
                
            services.AddTransient<IHistoryRepository<IpRange>, HistoryRepository<IpRange>>();

            services.AddSingleton<IModelFactory<IpRangeModel>, IpRangeModelFactory>();

            services.AddSingleton<IModelFactory<ClassificationModel>, ClassificationModelFactory>();

            services.AddTransient<IDeviceClassifier, DeviceMlClassifier>();

            services.Decorate<IValidator<IpRange>, IpRangeMlValidator>();

            services.AddSingleton<IFeatureRepository, FeatureRepository>();

            return services;
        }
    }
}
