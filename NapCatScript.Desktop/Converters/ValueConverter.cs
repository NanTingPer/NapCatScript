using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace NapCatScript.Desktop.Converters;

public class ValueConverter : IValueConverter
{
    /// <summary>
    /// 除以 parameter
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (double.TryParse(value?.ToString(), out double fontSize) &&
            double.TryParse(parameter.ToString(), out double chushu)) {
            return fontSize / chushu;
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}