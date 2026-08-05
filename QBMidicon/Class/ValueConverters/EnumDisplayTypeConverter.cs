#nullable enable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace QBMidicon.Class.ValueConverters;

public sealed class EnumDisplayTypeConverter : EnumConverter
{
    public EnumDisplayTypeConverter(Type type) : base(type)
    {
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (destinationType == typeof(string) && value is Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault()
                ?.GetCustomAttribute<DisplayAttribute>()?.Name ?? enumValue.ToString();
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
