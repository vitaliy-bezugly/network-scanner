using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IpScanner.Infrastructure.Repositories;
using IpScanner.Infrastructure.Repositories.Factories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using IpScanner.Models;
using IpScanner.Helpers.Filters;
using IpScanner.Helpers;
using IpScanner.Services.Abstract;
using IpScanner.Helpers.Messages.Ui.Visibility;
using IpScanner.Helpers.Messages;
using IpScanner.Helpers.Messages.Filters;
using IpScanner.Helpers.Extensions;
using Newtonsoft.Json.Linq;
using IpScanner.Helpers.Constants;

namespace IpScanner.ViewModels
{
    public partial class FavoritesViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool displayFavorites;
        [ObservableProperty]
        private string selectedCollection;
        [ObservableProperty]
        private FilteredCollection<Device> favoritesDevices;
        private Device selectedDevice;
        private StorageFile storageFile;
        private StorageFile previousFile;
        private readonly AppSettings settings;
        private readonly IMessenger messenger;
        private readonly IFileService fileService;
        private readonly IDialogService dialogService;
        private readonly IDeviceRepositoryFactory deviceRepositoryFactory;
        private readonly ILocalizationService localizationService;

        public FavoritesViewModel(IMessenger messenger, 
            IFileService fileService, 
            IDeviceRepositoryFactory deviceRepositoryFactory, 
            IDialogService dialogService, 
            ILocalizationService localizationService, 
            ISettingsService settingsService)
        {
            settings = settingsService.Settings;
            this.fileService = fileService;
            this.deviceRepositoryFactory = deviceRepositoryFactory;
            this.dialogService = dialogService;
            this.localizationService = localizationService;
            this.messenger = messenger;

            DisplayFavorites = settings.FavoritesSelected;
            favoritesDevices = new FilteredCollection<Device>();
            SelectedCollection = settings.FavoritesSelected ? "Favorites" : "Results";

            RegisterMessages(messenger);
        }

        partial void OnDisplayFavoritesChanged(bool value)
        {
            settings.FavoritesSelected = value;
        }

        partial void OnSelectedCollectionChanged(string value)
        {
            ExecuteSelectedOptionCommand.Execute(value);
        }

        [RelayCommand]
        private async Task ExecuteSelectedOptionAsync(string collection)
        {
            if (collection == "Results")
            {
                UnloadFavorites();
            }
            else if (collection == "Favorites")
            {
                await LoadFavoritesAsync();
            }
            else
            {
                throw new System.InvalidOperationException();
            }
        }

        [RelayCommand]
        private async Task LoadFavoritesAsync()
        {
            IEnumerable<Device> devices = await TryLoadDevicesAsync();

            if (devices == null)
            {
                await HandleLoadingFailureAsync();
                return;
            }

            UpdateFavoritesDevices(devices);
            messenger.Send(new FavoritesItemsVisibilityMessage(true));
            DisplayFavorites = true;
        }

        [RelayCommand]
        private void UnloadFavorites()
        {
            DisplayFavorites = false;
            messenger.Send(new FavoritesItemsVisibilityMessage(false));
        }

        [RelayCommand]
        private async Task AddToFavoritesAsync()
        {
            if (selectedDevice == null)
            {
                return;
            }

            StorageFile file = await GetStorageFileAsync();
            IDeviceRepository deviceRepository = deviceRepositoryFactory.CreateWithFile(file);

            selectedDevice.MarkAsFavorite();
            await deviceRepository.AddDeviceAsync(selectedDevice);
        }

        [RelayCommand]
        private async Task RemoveFromFavoritesAsync()
        {
            if (selectedDevice == null)
            {
                return;
            }

            StorageFile file = await GetStorageFileAsync();
            IDeviceRepository deviceRepository = deviceRepositoryFactory.CreateWithFile(file);

            selectedDevice.UnmarkAsFavorite();
            await deviceRepository.RemoveDeviceAsync(selectedDevice);
            FavoritesDevices.Remove(selectedDevice);
        }

        [RelayCommand]
        private async Task SaveFavoritesAsync()
        {
            StorageFile file = await GetStorageFileAsync();
            IDeviceRepository deviceRepository = deviceRepositoryFactory.CreateWithFile(file);
            await deviceRepository.SaveDevicesAsync(FavoritesDevices);
        }

        private async Task<IEnumerable<Device>> TryLoadDevicesAsync()
        {
            StorageFile file = await GetStorageFileAsync();
            IDeviceRepository deviceRepository = deviceRepositoryFactory.CreateWithFile(file);
            return await deviceRepository.GetDevicesOrNullAsync();
        }

        private async Task HandleLoadingFailureAsync()
        {
            RollbackStorageFile();
            await ShowLoadingDataErrorMessageAsync();

            if (!DisplayFavorites)
            {
                SelectedCollection = "Results";
            }
        }

        private void UpdateFavoritesDevices(IEnumerable<Device> devices)
        {
            FavoritesDevices.Clear();
            foreach (var device in devices)
            {
                FavoritesDevices.Add(device);
            }
        }

        private void RollbackStorageFile()
        {
            storageFile = previousFile;
        }

        private async Task ShowLoadingDataErrorMessageAsync()
        {
            string title = localizationService.GetString(LocalizationKeys.Error);
            string message = localizationService.GetString(LocalizationKeys.FileCorrupted);
            await dialogService.ShowMessageAsync(title, message);
        }

        private async Task<StorageFile> GetStorageFileAsync()
        {
            if (storageFile == null)
            {
                storageFile = await fileService.GetDefaultFileAsync();
            }

            return storageFile;
        }

        private void OnDeviceSelected(object sender, DeviceSelectedMessage message)
        {
            selectedDevice = message.Device;
        }

        private void OnDevicesLoaded(object sender, DevicesLoadedMessage<StorageFile> message)
        {
            previousFile = storageFile;
            storageFile = message.NewFile;

            if(DisplayFavorites)
            {
                ExecuteSelectedOptionCommand.Execute("Favorites");
            }
            else
            {
                SelectedCollection = "Favorites";
            }
        }

        private void OnFilterMessage(object sender, ApplyFilterMessage<Device> message)
        {
            FavoritesDevices.ApplyFilter(message.FilterStatus, message.Filter);
        }

        private void RegisterMessages(IMessenger messenger)
        {
            messenger.Register<DeviceSelectedMessage>(this, OnDeviceSelected);
            messenger.Register<DevicesLoadedMessage<StorageFile>>(this, OnDevicesLoaded);
            messenger.Register<ApplyFilterMessage<Device>>(this, OnFilterMessage);
        }
    }
}
