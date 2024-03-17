using System.Linq;
using IpScanner.Models.Enums;
using Microsoft.AI.MachineLearning;
using System.Collections.Generic;
using System.Threading.Tasks;
using IpScanner.Models;
using IpScanner.Helpers.Extensions;
using IpScanner.Infrastructure.Repositories.Abstract;

namespace IpScanner.Infrastructure.MachineLearning.Helpers
{
    internal class ClassificationHelper
    {
        private const int FeatureVectorSize = 4;
        private readonly IFeatureRepository featureRepository;

        public ClassificationHelper(IFeatureRepository featureRepository)
        {
            this.featureRepository = featureRepository;
        }

        public Task<TensorInt64Bit> PrepareFeatureVector(Device device)
        {
            string status = device.Status.ToString();
            string oui = device.MacAddress.GetFormattedOuiOrEmptyString();

            long[] tensorData = CovertFeaturesToNumericList(status, oui, device.Manufacturer, device.SupportHttp);
            return Task.FromResult(TensorInt64Bit.CreateFromArray(new long[] { 1, FeatureVectorSize }, tensorData));
        }

        public Task<DeviceType> GetTypeFromLabels(IEnumerable<string> result)
        {
            string deviceType = result.First();
            return Task.FromResult(deviceType.ConvertStringToDeviceType());
        }

        private long[] CovertFeaturesToNumericList(string status, string oui, string manufacturer, bool supportHttp)
        {
            long statusValue = featureRepository.GetStatusValueOrDefault(status);
            long ouiValue = featureRepository.GetOuiValueOrDefault(oui);
            long manufacturerValue = featureRepository.GetManufacturerValueOrDefault(manufacturer);
            long supportHttpValue = supportHttp ? 1 : 0;
                
            return new long[] { statusValue, ouiValue, manufacturerValue, supportHttpValue };
        }
    }
}
