using System.Globalization;
using System.Windows.Data;
using libMidi.Messages;
using QBMidicon.ViewModels;

namespace QBMidicon.Class.ValueConverters;

public class PitchRangeConverter
    : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // value = TrackItemViewModel
        if (value is not TrackItemViewModel vm)
            return string.Empty;

        var min = vm.MinPitch;
        var max = vm.MaxPitch;

        if (!min.HasValue && !max.HasValue)
            return string.Empty;

        if (min == max && min.HasValue)
            return min.Value.NoteName();

        if (min.HasValue && max.HasValue)
            return $"{min.Value.NoteName()} – {max.Value.NoteName()}";

        // 片方だけある場合（保険）
        return min.HasValue
            ? min.Value.NoteName()
            : max!.Value.NoteName();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
