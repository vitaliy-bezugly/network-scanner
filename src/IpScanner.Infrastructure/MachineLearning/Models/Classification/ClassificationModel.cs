using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Microsoft.AI.MachineLearning;

namespace IpScanner.Infrastructure.MachineLearning.Models
{
    internal class ClassificationModel
    {
        private LearningModel model;
        private LearningModelSession session;
        private LearningModelBinding binding;

        public static async Task<ClassificationModel> CreateFromStreamAsync(IRandomAccessStreamReference stream)
        {
            var learningModel = new ClassificationModel();

            learningModel.model = await LearningModel.LoadFromStreamAsync(stream);
            learningModel.session = new LearningModelSession(learningModel.model);
            learningModel.binding = new LearningModelBinding(learningModel.session);

            return learningModel;
        }

        internal async Task<ClassificationOutput> EvaluateAsync(ClassificationInput input)
        {
            binding.Bind("input", input.FeatureVector);
            var result = await session.EvaluateAsync(binding, "0");
            var output = new ClassificationOutput();

            var outputLabel = result.Outputs["output_label"] as TensorString;
            if (outputLabel == null)
            {
                throw new Exception("Unexpected model evaluation result output type");
            }

            output.PredictedLabels = outputLabel.GetAsVectorView();
            return output;
        }
    }
}
