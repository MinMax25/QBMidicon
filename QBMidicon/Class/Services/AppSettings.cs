using libQB.Enums;
using Tomlyn.Serialization;

namespace QBMidicon.Class.Services;

public sealed class AppSettings
{
    [TomlConverter(typeof(EnumTomlConverter<BaseTheme>))]
    public BaseTheme BaseTheme { get; set; } = BaseTheme.Dark;

    public string ThemeColor { get; set; } = "Steel";
    public string Language { get; set; } = "en-US";
    public string LastOpenedFilePath { get; set; } = string.Empty;
    public string LastSaveFolderPath { get; set; } = string.Empty;
}

internal sealed class EnumTomlConverter<TEnum> : TomlConverter<TEnum>
    where TEnum : struct, Enum
{
    public override TEnum Read(TomlReader reader) => Enum.Parse<TEnum>(reader.GetString());

    public override void Write(TomlWriter writer, TEnum value) => writer.WriteStringValue(value.ToString());
}
