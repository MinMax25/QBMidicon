using System.Windows.Media;
using ControlzEx.Theming;
using libQB.Attributes;
using libQB.Localization;
using QBMidicon.Class;
using QBMidicon.Class.Services;
using QBMidicon.Views;

namespace QBMidicon.ViewModels;

[DIPage<SettingPage>]
public partial class SettingViewModel : ViewModelBase
{
    #region Properties

    public ISettingService Setting
    {
        get
        {
            return SettingService;
        }
    }

    public Dictionary<string, Brush> AccentColors { get; set; }

    private static readonly string[] BuiltInLanguageOrder = { "ja-JP", "en-US" };

    private static readonly Dictionary<string, string> LanguageDisplayNames = new()
    {
        ["de-DE"] = "Deutsch", ["es-ES"] = "Español", ["fr-FR"] = "Français",
        ["ko-KR"] = "한국어", ["pt-BR"] = "Português (Brasil)",
        ["zh-CN"] = "简体中文", ["zh-TW"] = "繁體中文"
    };

    public IReadOnlyList<object> AvailableLanguages
    {
        get
        {
            var languages = LocalizationManager.Instance.AvailableLanguages
                .Select(x => LanguageDisplayNames.TryGetValue(x.Code, out var displayName)
                    ? new LanguageInfo { Code = x.Code, DisplayName = displayName, IsBuiltIn = x.IsBuiltIn }
                    : x)
                .ToList();
            var result = new List<object>();
            var builtIn = BuiltInLanguageOrder.Select(code => languages.FirstOrDefault(x => x.Code == code)).Where(x => x != null).ToList();
            if (builtIn.Count > 0) { result.Add(LanguageGroupHeaderItem.BuiltIn); result.AddRange(builtIn!); }
            var packs = languages.Where(x => !x.IsBuiltIn).OrderBy(x => x.DisplayName, StringComparer.CurrentCulture).ToList();
            if (packs.Count > 0) { result.Add(LanguageGroupHeaderItem.LanguagePacks); result.AddRange(packs); }
            return result;
        }
    }

    #endregion

    #region ctor

    public SettingViewModel(IDIContainer diContainer) : base(diContainer)
    {
        AccentColors =
            ThemeManager.Current.Themes
            .GroupBy(x => x.ColorScheme)
            .OrderBy(a => a.Key)
            .Select(a => new
            {
                Name = a.Key,
                ColorBrush = a.First().ShowcaseBrush
            })
            .ToDictionary(x => x.Name, x => x.ColorBrush);
    }

    #endregion

    #region Methods

    #endregion
}
