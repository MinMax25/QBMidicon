using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using libMidi.SMF;
using libQB.Attributes;
using QBMidicon.Class;
using QBMidicon.Class.Services;
using QBMidicon.Views;

namespace QBMidicon.ViewModels;

[DIPage<ConvertedDataPage>]
public partial class ConvertedDataViewModel : ViewModelBase
{
    #region Fields

    private MidiData _midiData;

    [ObservableProperty]
    private ObservableCollection<TrackItemViewModel> _tracks = new ObservableCollection<TrackItemViewModel>();

    [ObservableProperty]
    private TrackItemViewModel _selectedTrack;

    #endregion

    #region ctor

    public ConvertedDataViewModel(IDIContainer dIContainer, MidiData midiData) : base(dIContainer)
    {
        _midiData = midiData;

        SetDetail();
        _midiData.Converted += OnMidiDataConverted;
    }

    #endregion

    #region Methods

    #region Property Change Handler

    private void OnMidiDataConverted(object sender, EventArgs e)
    {
        SetDetail();
    }

    #endregion

    #region Command

    [RelayCommand]
    public void OnShowTrackDetail()
    {
        if (SelectedTrack == null)
        {
            return;
        }

        WindowService.ShowWindowWithCallback<TrackDetailWindow, TrackDetailViewModel, object>
        (
            parameter: SelectedTrack,
            owner: Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
        );
    }

    #endregion

    #region General

    private void SetDetail()
    {
        Tracks.Clear();

        foreach (var track in _midiData.Tracks)
        {
            Tracks.Add(new TrackItemViewModel(track));
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _midiData.Converted -= OnMidiDataConverted;
        }
    }

    #endregion

    #endregion
}