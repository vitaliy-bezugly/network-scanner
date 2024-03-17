using System;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Services.Abstract
{
    public interface INavigationService
    {
        void ReloadMainPage();
        void ReloadChildPages();
        void NavigateToPage(Frame frame, Type typeOfPage);
    }
}
