using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HayNotas.Converters;

public class RoleToBubbleColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString() == "user"
            ? new SolidColorBrush(Color.FromRgb(0x89, 0xB4, 0xFA)) // Accent blue
            : new SolidColorBrush(Color.FromRgb(0x31, 0x32, 0x44)); // Surface
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
