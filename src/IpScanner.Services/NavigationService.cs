using System;
using Windows.UI.Xaml.Controls;
using IpScanner.Services.Abstract;
using IpScanner.Services.Containers;

namespace IpScanner.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame mainFrame;
        private readonly IFramesContainer framesContainer;

        public NavigationService(Frame frame, IFramesContainer framesContainer)
        {
            mainFrame = frame;
            this.framesContainer = framesContainer;
        }

        public void NavigateToPage(Frame frame, Type typeOfPage)
        {
            frame.Navigate(typeOfPage);
        }

        public void ReloadMainPage()
        {
            mainFrame.Navigate(mainFrame.Content.GetType());
        }

        public void ReloadChildPages()
        {
            foreach (var frame in framesContainer)
            {
                frame.Navigate(frame.Content.GetType());
            }
        }
    }
}
