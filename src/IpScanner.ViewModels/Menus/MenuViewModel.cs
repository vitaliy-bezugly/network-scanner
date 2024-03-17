using CommunityToolkit.Mvvm.ComponentModel;

namespace IpScanner.ViewModels.Menus
{
    public class MenuViewModel : ObservableObject
    {
        private readonly ViewMenuViewModel viewMenuViewModel;
        private readonly FileMenuViewModel fileMenuViewModel;
        private readonly HelpMenuViewModel helpMenuViewModel;
        private readonly SettingsMenuViewModel settingsMenuViewModel;

        public MenuViewModel(ViewMenuViewModel viewMenuViewModel, 
               FileMenuViewModel fileMenuViewModel, 
               SettingsMenuViewModel settingsMenuViewModel, 
               HelpMenuViewModel helpMenuViewModel)
        {
            this.viewMenuViewModel = viewMenuViewModel;
            this.fileMenuViewModel = fileMenuViewModel;
            this.settingsMenuViewModel = settingsMenuViewModel;
            this.helpMenuViewModel = helpMenuViewModel;
        }

        public ViewMenuViewModel ViewViewModel => viewMenuViewModel;

        public FileMenuViewModel FileViewModel => fileMenuViewModel;

        public SettingsMenuViewModel SettingsViewModel => settingsMenuViewModel;

        public HelpMenuViewModel HelpViewModel => helpMenuViewModel;
    }
}
