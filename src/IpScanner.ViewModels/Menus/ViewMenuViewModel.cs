using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Models.Enums;
using IpScanner.Models;
using IpScanner.Helpers.Filters;
using IpScanner.Helpers;
using IpScanner.Services.Abstract;
using IpScanner.Helpers.Messages.Filters;
using IpScanner.Helpers.Messages.Ui.Visibility;

namespace IpScanner.ViewModels.Menus
{
    public partial class ViewMenuViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool showUnknown;
        [ObservableProperty]
        private bool showOnline;
        [ObservableProperty]
        private bool showOffline;
        [ObservableProperty]
        private bool showDetails;
        [ObservableProperty]
        private bool showActions;
        [ObservableProperty]
        private bool showMiscellaneous;
        private readonly IMessenger messenger;
        private readonly AppSettings settings;
        private readonly ItemFilter<Device> unknownFilter = new ItemFilter<Device>(device => device.Status != DeviceStatus.Unknown);
        private readonly ItemFilter<Device> onlineFilter = new ItemFilter<Device>(device => device.Status != DeviceStatus.Online);
        private readonly ItemFilter<Device> offlineFilter = new ItemFilter<Device>(device => device.Status != DeviceStatus.Offline);

        public ViewMenuViewModel(IMessenger messenger, ISettingsService settingsService)
        {
            this.messenger = messenger;
            settings = settingsService.Settings;
            ShowUnknown = settings.ShowUnknown;
            ShowOnline = settings.ShowOnline;
            ShowOffline = settings.ShowOffline;
            ShowDetails = settings.ShowDetails;
            ShowMiscellaneous = settings.ShowMiscellaneousToolbar;
            ShowActions = settings.ShowActionsToolbar;
            ApplyFilters();
        }

        partial void OnShowUnknownChanged(bool oldValue, bool newValue)
        {
            messenger.Send(new ApplyFilterMessage<Device>(unknownFilter, !newValue));
            settings.ShowUnknown = newValue;
        }

        partial void OnShowOnlineChanged(bool value)
        {
            messenger.Send(new ApplyFilterMessage<Device>(onlineFilter, !value));
            settings.ShowOnline = value;
        }

        partial void OnShowOfflineChanged(bool value)
        {
            messenger.Send(new ApplyFilterMessage<Device>(offlineFilter, !value));
            settings.ShowOffline = value;
        }

        partial void OnShowDetailsChanged(bool value)
        {
            messenger.Send(new DetailsPageVisibilityMessage(value));
            settings.ShowDetails = value;
        }

        partial void OnShowMiscellaneousChanged(bool value)
        {
            messenger.Send(new MiscellaneousBarVisibilityMessage(value));
            settings.ShowMiscellaneousToolbar = value;
        }

        partial void OnShowActionsChanged(bool value)
        {
            messenger.Send(new ActionsBarVisibilityMessage(value));
            settings.ShowActionsToolbar = value;
        }

        [RelayCommand]
        private void InvertOnlineVisibility()
        {
            ShowOnline = !ShowOnline;
        }

        [RelayCommand]
        private void InvertOfflineVisibility()
        {
            ShowOffline = !ShowOffline;
        }

        [RelayCommand]
        private void InvertUnknownVisibility()
        {
            ShowUnknown = !ShowUnknown;
        }

        [RelayCommand]
        private void InvertDetailsPageVisibility()
        {
            ShowDetails = !ShowDetails;
        }

        [RelayCommand]
        private void InvertMiscellaneousVisibility()
        {
            ShowMiscellaneous = !ShowMiscellaneous;
        }

        [RelayCommand]
        private void InvertActionsVisibility()
        {
            ShowActions = !ShowActions;
        }

        private void ApplyFilters()
        {
            messenger.Send(new ApplyFilterMessage<Device>(unknownFilter, !ShowUnknown));
            messenger.Send(new ApplyFilterMessage<Device>(onlineFilter, !ShowOnline));
            messenger.Send(new ApplyFilterMessage<Device>(offlineFilter, !ShowOffline));
        }
    }
}
