using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.WindowManagement;
using IpScanner.Services.Abstract;

namespace IpScanner.Services
{
    public class ContentBarCustomizationService : IContentBarCustomizationService
    {
        private UIElement appTitleBar;
        private CoreApplicationViewTitleBar coreTitleBar;
        private ColumnDefinition leftPaddingColumn;
        private ColumnDefinition rightPaddingColumn;

        public void Customize(CoreApplicationViewTitleBar coreTitleBar, UIElement appTitleBar, ColumnDefinition left, ColumnDefinition right)
        {
            InitializeElements(coreTitleBar, appTitleBar, left, right);
            this.coreTitleBar.ExtendViewIntoTitleBar = true;

            SetCaptionButtonsBackground();
            SetInactiveWindowColors();

            Window.Current.SetTitleBar(this.appTitleBar);

            this.coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            this.coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
        }

        public void SetCaptionButtonsBackground(AppWindowTitleBar titleBar)
        {
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Colors.LightGray;
            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ButtonHoverForegroundColor = Colors.Black;
        }

        public void SetInactiveWindowColors(AppWindowTitleBar titleBar)
        {
            titleBar.InactiveForegroundColor = Colors.Black;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.Black;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        private void InitializeElements(CoreApplicationViewTitleBar coreTitleBar, UIElement appTitleBar, ColumnDefinition left, ColumnDefinition right)
        {
            this.coreTitleBar = coreTitleBar;
            this.appTitleBar = appTitleBar;
            leftPaddingColumn = left;
            rightPaddingColumn = right;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            leftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            rightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                appTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                appTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        private void SetCaptionButtonsBackground()
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Colors.LightGray;
            titleBar.ButtonForegroundColor = Colors.Black;
            titleBar.ButtonHoverForegroundColor = Colors.Black;
        }

        private void SetInactiveWindowColors()
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.InactiveForegroundColor = Colors.Black;
            titleBar.InactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.Black;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }
    }
}
