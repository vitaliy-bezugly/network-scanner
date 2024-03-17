using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using CsvHelper;
using IpScanner.Domain;
using IpScanner.Helpers;
using IpScanner.Infrastructure.Configurators;
using IpScanner.Services.Abstract;
using IpScanner.Services.Configurators;
using IpScanner.Ui.Configurators;
using IpScanner.ViewModels.Configurators;
using MetroLog.Targets;
using MetroLog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using IpScanner.Helpers.Contracts;

namespace IpScanner.Ui
{
    sealed partial class App : Application
    {
        private readonly IServiceCollection serviceCollection;
        private readonly IConfiguration configuration;
        private IServiceProvider serviceProvider;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += ApplicationUnhandledException;

            configuration = BuildConfiguration();
            serviceCollection = new ServiceCollection();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            serviceCollection.ConfigureDomainLayer()
                .ConfigureInfrastructureLayer()
                .ConfigureServices()
                .ConfigureUi(configuration)
                .ConfigureViewModels();


            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;
            }

            serviceCollection.AddSingleton(rootFrame);
            serviceProvider = serviceCollection.BuildServiceProvider();
            Ioc.Default.ConfigureServices(serviceProvider);

            serviceProvider.GetRequiredService<ISettingsService>();

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }

                Window.Current.Activate();
            }
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            SaveSettings();
            deferral.Complete();
        }

        private IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }

        private void SaveSettings()
        {
            var settingsService = Ioc.Default.GetRequiredService<ISettingsService>();
            settingsService.SaveSettings();
        }

        private void ApplicationUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // try to prevent application closing
            e.Handled = true;

            SaveSettings();
            LogError(e.Exception);
        }

        private static void LogError(Exception exception)
        {
            if (!string.IsNullOrWhiteSpace(exception?.Message))
            {
                var logger = Ioc.Default.GetRequiredService<ILogger<App>>();
                logger.LogError(exception.Message, exception);
            }
        }
    }
}
