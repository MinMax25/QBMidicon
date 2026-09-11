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

    private static readonly Dictionary<string, string> DisplayNameOverrides = new()
    {
        ["zh-TW"] = "繁體中文",
        ["zh-CN"] = "简体中文",
        ["ko-KR"] = "한국어",
        ["es-ES"] = "Español",
        ["pt-BR"] = "Português (Brasil)",
        ["de-DE"] = "Deutsch",
        ["fr-FR"] = "Français",
    };

    private static readonly string[] BuiltInOrder = { "ja-JP", "en-US" };

    public IReadOnlyList<object> AvailableLanguages
    {
        get
        {
            var languages = LocalizationManager.Instance.AvailableLanguages
                .Select(l =>
                {
                    return DisplayNameOverrides.TryGetValue(l.Code, out var name)
                        ? new LanguageInfo { Code = l.Code, DisplayName = name, IsBuiltIn = l.IsBuiltIn }
                        : l;
                })
                .ToList();

            var builtIn = BuiltInOrder
                .Select(code =>
                {
                    return languages.FirstOrDefault(l => l.Code == code);
                })
                .Where(l => l != null)
                .ToList();

            var external = languages
                .Where(l => l.IsBuiltIn == false)
                .OrderBy(l => l.DisplayName, StringComparer.Ordinal)
                .ToList();

            var result = new List<object>();

            if (builtIn.Count > 0)
            {
                result.Add(LanguageGroupHeaderItem.BuiltIn);
                result.AddRange(builtIn!);
            }

            if (external.Count > 0)
            {
                result.Add(LanguageGroupHeaderItem.LanguagePacks);
                result.AddRange(external);
            }

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
