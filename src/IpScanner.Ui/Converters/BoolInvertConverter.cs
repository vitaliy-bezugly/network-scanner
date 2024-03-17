using System;
using Windows.UI.Xaml.Data;

namespace IpScanner.Ui.Converters
{
    public class BoolInvertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool bValue)
            {
                return !bValue;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool bValue)
            {
                return !bValue;
            }

            return value;
        }
    }
}
