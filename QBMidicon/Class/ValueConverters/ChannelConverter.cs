using System.Globalization;
using System.Windows.Data;
using libMidi.SMF.interfaces;

namespace QBMidicon.Class.ValueConverters;

public class ChannelConverter
    : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not IMidiEvent midiEvent)
            return string.Empty;
        return midiEvent.Channel == 0 ? "--" : $"{midiEvent.Channel}".PadLeft(2);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
