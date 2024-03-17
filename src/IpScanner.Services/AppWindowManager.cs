using System;
using System.Linq;
using IpScanner.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using IpScanner.Services.Containers;

namespace IpScanner.Services
{
    internal class AppWindowManager : IAppWindowManager
    {
        private readonly IDictionary<Type, AppWindow> windows;
        private readonly IColorThemeService colorThemeService;
        private readonly IChildElementsContainer childElementsContainer;

        public AppWindowManager(IColorThemeService colorThemeService, IChildElementsContainer childElementsContainer)
        {
            windows = new Dictionary<Type, AppWindow>();
            this.colorThemeService = colorThemeService;
            this.childElementsContainer = childElementsContainer;
        }

        public async Task<AppWindow> CreateAppWindowAsync(Type pageType)
        {
            AppWindow appWindow = await AppWindow.TryCreateAsync();
            appWindow.Closed += AppWindow_Closed;

            var appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(pageType);
            RegisterFrameForThemeChanging(appWindowContentFrame);

            ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);

            windows.Add(pageType, appWindow);
            return appWindow;
        }

        public async Task CloseAllAppWindowsAsync()
        {
            foreach (var (pageType, window) in windows)
            {
                await window.CloseAsync();
            }

            windows.Clear();
        }

        public bool Contains(Type pageType)
        {
            return windows.ContainsKey(pageType);
        }

        private void AppWindow_Closed(AppWindow sender, AppWindowClosedEventArgs args)
        {
            var item = windows.FirstOrDefault(x => x.Value == sender);

            if (item.Key != null)
            {
                windows.Remove(item.Key);
            }
        }

        private void RegisterFrameForThemeChanging(Frame frame)
        {
            childElementsContainer.AddChildElement(frame);
            colorThemeService.SetColorTheme(frame, colorThemeService.GetColorTheme());
        }
    }
}
