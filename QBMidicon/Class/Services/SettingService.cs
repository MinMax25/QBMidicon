using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using CommunityToolkit.Mvvm.ComponentModel;
using libQB.Attributes;
using libQB.Enums;
using QBMidicon.Services;

namespace QBMidicon.Class.Services
{
    [DISingleton<ISettingService>]
    public partial class SettingService
        : ObservableObject
        , ISettingService
    {
        #region Properties

        private static string SettingFilePath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Setting.json");

        [ObservableProperty]
        private BaseTheme baseTheme = BaseTheme.Dark;

        [ObservableProperty]
        private string themeColor = "Steel";

        [ObservableProperty]
        private Languages language = Languages.English;

        [ObservableProperty]
        public string lastOpenedFilePath = string.Empty;

        [ObservableProperty]
        public string lastSaveFolderPath = string.Empty;

        #endregion

        #region Fields

        private readonly IThemeSelectorService ThemeSelector;

        #endregion

        #region ctor

        public SettingService()
        {

        }

        public SettingService(IThemeSelectorService themeSelectorService)
        {
            ThemeSelector = themeSelectorService;
        }

        #endregion

        #region Methods

        #region PropertyChanged Callbacks

        partial void OnBaseThemeChanged(BaseTheme value)
        {
            ThemeSelector?.SetTheme(value, ThemeColor);
        }

        partial void OnThemeColorChanged(string value)
        {
            ThemeSelector?.SetTheme(BaseTheme, value);
        }

        #endregion

        #region General

        public void Load()
        {
            if (File.Exists(SettingFilePath))
            {
                var jsonString = File.ReadAllText(SettingFilePath);
                if (JsonSerializer.Deserialize<SettingService>(jsonString) is SettingService _restore)
                {
                    GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => x.GetCustomAttribute<JsonIgnoreAttribute>() == null)
                    .ToList()
                    .ForEach(p => p.SetValue(this, p.GetValue(_restore)));
                }
            }

            var culture = Language switch
            {
                Languages.Japanese => new CultureInfo("ja-JP"),
                Languages.English => new CultureInfo("en-US"),
                _ => throw new NotSupportedException()
            };

            libQB.Properties.Dialog.Culture = culture;
            libQB.Properties.Hamburger.Culture = culture;
            libQB.Properties.Menu.Culture = culture;
            libQB.Properties.Resources.Culture = culture;

            Properties.Resources.Culture = culture;

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            ThemeSelector.SetTheme(BaseTheme, ThemeColor);
        }

        public void Save()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize(this, options);
            File.WriteAllText(SettingFilePath, jsonString);
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
        }

        #endregion

        #endregion
    }
}