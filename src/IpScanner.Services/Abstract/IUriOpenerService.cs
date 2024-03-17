using System;
using System.Threading.Tasks;

namespace IpScanner.Services.Abstract
{
    public interface IUriOpenerService
    {
        Task OpenUriAsync(Uri uri);
    }
}
