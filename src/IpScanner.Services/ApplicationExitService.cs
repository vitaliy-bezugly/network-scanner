using Windows.UI.Xaml;
using IpScanner.Services.Abstract;

namespace IpScanner.Services
{
    public class ApplicationExitService : IApplicationExitService
    {
        public void Exit()
        {
            Application.Current.Exit();
        }
    }
}
