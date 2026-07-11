using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QBMidicon.Class.ValueConverters;

public class BoolToVisibilityConverter
    : IValueConverter
{
    public bool Invert { get; set; } = false;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool b)
        {
            return Visibility.Hidden;
        }

        if (Invert)
        {
            b = !b;
        }

        return b ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Visibility v)
        {
            return false;
        }

        var result = v == Visibility.Visible;
        return Invert ? !result : result;
    }
}
