using System;
using IpScanner.Infrastructure.MachineLearning.Models;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.Extensions.Configuration;

namespace IpScanner.Infrastructure.MachineLearning.Factories
{
    internal class ClassificationModelFactory : IModelFactory<ClassificationModel>
    {
        private ClassificationModel classificationModel;
        private readonly IConfiguration configuration;

        public ClassificationModelFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<ClassificationModel> CreateOrGetExistingModelAsync()
        {
            if (classificationModel == null)
            {
                classificationModel = await LoadClassificationModelAsync();
            }

            return classificationModel;
        }

        private async Task<ClassificationModel> LoadClassificationModelAsync()
        {
            var uri = new Uri(configuration["MachineLearning:ClassificationModelUri"]);

            StorageFile modelFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            return await ClassificationModel.CreateFromStreamAsync(modelFile as IRandomAccessStreamReference);
        }
    }
}
