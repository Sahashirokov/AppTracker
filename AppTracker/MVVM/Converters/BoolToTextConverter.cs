using System;
using System.Globalization;
using System.Windows.Data;

namespace LauncherApp.Converters;

public class BoolToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is string paramsString)
        {
            var parts = paramsString.Split('|');
            return (bool)value ? parts[0] : parts[1];
        }
        return (bool)value ? "Yes" : "No";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}