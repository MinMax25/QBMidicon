#nullable enable

using System.ComponentModel;
using libQB.Localization;

namespace QBMidicon.Class;

public sealed class LanguageGroupHeaderItem : INotifyPropertyChanged
{
    public static readonly LanguageGroupHeaderItem BuiltIn = new("QBMidicon.Resources.BuiltInLanguages");
    public static readonly LanguageGroupHeaderItem LanguagePacks = new("QBMidicon.Resources.LanguagePacks");

    private readonly string key;

    private LanguageGroupHeaderItem(string key)
    {
        this.key = key;
        LocalizationManager.Instance.PropertyChanged += (_, _) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
    }

    // ComboBox.SelectedValuePath="Code" 評価時にプロパティ欠落による Binding Error が発生するのを防止する
    public string Code => string.Empty;

    public string DisplayName => LocalizationManager.Instance[key] ?? key;

    public event PropertyChangedEventHandler? PropertyChanged;
}
