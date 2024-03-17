using System.Threading.Tasks;
using IpScanner.Models;
using IpScanner.Domain.Validators;
using IpScanner.Infrastructure.MachineLearning.Helpers;
using IpScanner.Infrastructure.MachineLearning.Models;
using IpScanner.Helpers;
using IpScanner.Infrastructure.MachineLearning.Factories;
using Microsoft.AI.MachineLearning;

namespace IpScanner.Infrastructure.MachineLearning
{
    internal class IpRangeMlValidator : IValidator<IpRange>
    {
        private readonly IModelFactory<IpRangeModel> modelFactory;
        private readonly IValidator<IpRange> validator;
        private readonly AppSettings settings;

        public IpRangeMlValidator(IModelFactory<IpRangeModel> modelFactory, 
            IValidator<IpRange> validator, 
            ISettingsService settingsService)
        {
            this.modelFactory = modelFactory;
            this.validator = validator;
            this.settings = settingsService.Settings;
        }

        public async Task<bool> ValidateAsync(IpRange value)
        {
            if(settings.EnableValidationMachineLearning)
            {
                return await ValidateViaMlAsync(value);
            }

            return await validator.ValidateAsync(value);
        }

        private async Task<bool> ValidateViaMlAsync(IpRange value)
        {
            IpRangeModel ipRangeModel = await modelFactory.CreateOrGetExistingModelAsync();
            var ipRangeHelper = new IpRangeHelper();

            // Prepare input string
            string stringToCheck = value.Range;
            TensorInt64Bit tensorData = await ipRangeHelper.GetStringTensor(stringToCheck);

            // Run the model
            var input = new IpRangeInput() { Data = tensorData };
            IpRangeOutput output = await ipRangeModel.EvaluateAsync(input);

            // Validate the output
            bool isValid = await ipRangeHelper.IsResultValid(output.Result);
            return isValid;
        }
    }
}
