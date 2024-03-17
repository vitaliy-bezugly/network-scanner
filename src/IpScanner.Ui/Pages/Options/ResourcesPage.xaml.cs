using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.ViewModels.Options;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages.Options
{
    public sealed partial class ResourcesPage : Page
    {
        public ResourcesPage()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetRequiredService<ResourcesPageViewModel>();
        }

        public ResourcesPageViewModel ViewModel { get; }
    }
}
