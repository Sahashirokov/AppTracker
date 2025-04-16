using System;
using System.Globalization;
using System.Windows.Data;

namespace LauncherApp.Converters;

public class BoolToStartTextConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? "Остановить" : "Запустить";
    }
    
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}