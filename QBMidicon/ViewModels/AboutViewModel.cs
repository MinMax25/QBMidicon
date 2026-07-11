using System.IO;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using libQB.Attributes;
using QBMidicon.Class;
using QBMidicon.Class.Services;
using QBMidicon.Views;

namespace QBMidicon.ViewModels;

[DIWindow<About>]
public partial class AboutViewModel : ViewModelBase
{
    #region Fields

    [ObservableProperty]
    private string _license;

    [ObservableProperty]
    private string _applicationName;

    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    private string _copyright = "© 2025 Min Max";

    #endregion

    #region ctor

    public AboutViewModel(IDIContainer diContainer) : base(diContainer)
    {
        InitializeData();
    }

    public AboutViewModel()
    {
        InitializeData();
    }

    #endregion

    #region Methods

    #region General

    private void InitializeData()
    {
        ApplicationName = typeof(App).Assembly.GetName().Name;

        var fullName = typeof(App).Assembly.Location;
        var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(fullName);

        Version = $"Version {info.FileVersion}";

        var baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.Combine(baseDirectory, @"Resources\license.txt");

        if (File.Exists(path))
        {
            License = File.ReadAllText(path);
        }
    }

    #endregion

    #endregion
}