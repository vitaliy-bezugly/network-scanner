using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Storage;
using IpScanner.Infrastructure.MachineLearning.Models;
using Microsoft.Extensions.Configuration;

namespace IpScanner.Infrastructure.MachineLearning.Factories
{
    internal class IpRangeModelFactory : IModelFactory<IpRangeModel>
    {
        private IpRangeModel ipRangeModel;
        private readonly IConfiguration configuration;

        public IpRangeModelFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IpRangeModel> CreateOrGetExistingModelAsync()
        {
            if (ipRangeModel == null)
            {
                ipRangeModel = await LoadIpRangeModelAsync();
            }

            return ipRangeModel;
        }

        private async Task<IpRangeModel> LoadIpRangeModelAsync()
        {
            var uri = new Uri(configuration["MachineLearning:ValidationModelUri"]);

            StorageFile modelFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            return await IpRangeModel.CreateFromStreamAsync(modelFile as IRandomAccessStreamReference);
        }
    }
}
