using System.Windows.Media;
using ControlzEx.Theming;
using libQB.Attributes;
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