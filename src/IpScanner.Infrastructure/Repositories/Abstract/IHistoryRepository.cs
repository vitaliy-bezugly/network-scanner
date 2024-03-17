using System.Collections.Generic;
using System.Threading.Tasks;

namespace IpScanner.Infrastructure.Repositories
{
    public interface IHistoryRepository<T>
    {
        Task AddItemAsync(T item);
        Task ClearAsync();
        Task<IEnumerable<T>> GetItemsAsync();
        Task RemoveItemAsync(T item);
    }
}
