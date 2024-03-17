﻿using System;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using IpScanner.Helpers.Extensions;
using Windows.UI.Xaml.Data;

namespace IpScanner.Ui.Converters
{
    public class MacAddressToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is PhysicalAddress macAddress))
            {
                return string.Empty;
            }

            return macAddress.ToFormattedString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var macAddress = value as string;
            if (string.IsNullOrEmpty(macAddress))
            {
                return PhysicalAddress.None;
            }

            // Validate if the format is XX:XX:XX:XX:XX:XX where X is a hex digit
            if (Regex.IsMatch(macAddress, @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$"))
            {
                return PhysicalAddress.Parse(string.Concat(macAddress.Split(new[] { ':', '-' })));
            }

            return PhysicalAddress.None;
        }

    }
}
