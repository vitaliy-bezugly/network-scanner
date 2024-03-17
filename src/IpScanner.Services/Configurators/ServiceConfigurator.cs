using IpScanner.Domain.Contracts;
using IpScanner.Helpers;
using IpScanner.Models;
using IpScanner.Services.Abstract;
using IpScanner.Services.Containers;
using MetroLog.Targets;
using MetroLog;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using IpScanner.Helpers.Contracts;

namespace IpScanner.Services.Configurators
{
    public static class ServiceConfigurator
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IDataGridService<Device>, DeviceDataGridService>();

            services.AddSingleton<IFramesContainer, FramesContainer>();

            services.AddTransient<IIpAddressParserService, IpAddressParserService>();

            services.AddTransient<INavigationService, NavigationService>();

            services.AddTransient<ILocalizationService, LocalizationService>();

            services.AddTransient<IDialogService, DialogService>();

            services.AddTransient<IApplicationExitService, ApplicationExitService>();

            services.AddSingleton<IPanelContainer, PanelContainer>();

            services.AddSingleton<IChildElementsContainer, ChildElementsContainer>();
            services.AddTransient<IColorThemeService, ColorThemeService>();
            services.AddSingleton<IAppWindowManager, AppWindowManager>();

            services.AddTransient<IModalsService, ModalsService>();

            services.AddSingleton<ISettingsService, SettingsService>();

            services.AddTransient<IClipboardService, ClipboardService>();

            services.AddTransient<IPrintService<Device>, DevicePrintService>();

            services.AddTransient<IContentBarCustomizationService, ContentBarCustomizationService>();

            services.AddTransient<IFileService, FileService>();

            services.AddTransient<IUriOpenerService, UriOpenerService>();

            services.AddTransient<IFtpService, FtpService>();

            services.AddTransient<IRdpService, RdpService>();

            services.AddScoped<IWakeOnLanService, WakeOnLanService>();

            services.AddTransient<ICmdService, CmdService>();

            services.AddTransient<ITelnetService, TelnetService>();

            services.AddTransient<IUdpService, UdpService>();

            services.AddTransient<IHttpService, HttpService>();

            services.AddTransient<ITcpService, TcpService>();

            services.AddTransient<ISpeechRecognizerService, SpeechRecognizerService>();

            services.AddTransient<ISpeechSynthesizerService, SpeechSynthesizerService>();

            services.AddTransient<IGptService, GptService>();

            services.AddTransient<IReportCreatorService<List<Device>>, DeviceReportCreatorService>();
            services.Decorate<IReportCreatorService<List<Device>>, DeviceReportCreatorServiceProxy>();

            services.AddTransient<IPromptCreatorService<List<Device>>, DevicePromptCreatorService>();

            ConfigureLogging(services);
            return services;
        }

        private static void ConfigureLogging(IServiceCollection services)
        {
            var logManager = LogManagerFactory.DefaultLogManager;
            var defaultConfig = new LoggingConfiguration();
            defaultConfig.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget());

            services.AddSingleton(logManager);
            services.AddTransient(typeof(ILogger<>), typeof(MetroLogger<>));
        }
    }
}
