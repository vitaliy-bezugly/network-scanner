using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using IpScanner.Infrastructure.Repositories.Factories;
using IpScanner.Models;
using IpScanner.Helpers.Messages.Ui;
using IpScanner.Helpers.Enums;
using IpScanner.Helpers.Messages;
using IpScanner.Services.Abstract;

namespace IpScanner.ViewModels.Submenus
{
    public partial class ContextSubmenuViewModel : ObservableObject
    {
        private Device selectedDevice;
        private readonly IFileService fileService;
        private readonly IMessenger messenger;
        private readonly IDeviceRepositoryFactory deviceRepositoryFactory;
        private readonly FavoritesViewModel favoritesViewModel;
        private readonly ToolsSubmenuViewModel toolsSubmenuViewModel;
        private readonly CopySubmenuViewModel copySubmenuViewModel;

        public ContextSubmenuViewModel(IFileService fileService, 
            IDeviceRepositoryFactory deviceRepositoryFactory,
            IMessenger messenger,
            FavoritesViewModel favoritesViewModel,
            ToolsSubmenuViewModel toolsSubmenuViewModel,
            CopySubmenuViewModel copySubmenuViewModel)
        {
            this.fileService = fileService;
            this.deviceRepositoryFactory = deviceRepositoryFactory;
            this.messenger = messenger;
            this.favoritesViewModel = favoritesViewModel;
            this.toolsSubmenuViewModel = toolsSubmenuViewModel;
            this.copySubmenuViewModel = copySubmenuViewModel;
            RegisterMessages(messenger);
        }

        public ToolsSubmenuViewModel ToolsViewModel => toolsSubmenuViewModel;

        public CopySubmenuViewModel CopyViewModel => copySubmenuViewModel;

        [RelayCommand]
        private async Task SaveDeviceAsync()
        {
            StorageFile file = await fileService.GetFileForWritingAsync(".json", ".xml", ".csv", ".html");
            if (file == null || selectedDevice == null)
            {
                return;
            }

            IDeviceRepository repository = deviceRepositoryFactory.CreateWithFile(file);
            await repository.SaveDevicesAsync(new List<Device> { selectedDevice });
        }

        [RelayCommand]
        private void EditName()
        {
            var message = new SetFocusToCellMessage(DeviceRow.Hostname, favoritesViewModel.DisplayFavorites);
            messenger.Send(message);
        }

        [RelayCommand]
        private void EditComments()
        {
            var message = new SetFocusToCellMessage(DeviceRow.Comments, favoritesViewModel.DisplayFavorites);
            messenger.Send(message);
        }

        [RelayCommand]
        private void EditMac()
        {
            var message = new SetFocusToCellMessage(DeviceRow.Mac, favoritesViewModel.DisplayFavorites);
            messenger.Send(message);
        }

        private void OnDeviceSelected(object sender, DeviceSelectedMessage message)
        {
            selectedDevice = message.Device;
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<DeviceSelectedMessage>(this, OnDeviceSelected);
        }
    }
}
