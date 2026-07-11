using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using libQB.ValueConverters;

namespace QBMidicon.Class.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<OctaveTranspose>))]
[TypeConverter(typeof(EnumDisplayTypeConverter))]
public enum OctaveTranspose
{
    [Display(Name = "-1 oct")]
    Minus1 = -1,

    [Display(Name = "0 oct")]
    Zero = 0,

    [Display(Name = "+1 oct")]
    Plus1 = 1
}
