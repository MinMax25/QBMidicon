using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using libMidi.Messages;
using libMidi.SMF;
using libMidi.SMF.interfaces;
using libQB.Attributes;
using QBMidicon.Class;
using QBMidicon.Class.Services;
using QBMidicon.Views;

namespace QBMidicon.ViewModels;

[DIPage<SourceDataPage>]
public partial class SourceDataViewModel : ViewModelBase
{
    #region Fields

    private MidiData _midiData;

    [ObservableProperty]
    private ITrack _selectedTrack;

    [ObservableProperty]
    private ObservableCollection<MidiEvent> _tempo = new ObservableCollection<MidiEvent>();

    [ObservableProperty]
    private ObservableCollection<MidiEvent> _key = new ObservableCollection<MidiEvent>();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ReloadCommand))]
    private bool _canReload;

    #endregion

    #region Properties

    public ObservableCollection<ITrack> Tracks
    {
        get
        {
            return _midiData.Origin != null ? new ObservableCollection<ITrack>(_midiData.Origin.Tracks) : null!;
        }
    }

    public string FilePath
    {
        get
        {
            return _midiData?.Origin == null ? string.Empty : _midiData.FilePath;
        }
    }

    public string Format
    {
        get
        {
            return _midiData?.Origin == null ? string.Empty : _midiData.Origin.Format.ToString();
        }
    }

    public string NumberOfTracks
    {
        get
        {
            return _midiData?.Origin == null ? string.Empty : _midiData.Origin.NumberOfTrack.ToString();
        }
    }

    public string Division
    {
        get
        {
            return _midiData?.Origin == null ? string.Empty : $"{_midiData.Division}";
        }
    }

    public string MidiStd
    {
        get
        {
            return _midiData?.Origin == null ? string.Empty : _midiData.Origin.MidiStd.ToString();
        }
    }

    public string Lyric
    {
        get
        {
            return _midiData?.Origin == null ? string.Empty : _midiData.LyricTrack?.Lyric ?? string.Empty;
        }
    }

    public string SRT
    {
        get
        {
            if (_midiData.Origin?.Tracks.FirstOrDefault(x => x is XFKaraokeMessage) is not XFKaraokeMessage karaoke)
            {
                return string.Empty;
            }

            return karaoke.GetSRT(SMFConverter.Def.Setting.SRTOffset, SMFConverter.Def.Setting.SRTRemoveComment);
        }
    }

    #endregion

    #region ctor

    public SourceDataViewModel(IDIContainer dIContainer, MidiData midiData) : base(dIContainer)
    {
        _midiData = midiData;

        SetDetail();
        CanReload = string.IsNullOrWhiteSpace(FilePath) == false;
        _midiData.Converted += OnMidiDataConverted;
    }

    #endregion

    #region Methods

    #region Property Change Handler

    private void OnMidiDataConverted(object sender, EventArgs e)
    {
        OnPropertyChanged(nameof(Tracks));
        OnPropertyChanged(nameof(FilePath));
        OnPropertyChanged(nameof(Format));
        OnPropertyChanged(nameof(NumberOfTracks));
        OnPropertyChanged(nameof(Division));
        OnPropertyChanged(nameof(MidiStd));
        OnPropertyChanged(nameof(Lyric));
        OnPropertyChanged(nameof(SRT));

        SetDetail();

        CanReload = string.IsNullOrWhiteSpace(FilePath) == false;
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

    [RelayCommand(CanExecute = nameof(CanExecuteReload))]
    public async Task OnReloadAsync()
    {
        if (await Dialog.ShowConfirmAsync("", "Reload") == false)
        {
            return;
        }

        var result = SMFLoader.Load(FilePath);
        var convtype = result.IsMultiTimber ? libMidi.SMF.enums.ConvertType.MultiTimber : libMidi.SMF.enums.ConvertType.Instrument;

        SMFConverter.Convert(convtype, result, _midiData);
        _midiData.Converted?.Invoke(_midiData, new EventArgs());
    }

    private bool CanExecuteReload()
    {
        return CanReload;
    }

    #endregion

    #region General

    private void SetDetail()
    {
        Tempo.Clear();

        foreach (var midiEvent in _midiData.GetAllEvents().Where(x => x.Message is Tempo))
        {
            Tempo.Add(midiEvent);
        }

        Key.Clear();

        foreach (var midiEvent in _midiData.GetAllEvents().Where(x => x.Message is KeySignature))
        {
            Key.Add(midiEvent);
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