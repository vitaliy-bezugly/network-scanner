using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using IpScanner.Services.Abstract;
using Microsoft.Extensions.Configuration;
using IpScanner.Helpers.Constants;

namespace IpScanner.ViewModels.Menus
{
    public partial class HelpMenuViewModel : ObservableObject
    {
        private readonly IDialogService dialogService;
        private readonly IUriOpenerService browserService;
        private readonly ILocalizationService localizationService;
        private readonly IConfiguration configuration;

        public HelpMenuViewModel(IUriOpenerService browserService, 
            IDialogService dialogService, 
            ILocalizationService localizationService,
            IConfiguration configuration)
        {
            this.browserService = browserService;
            this.dialogService = dialogService;
            this.localizationService = localizationService;
            this.configuration = configuration;
        }

        [RelayCommand]
        public async Task OpenContentsAsync()
        {
            var uri = new Uri(configuration["Urls:Contents"]);
            await browserService.OpenUriAsync(uri);
        }

        [RelayCommand]
        public async Task OpenBugReportAsync()
        {
            var uri = new Uri(configuration["Urls:BugReport"]);
            await browserService.OpenUriAsync(uri);
        }

        [RelayCommand]
        public async Task OpenRequestFeatureAsync()
        {
            var uri = new Uri(configuration["Urls:RequestFeature"]);
            await browserService.OpenUriAsync(uri);
        }

        [RelayCommand]
        public async Task OpenCommunityAsync()
        {
            var uri = new Uri(configuration["Urls:Community"]);
            await browserService.OpenUriAsync(uri);
        }

        [RelayCommand]
        public async Task OpenAboutAsync()
        {
            string aboutTitle = localizationService.GetString(LocalizationKeys.AboutTitle);
            string version = GetAppVersion();

            await dialogService.ShowMessageAsync(aboutTitle, $"Advanced IP Scanner {version}");
        }

        [RelayCommand]
        public async Task DownloadRadminAsync()
        {
            var uri = new Uri(configuration["Urls:Radmin"]);
            await browserService.OpenUriAsync(uri);
        }

        public static string GetAppVersion()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }
    }
}
