using System.Globalization;
using System.Windows.Data;
using libMidi.Messages;
using libMidi.Messages.enums;
using QBMidicon.ViewModels;

namespace QBMidicon.Class.ValueConverters;

public class VolumeConverter
: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not TrackItemViewModel Item)
            return string.Empty;
        var midiEvent = Item.Track.Events.FirstOrDefault(x => x.Message is ControlChange cc && cc.CtrlType == CtrlType.Volume);
        if (midiEvent?.Message is not ControlChange cc)
            return string.Empty;
        return $"{cc.Value}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
