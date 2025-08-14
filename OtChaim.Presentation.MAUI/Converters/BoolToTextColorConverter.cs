using System.Globalization;

namespace OtChaim.Presentation.MAUI.Converters;

public class BoolToTextColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isSelected)
        {
            return isSelected ? Colors.White : Colors.Black;
        }
        return Colors.Black;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
