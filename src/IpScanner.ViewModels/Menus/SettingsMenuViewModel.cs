using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Helpers.Markers;
using IpScanner.Services.Abstract;
using System.Threading.Tasks;
using Windows.Globalization;

namespace IpScanner.ViewModels.Menus
{
    public partial class SettingsMenuViewModel : ObservableObject
    {
        private readonly IOptionsPageMarker optionsPage;
        private readonly IModalsService modalsService;
        private readonly INavigationService navigationService;
        private readonly ILocalizationService localizationService;
        private readonly IAppWindowManager appWindowManager;

        public SettingsMenuViewModel(INavigationService navigationService, 
            ILocalizationService localizationService, 
            IModalsService modalsService,
            IOptionsPageMarker optionsPage,
            IAppWindowManager appWindowManager)
        {
            this.navigationService = navigationService;
            this.localizationService = localizationService;
            this.modalsService = modalsService;
            this.optionsPage = optionsPage;
            this.appWindowManager = appWindowManager;
        }

        public IRelayCommand ShowOptionsDialogCommand => new AsyncRelayCommand(ShowOptionsDialog, CanShowOptionsPage);

        [RelayCommand]
        private async Task ChangeLanguageAsync(string language)
        {
            await localizationService.SetLanguageAsync(new Language(language));
            navigationService.ReloadChildPages();
            navigationService.ReloadMainPage();
        }

        private async Task ShowOptionsDialog()
        {
            await modalsService.ShowPageAsync(optionsPage.GetType());
        }

        private bool CanShowOptionsPage()
        {
            return appWindowManager.Contains(optionsPage.GetType()) == false;
        }
    }
}
