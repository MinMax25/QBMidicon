using System.Globalization;
using System.Windows.Data;
using libMidi.Messages;
using libMidi.SMF;

namespace QBMidicon.Class.ValueConverters;

public class MidiMessageConverter
    : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not MidiEvent midiEvent)
            return string.Empty;
        return
            $"{midiEvent.Message}" +
            (
                midiEvent.Message is ProgramChange && midiEvent.InstrumentInfo != null
                ? $" ({InstMap.GetInstName(midiEvent.InstrumentInfo, midiEvent.Parent?.Parent?.IsDrum(midiEvent) ?? false)})"
                : string.Empty
            );
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
