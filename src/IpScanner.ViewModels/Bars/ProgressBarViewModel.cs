using System;
using IpScanner.Models.Enums;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Services.Abstract;
using IpScanner.Helpers.Messages.Progress;
using IpScanner.Helpers.Messages.Scanning;
using IpScanner.Helpers.Constants;

namespace IpScanner.ViewModels.Bars
{
    public partial class ProgressBarViewModel : ObservableObject
    {
        [ObservableProperty]
        private int totalCountOfIps;
        [ObservableProperty]
        private int countOfOnlineDevices;
        [ObservableProperty]
        private int countOfScannedIps;
        [ObservableProperty]
        private int countOfUnknownDevices;
        [ObservableProperty]
        private int countOfOfflineDevices;
        private bool scanningFinished;
        private readonly ILocalizationService localizationService;

        public ProgressBarViewModel(ILocalizationService localizationService, IMessenger messenger)
        {
            this.localizationService = localizationService;
            scanningFinished = false;

            CountOfScannedIps = 0;
            CountOfUnknownDevices = 0;
            CountOfOnlineDevices = 0;
            CountOfOfflineDevices = 0;
            TotalCountOfIps = int.MaxValue;

            RegisterMessages(messenger);
        }

        public string ProgressString
        {
            get
            {
                string dead = localizationService.GetString(LocalizationKeys.Dead);
                string unknown = localizationService.GetString(LocalizationKeys.Unknown);
                double progress = CalculateProgress();

                if (progress == 0 || scanningFinished)
                {
                    string online = localizationService.GetString(LocalizationKeys.Online);
                    return $"{CountOfOnlineDevices} {online}, {CountOfOfflineDevices} {dead}, {CountOfUnknownDevices} {unknown}";
                }

                return $"{CalculateProgress()}%, {CountOfOfflineDevices} {dead}, {CountOfUnknownDevices} {unknown}";
            }
        }

        partial void OnCountOfScannedIpsChanged(int value)
        {
            OnPropertyChanged(nameof(ProgressString));
        }

        partial void OnCountOfUnknownDevicesChanged(int value)
        {
            OnPropertyChanged(nameof(ProgressString));
        }

        partial void OnCountOfOnlineDevicesChanged(int value)
        {
            OnPropertyChanged(nameof(ProgressString));
        }

        public void UpdateProgress(int currentCount, DeviceStatus status)
        {
            CountOfScannedIps = currentCount;
            IncreaseCountOfSpecificDevices(status);
        }

        public void IncreaseProgress(DeviceStatus status)
        {
            CountOfScannedIps += 1;
            IncreaseCountOfSpecificDevices(status);
        }

        public void ResetProgress()
        {
            CountOfScannedIps = 0;
            CountOfUnknownDevices = 0;
            CountOfOnlineDevices = 0;
            CountOfOfflineDevices = 0;
            TotalCountOfIps = int.MaxValue;
            scanningFinished = false;
        }

        public void SetTotalCountOfIps(int count)
        {
            TotalCountOfIps = count;
        }

        private double CalculateProgress()
        {
            if (TotalCountOfIps == 0)
            {
                return 0;
            }

            return Math.Ceiling(((double)CountOfScannedIps / TotalCountOfIps) * 100);
        }

        private void IncreaseCountOfSpecificDevices(DeviceStatus status)
        {
            switch (status)
            {
                case DeviceStatus.Unknown:
                    CountOfUnknownDevices++;
                    break;
                case DeviceStatus.Online:
                    CountOfOnlineDevices++;
                    break;
                case DeviceStatus.Offline:
                    CountOfOfflineDevices++;
                    break;
            }
        }

        private void OnResetProgressMessage(object sender, ResetProgressMessage message)
        {
            ResetProgress();
        }

        private void OnDeviceScannedMessage(object sender, DeviceScannedMessage message)
        {
            IncreaseProgress(message.Device.Status);
        }

        private void OnSetTotalCountOfScannedIpsMessage(object sender, SetTotalCountOfScannedIpsMessage message)
        {
            SetTotalCountOfIps(message.TotalCount);
        }

        private void OnScanningFinishedMessage(object sender, ScanningFinishedMessage message)
        {
            scanningFinished = true;
            CountOfScannedIps = TotalCountOfIps;
            CountOfUnknownDevices = TotalCountOfIps - CountOfOnlineDevices - CountOfOfflineDevices;
            OnPropertyChanged(nameof(ProgressString));
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<ResetProgressMessage>(this, OnResetProgressMessage);
            messenger.Register<DeviceScannedMessage>(this, OnDeviceScannedMessage);
            messenger.Register<SetTotalCountOfScannedIpsMessage>(this, OnSetTotalCountOfScannedIpsMessage);
            messenger.Register<ScanningFinishedMessage>(this, OnScanningFinishedMessage);
        }
    }
}
