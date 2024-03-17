﻿using System;
using Windows.UI.Xaml.Data;
using IpScanner.Models.Enums;

namespace IpScanner.Ui.Converters
{
    public class DeviceStatusToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var status = (DeviceStatus)value;

            switch (status)
            {
                case DeviceStatus.Online:
                    return "\uE701";
                case DeviceStatus.Offline:
                    return "\uEB63";
                case DeviceStatus.Unknown:
                    return "\uE9CE";
                default:
                    return "\uE9CE";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();    
        }
    }
}
