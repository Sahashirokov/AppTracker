using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace LauncherApp.Converters;

public class AllTrueToVisibilityConverter: IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        return values.All(v => v is bool b && b) 
            ? Visibility.Visible 
            : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}