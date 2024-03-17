using System;
using Windows.System;
using System.Threading.Tasks;
using IpScanner.Services.Abstract;

namespace IpScanner.Services
{
    public class UriOpenerService : IUriOpenerService
    {
        public async Task OpenUriAsync(Uri uri)
        {
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
