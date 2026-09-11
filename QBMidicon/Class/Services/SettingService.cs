using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using libQB.Attributes;
using libQB.Enums;
using libQB.Settings;
using QBMidicon.Services;

namespace QBMidicon.Class.Services;

[DISingleton<ISettingService>]
public partial class SettingService : ObservableObject, ISettingService
{
    private readonly ISettingStore<AppSettings> _settingStore;
    private readonly IThemeSelectorService _themeSelector;

    [ObservableProperty]
    private BaseTheme baseTheme = BaseTheme.Dark;

    [ObservableProperty]
    private string themeColor = "Steel";

    [ObservableProperty]
    private string language = "en-US";

    [ObservableProperty]
    private string lastOpenedFilePath = string.Empty;

    [ObservableProperty]
    private string lastSaveFolderPath = string.Empty;

    public SettingService(ISettingStore<AppSettings> settingStore, IThemeSelectorService themeSelector)
    {
        _settingStore = settingStore;
        _themeSelector = themeSelector;
    }

    partial void OnBaseThemeChanged(BaseTheme value) => _themeSelector?.SetTheme(value, ThemeColor);

    partial void OnThemeColorChanged(string value) => _themeSelector?.SetTheme(BaseTheme, value);

    partial void OnLanguageChanged(string value) => ApplyLanguage(value);

    public void Load()
    {
        var settings = _settingStore.Load();
        BaseTheme = settings.BaseTheme;
        ThemeColor = settings.ThemeColor;
        Language = settings.Language;
        LastOpenedFilePath = settings.LastOpenedFilePath;
        LastSaveFolderPath = settings.LastSaveFolderPath;

        ApplyLanguage(Language);
        _themeSelector.SetTheme(BaseTheme, ThemeColor);
    }

    public void Save()
    {
        _settingStore.Save(new AppSettings
        {
            BaseTheme = BaseTheme,
            ThemeColor = ThemeColor,
            Language = Language,
            LastOpenedFilePath = LastOpenedFilePath,
            LastSaveFolderPath = LastSaveFolderPath
        });
    }

    private static void ApplyLanguage(string cultureCode)
    {
        CultureInfo culture;
        try
        {
            culture = new CultureInfo(cultureCode);
        }
        catch (CultureNotFoundException)
        {
            culture = new CultureInfo("en-US");
        }

        Properties.Resources.Culture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        libQB.Localization.LocalizationManager.Instance.SetCulture(culture.Name);
    }

    public void Dispose()
    {
    }
}
