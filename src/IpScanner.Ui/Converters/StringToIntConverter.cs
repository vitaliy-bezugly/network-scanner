using System;
using Windows.UI.Xaml.Data;

namespace IpScanner.Ui.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            int result;
            if (int.TryParse(value.ToString(), out result))
            {
                return result;
            }

            return 0;
        }
    }
}
