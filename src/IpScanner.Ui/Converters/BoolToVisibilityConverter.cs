using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;

namespace IpScanner.Ui.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var visible = (bool)value;
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var visibility = (Visibility)value;
            return visibility == Visibility.Visible;
        }
    }
}
