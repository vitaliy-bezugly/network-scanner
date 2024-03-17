using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.ViewModels.Bars;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class InputBar : Page
    {
        public InputBar()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetRequiredService<InputBarViewModel>();   
        }

        public InputBarViewModel ViewModel { get; }
    }
}
