using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.Ui.Pages;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Controls;
using IpScanner.Services.Abstract;
using IpScanner.ViewModels;

namespace IpScanner.Ui
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeScanPageViewModelForMessanger();
            this.InitializeComponent();
            this.CustomizeToolbar();

            NavigateToScanPage(); 
        }

        private static void InitializeScanPageViewModelForMessanger() => Ioc.Default.GetService<ScanPageViewModel>();

        private void CustomizeToolbar()
        {
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            var contentBarService = Ioc.Default.GetService<IContentBarCustomizationService>();
            contentBarService.Customize(coreTitleBar, AppTitleBar, LeftPaddingColumn, RightPaddingColumn);
        }

        private void NavigateToScanPage()
        {
            INavigationService navigationService = Ioc.Default.GetService<INavigationService>();
            navigationService.NavigateToPage(ContentFrame, typeof(ScanPage));
        }
    }
}
