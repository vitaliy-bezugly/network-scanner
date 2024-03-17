using System.Threading.Tasks;

namespace IpScanner.Domain.Validators
{
    public interface IValidator<T>
    {
        Task<bool> ValidateAsync(T value);
    }
}
