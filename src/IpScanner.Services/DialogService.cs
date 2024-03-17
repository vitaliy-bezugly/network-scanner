using System;
using Windows.UI.Popups;
using IpScanner.Services.Abstract;
using System.Threading.Tasks;

namespace IpScanner.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowMessageAsync(string title, string message)
        {
            var dialog = new MessageDialog(message, title);
            await dialog.ShowAsync();
        }
    }
}
