using System;
using IpScanner.Services.Abstract;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;
using IpScanner.Services.Containers;

namespace IpScanner.Services
{
    public class ModalsService : IModalsService
    {
        private readonly IChildElementsContainer childElementsContainer;
        private readonly IContentBarCustomizationService contentBarCustomizationService;
        private readonly IAppWindowManager appWindowManager;

        public ModalsService(IChildElementsContainer childElementsContainer,
            IContentBarCustomizationService contentBarCustomizationService,
            IAppWindowManager appWindowManager)
        {
            this.childElementsContainer = childElementsContainer;
            this.contentBarCustomizationService = contentBarCustomizationService;
            this.appWindowManager = appWindowManager;
        }

        public async Task ShowPageAsync(Type pageType)
        {
            AppWindow appWindow = await appWindowManager.CreateAppWindowAsync(pageType);
            CusomizeTitleBar(appWindow);

            appWindow.Closed += AppWindow_Closed;
            await appWindow.TryShowAsync();
        }

        private void CusomizeTitleBar(AppWindow appWindow)
        {
            AppWindowTitleBar titleBar = appWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;
            contentBarCustomizationService.SetCaptionButtonsBackground(titleBar);
            contentBarCustomizationService.SetInactiveWindowColors(titleBar);
        }

        private void AppWindow_Closed(AppWindow sender, AppWindowClosedEventArgs args)
        {
            childElementsContainer.ClearChildElements();
        }
    }
}
