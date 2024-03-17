using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Helpers;
using IpScanner.Services.Abstract;

namespace IpScanner.ViewModels.Options
{
    public partial class PerformancePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool highAccuracy;
        [ObservableProperty]
        private int scanningSpeed;
        private readonly AppSettings settings;

        public PerformancePageViewModel(ISettingsService settingsService)
        {
            settings = settingsService.Settings;

            HighAccuracy = settings.HighAccuracy;
            ScanningSpeed = settings.ScanningSpeed;
        }

        [RelayCommand]
        private void Save()
        {
            settings.HighAccuracy = HighAccuracy;
            settings.ScanningSpeed = ScanningSpeed;
        }
    }
}
