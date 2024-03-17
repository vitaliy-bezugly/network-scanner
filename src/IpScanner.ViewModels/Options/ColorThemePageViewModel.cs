using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Helpers;
using IpScanner.Services.Abstract;
using Windows.UI.Xaml;

namespace IpScanner.ViewModels.Options
{
    public partial class ColorThemePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string selectedTheme;
        private readonly AppSettings settings;
        private readonly IColorThemeService colorThemeService;

        public ColorThemePageViewModel(IColorThemeService colorThemeService, ISettingsService settingsService)
        {
            settings = settingsService.Settings;
            this.colorThemeService = colorThemeService;
            SelectedTheme = settings.ColorTheme;
        }

        partial void OnSelectedThemeChanged(string value)
        {
            ChangeTheme();
        }

        private void ChangeTheme()
        {
            settings.ColorTheme = SelectedTheme;
            colorThemeService.SetColorTheme(MapToElementTheme(SelectedTheme));
        }

        private ElementTheme MapToElementTheme(string theme)
        {
            switch (theme.ToLower())
            {
                case "light":
                    return ElementTheme.Light;
                case "dark":
                    return ElementTheme.Dark;
                default:
                    return ElementTheme.Default;
            }
        }
    }
}
