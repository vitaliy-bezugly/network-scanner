using IpScanner.ViewModels.Bars;
using IpScanner.ViewModels.Menus;
using IpScanner.ViewModels.Options;
using IpScanner.ViewModels.Submenus;
using Microsoft.Extensions.DependencyInjection;

namespace IpScanner.ViewModels.Configurators
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureViewModels(this IServiceCollection services)
        {
            services.AddSingleton<MenuViewModel>();
            services.AddSingleton<ScanPageViewModel>();
            services.AddSingleton<DetailsPageViewModel>();
            services.AddSingleton<FavoritesViewModel>();

            services.AddTransient<OptionsPageViewModel>();
            services.AddSingleton<ColorThemePageViewModel>();
            services.AddTransient<PerformancePageViewModel>();
            services.AddTransient<ResourcesPageViewModel>();
            services.AddTransient<PeripheralPageViewModel>();

            services.AddSingleton<InputBarViewModel>();
            services.AddSingleton<ProgressBarViewModel>();
            services.AddSingleton<ActionBarViewModel>();

            services.AddSingleton<ContextSubmenuViewModel>();
            services.AddSingleton<ToolsSubmenuViewModel>();
            services.AddSingleton<CopySubmenuViewModel>();

            services.AddSingleton<ViewMenuViewModel>();
            services.AddSingleton<FileMenuViewModel>();
            services.AddSingleton<SettingsMenuViewModel>();
            services.AddSingleton<HelpMenuViewModel>();

            return services;
        }
    }
}
