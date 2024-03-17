using System;
using System.Threading.Tasks;

namespace IpScanner.Services.Abstract
{
    public interface IModalsService
    {
        Task ShowPageAsync(Type pageType);
    }
}
