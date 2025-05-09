using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LauncherApp.Converters;

public class BoolToVisibilityConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolValue) return Visibility.Collapsed;
        
        var invert = parameter?.ToString() == "Reverse";
        return (boolValue ^ invert) ? Visibility.Visible : Visibility.Collapsed;
    }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}