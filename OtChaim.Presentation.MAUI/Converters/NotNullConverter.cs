using System.Globalization;

namespace OtChaim.Presentation.MAUI.Converters;

/// <summary>
/// Converter that returns true if the value is not null, false otherwise.
/// </summary>
public class NotNullConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
