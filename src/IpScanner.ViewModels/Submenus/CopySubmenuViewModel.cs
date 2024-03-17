using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Helpers.Messages;
using IpScanner.Models;
using IpScanner.Services.Abstract;
using System.Windows.Input;

namespace IpScanner.ViewModels.Submenus
{
    public partial class CopySubmenuViewModel : ObservableObject
    {
        private Device selectedDevice;
        private readonly IClipboardService clipboardService;

        public CopySubmenuViewModel(IClipboardService clipboardService, IMessenger messenger)
        {
            this.clipboardService = clipboardService;
            RegisterMessages(messenger);
        }

        [RelayCommand]
        private void CopyAll()
        {
            clipboardService.CopyToClipboard(selectedDevice.ToString());
        }

        [RelayCommand]
        private void CopyName()
        {
            clipboardService.CopyToClipboard(selectedDevice.Name);
        }

        [RelayCommand]
        private void CopyIp()
        {
            clipboardService.CopyToClipboard(selectedDevice.Ip.ToString());
        }

        [RelayCommand]
        private void CopyMac()
        {
            clipboardService.CopyToClipboard(selectedDevice.MacAddress.ToString());
        }

        [RelayCommand]
        private void CopyManufacturer()
        {
            clipboardService.CopyToClipboard(selectedDevice.Manufacturer);
        }

        private void OnSelectedDeviceMessage(object sender, DeviceSelectedMessage message)
        {
            selectedDevice = message.Device;
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<DeviceSelectedMessage>(this, OnSelectedDeviceMessage);
        }
    }
}
