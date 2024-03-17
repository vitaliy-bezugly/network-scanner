using System.Threading.Tasks;
using Windows.Storage;

namespace IpScanner.Services.Abstract
{
    public interface IFileService
    {
        Task<StorageFile> GetDefaultFileAsync();
        Task<StorageFile> GetFileForReadingAsync(params string[] fileTypes);
        Task<StorageFile> GetFileForWritingAsync(params string[] fileTypes);
        Task WriteToFileAsync(StorageFile file, string content);
    }
}
