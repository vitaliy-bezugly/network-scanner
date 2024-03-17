using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using IpScanner.Ui.Pages.Options;
using IpScanner.Helpers.Markers;
using Microsoft.Extensions.Configuration;

namespace IpScanner.Ui.Configurators
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureUi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<OptionsPage>();
            services.AddTransient<IOptionsPageMarker, OptionsRootPage>();
            services.AddSingleton<IMessenger, StrongReferenceMessenger>();
            services.AddSingleton(configuration);

            return services;
        }
    }
}
