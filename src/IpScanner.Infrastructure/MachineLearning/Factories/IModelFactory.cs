using System.Threading.Tasks;

namespace IpScanner.Infrastructure.MachineLearning.Factories
{
    public interface IModelFactory<T>
    {
        Task<T> CreateOrGetExistingModelAsync();
    }
}
