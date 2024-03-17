using System.Collections.Generic;

namespace IpScanner.Infrastructure.MachineLearning.Models
{
    internal class ClassificationOutput
    {
        public IReadOnlyList<string> PredictedLabels;  // shape: [?]
    }
}
