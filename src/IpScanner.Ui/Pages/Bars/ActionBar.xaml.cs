using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.ViewModels.Bars;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class ActionBar : Page
    {
        public ActionBar()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetRequiredService<ActionBarViewModel>();
        }

        public ActionBarViewModel ViewModel { get; }
    }
}
