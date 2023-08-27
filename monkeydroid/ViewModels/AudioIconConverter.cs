
using System.Globalization;

namespace monkeydroid.ViewModels;

internal class AudioIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (bool?)value switch
        {
            true  => "\U000F075A ", // music note
            false => "\U000F075B ", // slashed music note
            null  => "\U0000F420 ", // circled question mark
        };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => null;
}
