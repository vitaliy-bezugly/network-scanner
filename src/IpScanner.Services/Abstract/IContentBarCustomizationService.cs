using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.WindowManagement;

namespace IpScanner.Services.Abstract
{
    public interface IContentBarCustomizationService
    {
        void Customize(CoreApplicationViewTitleBar titleBar, UIElement appTitleBar, ColumnDefinition left, ColumnDefinition right);
        void SetCaptionButtonsBackground(AppWindowTitleBar titleBar);
        void SetInactiveWindowColors(AppWindowTitleBar titleBar);
    }
}
