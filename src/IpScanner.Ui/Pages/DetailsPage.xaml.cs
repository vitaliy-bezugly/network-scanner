using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.ViewModels;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class DetailsPage : Page
    {
        public DetailsPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<DetailsPageViewModel>();
        }

        public DetailsPageViewModel ViewModel { get; }
    }
}
