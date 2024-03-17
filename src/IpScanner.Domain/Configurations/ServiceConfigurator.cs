using IpScanner.Models;
using IpScanner.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;
using IpScanner.Domain.Factories;
using IpScanner.Domain.Abstract;

namespace IpScanner.Domain
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureDomainLayer(this IServiceCollection services)
        {
            services.AddTransient<IValidator<IpRange>, IpRangeValidator>();

            services.AddTransient<NetworkScanner>();

            services.AddTransient<INetworkScanner, ConcurrentThreadScanner>();

            services.AddTransient<INetworkScannerFactory, NetworkScannerFactory>();

            return services;
        }
    }
}
