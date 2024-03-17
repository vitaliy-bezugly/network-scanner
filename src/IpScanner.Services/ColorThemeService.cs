using Windows.UI.Xaml;
using IpScanner.Services.Abstract;
using IpScanner.Services.Containers;

namespace IpScanner.Services
{
    public class ColorThemeService : IColorThemeService
    {
        private readonly IChildElementsContainer childElements;

        public ColorThemeService(IChildElementsContainer childElements)
        {
            this.childElements = childElements;
        }

        public ElementTheme GetColorTheme()
        {
            return childElements.MainFrame.RequestedTheme;
        }

        public void SetColorTheme(ElementTheme theme)
        {
            childElements.MainFrame.RequestedTheme = theme;

            foreach (var element in childElements)
            {
                element.RequestedTheme = theme;
            }
        }

        public void SetColorTheme(FrameworkElement element, ElementTheme theme)
        {
            element.RequestedTheme = theme;
        }
    }
}
