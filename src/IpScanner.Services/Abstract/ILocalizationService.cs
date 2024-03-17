using System.Threading.Tasks;
using Windows.Globalization;

namespace IpScanner.Services.Abstract
{
    public interface ILocalizationService
    {
        Task SetLanguageAsync(Language language);
        string GetString(string key);
    }
}