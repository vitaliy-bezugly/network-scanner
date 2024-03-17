using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace IpScanner.Infrastructure.Repositories
{
    internal class HistoryRepository<T> : IHistoryRepository<T>
    {
        private readonly string fileName;

        public HistoryRepository(IConfiguration configuration)
        {
            fileName = configuration["History:FileName"];
        }

        public async Task AddItemAsync(T item)
        {
            List<T> items = await ReadFromFileAsync();
            items.Add(item);

            await WriteToFileAsync(items);
        }

        public async Task ClearAsync()
        {
            await WriteToFileAsync(new List<T>());
        }

        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            return await ReadFromFileAsync();
        }

        public async Task RemoveItemAsync(T item)
        {
            List<T> items = await ReadFromFileAsync();
            items.Remove(item);

            await WriteToFileAsync(items);
        }

        private async Task<List<T>> ReadFromFileAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;

            try
            {
                var file = await localFolder.GetFileAsync(fileName);
                var json = await FileIO.ReadTextAsync(file);

                return JsonConvert.DeserializeObject<List<T>>(json);
            }
            catch (FileNotFoundException)
            {
                return new List<T>();
            }
        }

        private async Task WriteToFileAsync(List<T> items)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            var json = JsonConvert.SerializeObject(items);

            await FileIO.WriteTextAsync(file, json);
        }
    }
}
