using IpScanner.Domain.Contracts;
using IpScanner.Infrastructure.MachineLearning.Factories;
using IpScanner.Infrastructure.MachineLearning.Helpers;
using IpScanner.Infrastructure.MachineLearning.Models;
using IpScanner.Models;
using IpScanner.Models.Enums;
using System.Threading.Tasks;
using Microsoft.AI.MachineLearning;
using IpScanner.Infrastructure.Repositories.Abstract;

namespace IpScanner.Infrastructure.MachineLearning
{
    internal class DeviceMlClassifier : IDeviceClassifier
    {
        private readonly IModelFactory<ClassificationModel> modelFactory;
        private readonly IFeatureRepository featureRepository;

        public DeviceMlClassifier(IModelFactory<ClassificationModel> modelFactory, IFeatureRepository featureRepository)
        {
            this.modelFactory = modelFactory;
            this.featureRepository = featureRepository;
        }

        public async Task<DeviceType> ClassifyAsync(Device device)
        {
            ClassificationModel model = await modelFactory.CreateOrGetExistingModelAsync();
            var helper = new ClassificationHelper(featureRepository);

            // Prepare input string
            TensorInt64Bit tensorData = await helper.PrepareFeatureVector(device);

            // Run the model
            var input = new ClassificationInput() { FeatureVector = tensorData };
            ClassificationOutput output = await model.EvaluateAsync(input);

            // Get the output
            DeviceType result = await helper.GetTypeFromLabels(output.PredictedLabels);
            return result;
        }
    }
}
