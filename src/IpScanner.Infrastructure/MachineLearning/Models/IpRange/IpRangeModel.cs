using System;
using Microsoft.AI.MachineLearning;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace IpScanner.Infrastructure.MachineLearning.Models
{
    public class IpRangeModel
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;

        public static async Task<IpRangeModel> CreateFromStreamAsync(IRandomAccessStreamReference stream)
        {
            var learningModel = new IpRangeModel();

            learningModel.model = await LearningModel.LoadFromStreamAsync(stream);
            learningModel.session = new LearningModelSession(learningModel.model);
            learningModel.binding = new LearningModelBinding(learningModel.session);

            return learningModel;
        }

        internal async Task<IpRangeOutput> EvaluateAsync(IpRangeInput input)
        {
            binding.Bind("input", input.Data);
            var result = await session.EvaluateAsync(binding, "0");
            var output = new IpRangeOutput();

            var outputLabel = result.Outputs["output_label"] as TensorInt64Bit;
            if (outputLabel == null)
            {
                throw new Exception($"Unexpected model evaluation result output type");
            }

            output.Result = outputLabel;
            return output;
        }
    }
}
