﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace LauncherApp.Converters;

public class InvertBoolConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool b ? !b : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}