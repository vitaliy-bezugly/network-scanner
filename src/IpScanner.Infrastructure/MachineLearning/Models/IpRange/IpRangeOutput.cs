using Microsoft.AI.MachineLearning;

namespace IpScanner.Infrastructure.MachineLearning.Models
{
    internal class IpRangeOutput
    {
        public TensorInt64Bit Result;  // shape: [1, 1]
    }
}
