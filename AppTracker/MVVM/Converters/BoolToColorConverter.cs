using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LauncherApp.Converters;

public class BoolToColorConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is string colors)
        {
            var parts = colors.Split('|');
            return (bool)value 
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString(parts[0]))
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString(parts[1]));
        }
        return Brushes.Gray;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}