using System.Globalization;

namespace OtChaim.Presentation.MAUI.Converters;

/// <summary>
/// Converter that inverts a boolean value.
/// </summary>
public class InverseBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool boolValue ? !boolValue : throw new ArgumentException($"{GetType().Name} cannot convert non bool values", nameof(value));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
