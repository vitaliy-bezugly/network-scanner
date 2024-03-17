using Microsoft.AI.MachineLearning;

namespace IpScanner.Infrastructure.MachineLearning.Models
{
    internal class ClassificationInput
    {
        public TensorInt64Bit FeatureVector;  // shape: [?, 4]
    }
}
