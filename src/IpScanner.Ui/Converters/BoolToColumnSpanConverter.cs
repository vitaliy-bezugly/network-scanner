using System;
using Windows.UI.Xaml.Data;

namespace IpScanner.Ui.Converters
{
    public class BoolToColumnSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool span = (bool)value;
            return span ? 1 : 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            int span = (int)value;
            return span == 1;
        }
    }
}
