using System.Globalization;
using System.Windows.Data;
using libMidi;
using libMidi.SMF.interfaces;

namespace QBMidicon.Class.ValueConverters;

public class BreakedTimeConverter
    : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not IMidiEvent midiEvent)
            return string.Empty;
        return midiEvent.BreakedTime();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
