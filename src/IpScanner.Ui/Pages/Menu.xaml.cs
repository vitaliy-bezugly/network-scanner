using CommunityToolkit.Mvvm.DependencyInjection;
using IpScanner.ViewModels.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml.Controls;

namespace IpScanner.Ui.Pages
{
    public sealed partial class Menu : Page
    {
        public Menu()
        {
            this.InitializeComponent();
            ViewModel = Ioc.Default.GetService<MenuViewModel>();
        }

        public MenuViewModel ViewModel { get; }
    }
}
