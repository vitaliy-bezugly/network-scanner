using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.ViewModels.Options;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages.Options
{
    public sealed partial class PerformancePage : Page
    {
        public PerformancePage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetRequiredService<PerformancePageViewModel>();
        }

        public PerformancePageViewModel ViewModel { get; }
    }
}
