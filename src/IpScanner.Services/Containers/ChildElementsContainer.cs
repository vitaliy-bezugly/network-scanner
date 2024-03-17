using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Services.Containers
{
    internal class ChildElementsContainer : IChildElementsContainer
    {
        private readonly FrameworkElement mainFrame;
        private readonly List<FrameworkElement> childElements;

        public ChildElementsContainer(Frame mainFrame)
        {
            this.mainFrame = mainFrame;
            childElements = new List<FrameworkElement>();
        }

        public FrameworkElement MainFrame => mainFrame;

        public void AddChildElement(FrameworkElement element)
        {
            childElements.Add(element);
        }

        public void ClearChildElements()
        {
            childElements.Clear();
        }

        public IEnumerator<FrameworkElement> GetEnumerator()
        {
            return childElements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
