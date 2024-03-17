using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.ViewModels.Bars;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class ProgressBar : Page
    {
        public ProgressBar()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<ProgressBarViewModel>();
        }

        public ProgressBarViewModel ViewModel { get; }
    }
}
