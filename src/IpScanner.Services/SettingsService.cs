using IpScanner.Helpers;
using Windows.Storage;

namespace IpScanner.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly AppSettings appSettings;

        public SettingsService()
        {
            appSettings = LoadSettings();
        }

        public void SaveSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var composite = new ApplicationDataCompositeValue();

            foreach (var property in typeof(AppSettings).GetProperties())
            {
                if (property.CanRead)
                {
                    var value = property.GetValue(appSettings);
                    composite[property.Name] = value;
                }
            }

            localSettings.Values["AppSettings"] = composite;
        }

        public AppSettings Settings => appSettings;

        private AppSettings LoadSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var composite = (ApplicationDataCompositeValue)localSettings.Values["AppSettings"];

            var appSettings = new AppSettings();

            if (composite != null)
            {
                FillSettingsObject(appSettings, composite);
            }

            return appSettings;
        }

        private void FillSettingsObject(AppSettings appSettings, ApplicationDataCompositeValue composite)
        {
            foreach (var property in typeof(AppSettings).GetProperties())
            {
                if (property.CanWrite && composite.ContainsKey(property.Name))
                {
                    property.SetValue(appSettings, composite[property.Name]);
                }
            }
        }
    }
}
