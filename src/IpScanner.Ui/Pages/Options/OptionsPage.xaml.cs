using Windows.UI.Xaml.Controls;
using IpScanner.ViewModels.Options;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using IpScanner.Helpers.Markers;

namespace IpScanner.Ui.Pages.Options
{
    public sealed partial class OptionsPage : Page, IOptionsPageMarker
    {
        public OptionsPage()
        {
            this.InitializeComponent();

            ViewModel = Ioc.Default.GetService<OptionsPageViewModel>();
            ViewModel.NavigateTo(typeof(ColorThemePage), ContentFrame);
        }

        public OptionsPageViewModel ViewModel { get; }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (!(args.SelectedItemContainer.Tag is string tag))
            {
                throw new NullReferenceException("The tag of the selected item is null.");
            }

            Type pageType = Type.GetType($"IpScanner.Ui.Pages.Options.{tag}Page") 
                ?? throw new NullReferenceException($"The page type for tag '{tag}' is null.");

            ViewModel.NavigateTo(pageType, ContentFrame);
        }
    }
}
