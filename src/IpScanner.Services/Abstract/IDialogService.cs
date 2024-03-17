using System.Threading.Tasks;

namespace IpScanner.Services.Abstract
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string title, string message);
    }
}
