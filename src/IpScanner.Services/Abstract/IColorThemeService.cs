using Windows.UI.Xaml;

namespace IpScanner.Services.Abstract
{
    public interface IColorThemeService
    {
        ElementTheme GetColorTheme();
        void SetColorTheme(ElementTheme theme);
        void SetColorTheme(FrameworkElement element, ElementTheme theme);
    }
}
