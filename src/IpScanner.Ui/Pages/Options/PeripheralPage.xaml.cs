using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.ViewModels.Options;

namespace IpScanner.Ui.Pages.Options
{
    public sealed partial class PeripheralPage : Page
    {
        public PeripheralPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetRequiredService<PeripheralPageViewModel>();
        }

        public PeripheralPageViewModel ViewModel { get; }
        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.Dispose();
        }
    }
}
