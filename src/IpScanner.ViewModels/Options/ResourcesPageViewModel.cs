using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Helpers;
using IpScanner.Helpers.Messages.Ui.Enabled;
using IpScanner.Helpers.Messages.Ui.Visibility;

namespace IpScanner.ViewModels.Options
{
    public partial class ResourcesPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool scanHttp;
        [ObservableProperty]
        private bool scanTcp;
        [ObservableProperty]
        private bool scanUdp;
        [ObservableProperty]
        private int udpPort;
        [ObservableProperty]
        private int tcpPort;
        [ObservableProperty]
        private bool scanDateTime;
        [ObservableProperty]
        private bool enableValidationMachineLearning;
        [ObservableProperty]
        private bool enableClassificationMachineLearning;
        [ObservableProperty]
        private bool enableGpt;
        [ObservableProperty]
        private string apiKey;
        private readonly AppSettings settings;
        private readonly IMessenger messenger;

        public ResourcesPageViewModel(ISettingsService settingsService, IMessenger messenger)
        {
            this.messenger = messenger;
            settings = settingsService.Settings;
            ScanHttp = settings.ScanHttp;
            ScanDateTime = settings.ScanDateTime;
            ScanTcp = settings.ScanTcp;
            ScanUdp = settings.ScanUdp;
            UdpPort = settings.UdpPort;
            TcpPort = settings.TcpPort;
            EnableGpt = settings.EnableGpt;
            ApiKey = settings.GptApiKey;
            EnableValidationMachineLearning = settings.EnableValidationMachineLearning;
            EnableClassificationMachineLearning = settings.EnableClassificationMachineLearning;
        }

        [RelayCommand]
        private void Save()
        {
            settings.UdpPort = UdpPort;
            settings.TcpPort = TcpPort;
            settings.ScanHttp = ScanHttp;
            settings.ScanTcp = ScanTcp;
            settings.ScanUdp = ScanUdp;
            settings.EnableValidationMachineLearning = EnableValidationMachineLearning;
            settings.EnableClassificationMachineLearning = EnableClassificationMachineLearning;
            settings.GptApiKey = ApiKey;
            SaveGptEnableStatus();
            SaveDateTimeEnableStatus();
        }

        private void SaveDateTimeEnableStatus() 
        { 
            settings.ScanDateTime = ScanDateTime;
            messenger.Send(new DatetimeVisibilityMessage(ScanDateTime));
        }

        private void SaveGptEnableStatus()
        {
            settings.EnableGpt = EnableGpt;
            messenger.Send(new GptEnabledMessage(EnableGpt));
        }
    }
}
