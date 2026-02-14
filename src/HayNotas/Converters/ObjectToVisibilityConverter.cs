using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HayNotas.Converters;

public class ObjectToVisibilityConverter : IValueConverter
{
    public static readonly ObjectToVisibilityConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is not null ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class ObjectToBoolConverter : IValueConverter
{
    public static readonly ObjectToBoolConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is not null;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
