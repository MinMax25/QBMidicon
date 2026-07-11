using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using libQB.Attributes;
using libQB.WindowServices;
using QBMidicon.Class;
using QBMidicon.Class.Services;
using QBMidicon.Views;

namespace QBMidicon.ViewModels;

[DIWindow<SavePage>]
public partial class SaveViewModel : ViewModelBase, IParameterReceiver, IResultProvider<object>
{
    #region Fields

    [ObservableProperty]
    private string _title = "Save SMF File";

    [ObservableProperty]
    private SaveParameter _param = new SaveParameter();

    #endregion

    #region Properties

    private SaveParameter Result { get; set; }

    #endregion

    #region ctor

    public SaveViewModel(IDIContainer diContainer) : base(diContainer)
    {
        PropertyChanged += OnPropertyChanged;
    }

    public SaveViewModel()
    {
    }

    #endregion

    #region Methods

    #region Property Change Handler

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }

    #endregion

    #region Command

    [RelayCommand]
    private async Task OnSelectFolderAsync()
    {
        var path = await Dialog.ShowSelectFolderDialog("Select Folder", Param.FolderPath);

        if (path is string selectedPath)
        {
            Param.FolderPath = selectedPath;
        }
    }

    [RelayCommand]
    private void OnSave(object sender)
    {
        Result = new SaveParameter
        {
            FolderPath = Param.FolderPath,
            FileName = Param.FileName,
            IsRawData = Param.IsRawData,
            IsFilteredData = Param.IsFilteredData,
            IsLyricTrackData = Param.IsLyricTrackData,
            IsLyricTextData = Param.IsLyricTextData,
            IsLyricSRTData = Param.IsLyricSRTData
        };

        if (sender is Window window)
        {
            WindowService.CloseWindow(window);
        }
    }

    #endregion

    #region General

    public void ReceiveParameter(object parameter)
    {
        if (parameter is not SaveParameter p)
        {
            return;
        }

        Param.FolderPath = p.FolderPath;
        Param.FileName = p.FileName;
        Param.IsRawData = p.IsRawData;
        Param.IsFilteredData = p.IsFilteredData;
        Param.IsLyricTrackData = p.IsLyricTrackData;
        Param.IsLyricTextData = p.IsLyricTextData;
        Param.IsLyricSRTData = p.IsLyricSRTData;
    }

    public object GetResult()
    {
        return Result;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            PropertyChanged -= OnPropertyChanged;
        }
    }

    #endregion

    #endregion
}