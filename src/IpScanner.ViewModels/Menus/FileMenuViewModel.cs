using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Threading.Tasks;
using IpScanner.Models;
using IpScanner.Infrastructure.Repositories;
using System.Collections.Generic;
using IpScanner.Infrastructure.Repositories.Factories;
using System.Windows.Input;
using IpScanner.Services.Abstract;
using Windows.Storage;
using System;
using IpScanner.Helpers.Messages.Scanning;
using IpScanner.Helpers.Messages;
using IpScanner.Helpers.Messages.Printing;
using IpScanner.ViewModels.Bars;
using IpScanner.Helpers.Enums;
using IpScanner.Services;

namespace IpScanner.ViewModels.Menus
{
    public partial class FileMenuViewModel : ObservableObject
    {
        private readonly string[] fileWritingExtensions;
        private readonly string[] fileReadingExtensions;
        private readonly IMessenger messenger;
        private readonly IFileService fileService;
        private readonly IApplicationExitService applicationService;
        private readonly IDeviceRepositoryFactory deviceRepositoryFactory;
        private readonly ScanPageViewModel scanPageViewModel;
        private readonly FavoritesViewModel favoritesViewModel;
        private readonly ActionBarViewModel actionBarViewModel;

        public FileMenuViewModel(IMessenger messenger, 
            IFileService fileService, 
            IApplicationExitService applicationService, 
            IDeviceRepositoryFactory deviceRepositoryFactory,
            ScanPageViewModel scanPageViewModel,
            FavoritesViewModel favoritesViewModel,
            ActionBarViewModel actionBarViewModel)
        {
            this.messenger = messenger;
            this.fileService = fileService;
            this.applicationService = applicationService;
            this.deviceRepositoryFactory = deviceRepositoryFactory;
            this.scanPageViewModel = scanPageViewModel;
            this.favoritesViewModel = favoritesViewModel;
            this.actionBarViewModel = actionBarViewModel;

            fileWritingExtensions = new string[] { ".xml", ".json", ".csv", ".html" };
            fileReadingExtensions = new string[] { ".xml", ".json" };
        }

        public ICommand ScanFromFileCommand => new AsyncRelayCommand(ScanFromFileAsync, CanScanFromFile);

        private async Task ScanFromFileAsync()
        {
            StorageFile file = await fileService.GetFileForReadingAsync(fileReadingExtensions);
            if (file == null)
            {
                return;
            }

            messenger.Send(new StartScanningMessage<StorageFile>(ScanningSource.File, file));
        }

        private bool CanScanFromFile()
        {
            return actionBarViewModel.Scanning == false;
        }

        [RelayCommand]
        private async Task SaveDevicesAsync()
        {
            List<Device> devices = favoritesViewModel.DisplayFavorites 
                ? favoritesViewModel.FavoritesDevices.ToList() 
                : scanPageViewModel.ScannedDevices.ToList();

            StorageFile file = await fileService.GetFileForWritingAsync(fileWritingExtensions);
            if(file == null)
            {
                return;
            }

            IDeviceRepository deviceRepository = deviceRepositoryFactory.CreateWithFile(file);
            await deviceRepository.SaveDevicesAsync(devices);
        }

        [RelayCommand]
        private async Task LoadFavoritesAsync()
        {
            StorageFile file = await fileService.GetFileForReadingAsync(fileReadingExtensions);
            if(file == null)
            {
                return;
            }

            messenger.Send(new DevicesLoadedMessage<StorageFile>(file));
        }

        [RelayCommand]
        private void PrintPreview()
        {
            messenger.Send(new ShowPrintPreviewMessage());
        }

        [RelayCommand]
        private void Exit()
        {
            applicationService.Exit();
        }
    }
}
