using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using libMidi.SMF;
using libQB.Attributes;
using libQB.WindowServices;
using MahApps.Metro.Controls;
using QBMidicon.Class;
using QBMidicon.Class.Enums;
using QBMidicon.Class.Services;
using QBMidicon.Contracts.Views;
using QBMidicon.Properties;
using QBMidicon.Views;

namespace QBMidicon.ViewModels;

[DIWindow<IShellWindow, ShellWindow>]
public partial class ShellViewModel : ViewModelBase, IWindowClosingAware
{
    #region Fields

    private readonly MidiData _midiData;

    private HamburgerMenuItem _selectedMenuItem;

    private RelayCommand _goBackCommand;

    private ICommand _menuItemInvokedCommand;

    private ICommand _loadedCommand;

    private ICommand _unloadedCommand;

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveFileCommand))]
    private bool _canSave;

    #endregion

    #region Properties

    public HamburgerMenuItem SelectedMenuItem
    {
        get
        {
            return _selectedMenuItem;
        }
        set
        {
            SetProperty(ref _selectedMenuItem, value);
        }
    }

    public RelayCommand GoBackCommand
    {
        get
        {
            return _goBackCommand ??= new RelayCommand(OnGoBack, CanGoBack);
        }
    }

    public ICommand MenuItemInvokedCommand
    {
        get
        {
            return _menuItemInvokedCommand ??= new RelayCommand(OnMenuItemInvoked);
        }
    }

    public ICommand LoadedCommand
    {
        get
        {
            return _loadedCommand ??= new RelayCommand(OnLoaded);
        }
    }

    public ICommand UnloadedCommand
    {
        get
        {
            return _unloadedCommand ??= new RelayCommand(OnUnloaded);
        }
    }

    public ObservableCollection<HamburgerMenuItemBase> MenuItems { get; } =
    [
        new HamburgerMenuIconItem()
        {
            Label = Resources.Title_SourceDataPage,
            Icon = ClonePackIcon(PageIcon.SourceDataPage),
            TargetPageType = typeof(SourceDataViewModel)
        },
        new HamburgerMenuIconItem()
        {
            Label = Resources.Title_ConvertedDataPage,
            Icon = ClonePackIcon(PageIcon.ConvertedDataPage),
            TargetPageType = typeof(ConvertedDataViewModel)
        },
        new HamburgerMenuIconItem()
        {
            Label = Resources.Title_EventFilterPage,
            Icon = ClonePackIcon(PageIcon.EventFilterPage),
            TargetPageType = typeof(EventFilterViewModel)
        },
        new HamburgerMenuIconItem()
        {
            Label = Resources.Title_ConvertSettingPage,
            Icon = ClonePackIcon(PageIcon.ConvertSettingPage),
            TargetPageType = typeof(ConvertSettingPageViewModel)
        }
    ];

    public ObservableCollection<HamburgerMenuItemBase> OptionMenuItems { get; } =
    [
        new HamburgerMenuIconItem()
        {
            Label = libQB.Properties.Resources.Setting_PageTitle,
            Icon = ClonePackIcon(PageIcon.SettingPage),
            TargetPageType = typeof(SettingViewModel)
        }
    ];

    private ISettingService Setting
    {
        get
        {
            return SettingService;
        }
    }

    #endregion

    #region ctor

    public ShellViewModel(IDIContainer diContainer, MidiData midiData) : base(diContainer)
    {
        _title = Resources.AppDisplayName;
        _midiData = midiData;
    }

    #endregion

    #region Methods

    #region Command

    [RelayCommand]
    private async Task OnOpenFileAsync()
    {
        var path = await Dialog.ShowOpenFileDialog(libQB.Properties.Dialog.Common_OpenFile, "SMF|*.mid", Setting.LastOpenedFilePath);

        if (path == null)
        {
            return;
        }

        Setting.LastOpenedFilePath = path;

        try
        {
            var result = SMFLoader.Load(path);
            var convtype = result.IsMultiTimber ? libMidi.SMF.enums.ConvertType.MultiTimber : libMidi.SMF.enums.ConvertType.Instrument;

            SMFConverter.Convert(convtype, result, _midiData);
            _midiData.Converted?.Invoke(_midiData, new EventArgs());
            CanSave = true;
        }
        catch (Exception ex)
        {
            CanSave = false;
            await Dialog.ShowErrorAsync(ex.Message);
        }
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void OnSaveFile()
    {
        var firstTrack = _midiData.Origin.Tracks.FirstOrDefault();
        var name = firstTrack?.ToString() ?? "Untitled";

        if (string.IsNullOrWhiteSpace(Setting.LastSaveFolderPath))
        {
            Setting.LastSaveFolderPath = Path.GetDirectoryName(Setting.LastOpenedFilePath);
        }

        var folderPath = Path.Combine(Setting.LastSaveFolderPath, "*.*");

        WindowService.ShowWindowWithCallback<SavePage, SaveViewModel, object>(
            parameter: new SaveParameter(folderPath, name),
            owner: Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive),
            resultCallback: x =>
            {
                if (x is SaveParameter param)
                {
                    if (!Path.Exists(param.FolderPath))
                    {
                        Directory.CreateDirectory(param.FolderPath);
                    }

                    SMFConverter.SaveSetMidiData(
                        _midiData,
                        param.FolderPath,
                        param.FileName,
                        mid: param.IsRawData,
                        nopc: param.IsFilteredData,
                        vocal: param.IsLyricTrackData,
                        srttext: param.IsLyricSRTData,
                        lyrictext: param.IsLyricTextData
                    );

                    Setting.LastSaveFolderPath = param.FolderPath;
                }
            });
    }

    [RelayCommand]
    private void OnShowAbout()
    {
        WindowService.ShowWindowWithCallback<About, AboutViewModel, object>
        (
            owner: Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
        );
    }

    [RelayCommand]
    private void OnShowDocument()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Document\Manual\index.html");

        if (!File.Exists(path))
        {
            return;
        }

        var p = new Process
        {
            StartInfo = new ProcessStartInfo(path)
            {
                UseShellExecute = true
            }
        };

        p.Start();
    }

    [RelayCommand]
    private void OnClose()
    {
        WindowService.CloseWindow(Application.Current.MainWindow);
    }

    [RelayCommand]
    private void OnUndo()
    {
        UndoManager.Undo();
    }

    [RelayCommand]
    private void OnRedo()
    {
        UndoManager.Redo();
    }

    #endregion

    #region Property Change Handler

    private void OnLoaded()
    {
        Navigation.Navigated += OnNavigated;
    }

    private void OnUnloaded()
    {
        Navigation.Navigated -= OnNavigated;
    }

    private void OnNavigated(object sender, string viewModelName)
    {
        var item = MenuItems.OfType<HamburgerMenuItem>()
                            .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);

        if (item != null)
        {
            SelectedMenuItem = item;
        }

        GoBackCommand.NotifyCanExecuteChanged();
    }

    #endregion

    #region EventHandler

    public async Task<bool> OnWindowClosingAsync()
    {
        return await Task.Run(() =>
        {
            return true;
        });
    }

    #endregion

    #region General

    private bool CanGoBack()
    {
        return Navigation.CanGoBack;
    }

    private void OnGoBack()
    {
        Navigation.GoBack();
    }

    private void OnMenuItemInvoked()
    {
        if (SelectedMenuItem is HamburgerMenuIconItem item && item.TargetPageType != null)
        {
            NavigateTo(item.TargetPageType);
        }
    }

    private void NavigateTo(Type targetViewModel)
    {
        if (targetViewModel != null)
        {
            Navigation.NavigateTo(targetViewModel.FullName);
        }
    }

    private static object ClonePackIcon(PageIcon pageIcon)
    {
        var original = Application.Current.Resources[$"{pageIcon}Icon"];

        if (original == null)
        {
            return null;
        }

        string xaml = System.Windows.Markup.XamlWriter.Save(original);
        return System.Windows.Markup.XamlReader.Parse(xaml);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
        }
    }

    #endregion

    #endregion
}