using System;
using Windows.UI.Xaml.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using IpScanner.Services.Abstract;

namespace IpScanner.ViewModels.Options
{
    public class OptionsPageViewModel : ObservableObject
    {
        private readonly INavigationService navigationService;

        public OptionsPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public void NavigateTo(Type pageType, Frame frame)
        {
            navigationService.NavigateToPage(frame, pageType);
        }
    }
}
