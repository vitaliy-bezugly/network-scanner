using System.Collections.Generic;
using Windows.UI.Xaml;

namespace IpScanner.Services.Containers
{
    public interface IChildElementsContainer : IEnumerable<FrameworkElement>
    {
        FrameworkElement MainFrame { get; }
        void AddChildElement(FrameworkElement element);
        void ClearChildElements();
    }
}
