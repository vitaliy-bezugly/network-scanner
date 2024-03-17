using System;
using IpScanner.Services.Abstract;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace IpScanner.Services
{
    public class FileService : IFileService
    {
        private readonly string fileName;

        public FileService(IConfiguration configuration)
        {
            fileName = configuration["Devices:DefaultFileName"];
        }

        public async Task<StorageFile> GetDefaultFileAsync()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            return await localFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
        }

        public async Task<StorageFile> GetFileForReadingAsync(params string[] fileTypes)
        {
            return await GetFileFromOpenPickerAsync(fileTypes);
        }

        public async Task<StorageFile> GetFileForWritingAsync(params string[] fileTypes)
        {
            return await GetFileFromSavePickerAsync(fileTypes);
        }

        public async Task WriteToFileAsync(StorageFile file, string content)
        {
            await FileIO.WriteTextAsync(file, content);
        }

        private async Task<StorageFile> GetFileFromOpenPickerAsync(params string[] fileTypes)
        {
            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            foreach (var item in fileTypes)
            {
                openPicker.FileTypeFilter.Add(item);
            }

            return await openPicker.PickSingleFileAsync();
        }

        private async Task<StorageFile> GetFileFromSavePickerAsync(params string[] fileTypes)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            foreach (var item in fileTypes)
            {
                savePicker.FileTypeChoices.Add(item, new List<string>() { item });
            }

            savePicker.SuggestedFileName = "items";
            return await savePicker.PickSaveFileAsync();
        }
    }
}
