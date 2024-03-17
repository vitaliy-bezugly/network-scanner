using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Helpers.Markers;
using IpScanner.Services.Containers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace IpScanner.Ui.Pages.Options
{
    public sealed partial class OptionsRootPage : Page, IOptionsPageMarker
    {
        public OptionsRootPage()
        {
            this.InitializeComponent();

            var optionsPage = Ioc.Default.GetService<OptionsPage>();
            OptionsFrame.Navigate(optionsPage.GetType());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var frameContainer = Ioc.Default.GetService<IFramesContainer>();
            frameContainer.Remove(OptionsFrame);
            frameContainer.Add(OptionsFrame);
        }
    }
}
