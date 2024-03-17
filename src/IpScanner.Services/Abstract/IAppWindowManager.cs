using System;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;

namespace IpScanner.Services.Abstract
{
    public interface IAppWindowManager
    {
        Task<AppWindow> CreateAppWindowAsync(Type pageType);
        Task CloseAllAppWindowsAsync();
        bool Contains(Type pageType);
    }
}
