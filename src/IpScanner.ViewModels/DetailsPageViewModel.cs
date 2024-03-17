using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Infrastructure.Repositories;
using IpScanner.Infrastructure.Repositories.Factories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using IpScanner.Models;
using IpScanner.Helpers;
using IpScanner.Services.Abstract;
using IpScanner.Helpers.Messages;
using IpScanner.Helpers.Messages.Ui.Visibility;

namespace IpScanner.ViewModels
{
    public partial class DetailsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private Device device;
        [ObservableProperty]
        private bool displayDate;
        private readonly AppSettings settings;
        private readonly IFileService fileService;
        private readonly IPrintService<Device> printService;
        private readonly IDeviceRepositoryFactory deviceRepositoryFactory;
        private readonly IUriOpenerService uriOpenerService;

        public DetailsPageViewModel(IMessenger messenger,
            IPrintService<Device> printService,
            IFileService fileService,
            IDeviceRepositoryFactory deviceRepositoryFactory,
            ISettingsService settingsService,
            IUriOpenerService uriOpenerService)
        {
            this.printService = printService;
            this.fileService = fileService;
            this.deviceRepositoryFactory = deviceRepositoryFactory;
            this.uriOpenerService = uriOpenerService;
            settings = settingsService.Settings;
            DisplayDate = settings.ScanDateTime;
            device = new Device(IPAddress.Any);

            RegisterMessages(messenger);
        }

        [RelayCommand]
        private async Task ShowPrintPreviewAsync()
        {
            await printService.ShowPrintUIAsync(new List<Device> { Device });
        }

        [RelayCommand]
        private async Task SaveDeviceAsync()
        {
            StorageFile file = await fileService.GetFileForWritingAsync(".json", ".xml", ".csv", ".html");
            if (file == null || Device == null)
            {
                return;
            }

            IDeviceRepository repository = deviceRepositoryFactory.CreateWithFile(file);
            await repository.SaveDevicesAsync(new List<Device> { Device });
        }

        [RelayCommand]
        private async Task OpenUriAsync()
        {
            if (Device == null || Device.Service == null)
            {
                return;
            }

            await uriOpenerService.OpenUriAsync(Device.Service.Uri);
        }

        private void OnSelectedDeviceChanged(object sender, DeviceSelectedMessage message)
        {
            Device = message.Device;
        }

        private void OnChangeDatetimeMessage(object sender, DatetimeVisibilityMessage message)
        {
            DisplayDate = message.Visible;
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<DeviceSelectedMessage>(this, OnSelectedDeviceChanged);
            messenger.Register<DatetimeVisibilityMessage>(this, OnChangeDatetimeMessage);
        }
    }
}
