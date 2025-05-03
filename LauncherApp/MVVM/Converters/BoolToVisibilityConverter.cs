using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LauncherApp.Converters;

public class BoolToVisibilityConverter: IValueConverter
{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Если значение true → Visible, иначе Collapsed
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}