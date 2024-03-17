using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IpScanner.Models.Enums;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Models;
using IpScanner.Helpers.Messages.Scanning;
using IpScanner.Helpers.Messages.IpRange;
using IpScanner.Helpers.Messages.Toolbar;
using IpScanner.Helpers.Enums;
using IpScanner.Helpers.Messages.Ui.Visibility;
using IpScanner.Helpers.Messages;
using IpScanner.Services.Abstract;
using IpScanner.Helpers;
using Windows.Storage;

namespace IpScanner.ViewModels.Bars
{
    public partial class ActionBarViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool paused;
        [ObservableProperty]
        private bool stopping;
        [ObservableProperty]
        private bool scanning;
        [ObservableProperty]
        private bool showMiscellaneous;
        [ObservableProperty]
        private bool showActions;
        [ObservableProperty]
        private Device selectedDevice;
        [ObservableProperty]
        private FavoritesViewModel favoritesViewModel;
        private readonly IMessenger messenger;
        private readonly AppSettings settings;

        public ActionBarViewModel(IMessenger messenger, ISettingsService settingsService, FavoritesViewModel favoritesViewModel)
        {
            this.messenger = messenger;
            this.favoritesViewModel = favoritesViewModel;
            Paused = false;
            Scanning = false;
            Stopping = false;
            settings = settingsService.Settings;
            ShowMiscellaneous = settingsService.Settings.ShowMiscellaneousToolbar;
            ShowActions = settingsService.Settings.ShowActionsToolbar;

            RegisterMessages(messenger);
        }

        [RelayCommand]
        private void Scan()
        {
            Scanning = true;
            ScanningSource source = settings.FavoritesSelected ? ScanningSource.Favorites : ScanningSource.IpRange;
            messenger.Send(new StartScanningMessage<StorageFile>(source));
        }

        [RelayCommand]
        private void Rescan()
        {
            if(SelectedDevice == null)
            {
                return;
            }   

            Scanning = true;
            ScanningSource source = settings.FavoritesSelected ? ScanningSource.Favorites : ScanningSource.IpRange;
            messenger.Send(new RescanSelectedItemMessage(source));
        }

        [RelayCommand]
        private void SetSubnetMask()
        {
            messenger.Send(new SetSubnetMaskMessage("192.168.0.1-254, 26.0.0.1-254"));
        }

        [RelayCommand]
        private void SetSubnetMaskClassC()
        {
            messenger.Send(new SetSubnetMaskMessage("192.168.0.1-254"));
        }

        [RelayCommand]
        private void ExploreInExplorer()
        {
            messenger.Send(new ExploreSelectedItemMessage(OperationType.FileExplorer));
        }

        [RelayCommand]
        private void ExploreWithHttp()
        {
            messenger.Send(new ExploreSelectedItemMessage(OperationType.Http));
        }

        [RelayCommand]
        private void ExploreWithHttps()
        {
            messenger.Send(new ExploreSelectedItemMessage(OperationType.Https));
        }

        [RelayCommand]
        private void ExploreWithFtp()
        {
            messenger.Send(new ExploreSelectedItemMessage(OperationType.Ftp));
        }

        [RelayCommand]
        private void ExploreWithRdp()
        {
            messenger.Send(new ExploreSelectedItemMessage(OperationType.Rdp));
        }

        [RelayCommand]
        private void WakeOnLan()
        {
            messenger.Send(new ExploreSelectedItemMessage(OperationType.WakeOnLan));
        }

        [RelayCommand]
        private void Cancel()
        {
            Stopping = true;
            messenger.Send(new UpdateScanningStatusMessage(ScanningStatus.Canceled));
        }

        [RelayCommand]
        private void Pause()
        {
            Paused = true;
            messenger.Send(new UpdateScanningStatusMessage(ScanningStatus.Paused));
        }

        [RelayCommand]
        private void Resume()
        {
            Paused = false;
            messenger.Send(new UpdateScanningStatusMessage(ScanningStatus.Running));
        }

        private void FinishScanning()
        {
            Scanning = false;
            Paused = false;
            Stopping = false;
        }

        private void OnMiscellaneousBarVisibilityMessage(object sender, MiscellaneousBarVisibilityMessage message)
        {
            ShowMiscellaneous = message.Visible;
        }

        private void OnActionsBarVisibilityMessage(object sender, ActionsBarVisibilityMessage message)
        {
            ShowActions = message.Visible;
        }

        private void OnDeviceSelectedMessage(object sender, DeviceSelectedMessage message)
        {
            SelectedDevice = message.Device;
        }

        private void OnFinishedScanning(object sender, ScanningFinishedMessage message)
        {
            FinishScanning();
        }

        private void OnValidationErrorMessage(object sender, ValidationErrorMessage message)
        {
            Scanning = false;
        }

        private void OnStartedScanning(object sender, StartScanningMessage<StorageFile> message)
        {
            if(message.Source == ScanningSource.File)
            {
                Scanning = true;
            }
        }

        private void RegisterMessages(IMessenger messanger)
        {
            messanger.Register<MiscellaneousBarVisibilityMessage>(this, OnMiscellaneousBarVisibilityMessage);
            messanger.Register<ActionsBarVisibilityMessage>(this, OnActionsBarVisibilityMessage);
            messanger.Register<DeviceSelectedMessage>(this, OnDeviceSelectedMessage);
            messanger.Register<ScanningFinishedMessage>(this, OnFinishedScanning);
            messanger.Register<StartScanningMessage<StorageFile>>(this, OnStartedScanning);
            messanger.Register<ValidationErrorMessage>(this, OnValidationErrorMessage);
        }
    }
}
