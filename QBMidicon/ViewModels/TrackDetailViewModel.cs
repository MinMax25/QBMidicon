using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using libMidi.SMF;
using libMidi.SMF.interfaces;
using libQB.Attributes;
using libQB.WindowServices;
using QBMidicon.Class;
using QBMidicon.Class.Services;
using QBMidicon.Views;

namespace QBMidicon.ViewModels;

[DIWindow<TrackDetailWindow>]
public partial class TrackDetailViewModel : ViewModelBase, IParameterReceiver
{
    #region Fields

    private TrackItemViewModel _trackVM;

    [ObservableProperty]
    private ObservableCollection<MidiEvent> _events = new ObservableCollection<MidiEvent>();

    [ObservableProperty]
    private bool _filterOn = false;

    [ObservableProperty]
    private bool _filterOff = false;

    [ObservableProperty]
    private bool _lyricMatched;

    [ObservableProperty]
    private ObservableCollection<FilterItem> _filterMeta = new ObservableCollection<FilterItem>();

    [ObservableProperty]
    private ObservableCollection<FilterItem> _filterChannelVoice = new ObservableCollection<FilterItem>();

    [ObservableProperty]
    private ObservableCollection<FilterItem> _filterSysEx = new ObservableCollection<FilterItem>();

    [ObservableProperty]
    private ObservableCollection<FilterItem> _filterControlChange = new ObservableCollection<FilterItem>();

    [ObservableProperty]
    private bool _isNotSourceTrack = true;

    #endregion

    #region Properties

    private ITrack Track
    {
        get
        {
            return _trackVM.Track;
        }
    }

    private IEnumerable<FilterItem> AllFilters
    {
        get
        {
            return FilterMeta
                .Union(FilterChannelVoice)
                .Union(FilterSysEx)
                .Union(FilterControlChange);
        }
    }

    #endregion

    #region ctor

    public TrackDetailViewModel(IDIContainer dIContainer) : base(dIContainer)
    {
        foreach (var item in SMFConverter.Def.InitMeta)
        {
            var filter = new FilterItem(item.Key, item.Value, SMFConverter.Def.Filter.ContainsKey(item.Key));
            filter.PropertyChanged -= Filter_PropertyChanged;
            filter.PropertyChanged += Filter_PropertyChanged;
            _filterMeta.Add(filter);
        }

        foreach (var item in SMFConverter.Def.InitChannelVoice)
        {
            var filter = new FilterItem(item.Key, item.Value, SMFConverter.Def.Filter.ContainsKey(item.Key));
            filter.PropertyChanged -= Filter_PropertyChanged;
            filter.PropertyChanged += Filter_PropertyChanged;
            _filterChannelVoice.Add(filter);
        }

        foreach (var item in SMFConverter.Def.InitSysEx)
        {
            var filter = new FilterItem(item.Key, item.Value, SMFConverter.Def.Filter.ContainsKey(item.Key));
            filter.PropertyChanged -= Filter_PropertyChanged;
            filter.PropertyChanged += Filter_PropertyChanged;
            _filterSysEx.Add(filter);
        }

        foreach (var item in SMFConverter.Def.InitControlChange)
        {
            var filter = new FilterItem(item.Key, item.Value, SMFConverter.Def.Filter.ContainsKey(item.Key));
            filter.PropertyChanged -= Filter_PropertyChanged;
            filter.PropertyChanged += Filter_PropertyChanged;
            _filterControlChange.Add(filter);
        }
    }

    #endregion

    #region Methods

    #region Property Change Handler

    partial void OnFilterOnChanged(bool value)
    {
        if (value == false)
        {
            return;
        }

        Track.FilterEnabled = true;
        Track.LyricMatched = false;
        ShowEvents();
        UpdateTrackVMFlags();
    }

    partial void OnFilterOffChanged(bool value)
    {
        if (value == false)
        {
            return;
        }

        Track.FilterEnabled = false;
        Track.LyricMatched = false;
        ShowEvents();
        UpdateTrackVMFlags();
    }

    partial void OnLyricMatchedChanged(bool value)
    {
        if (value == false)
        {
            return;
        }

        Track.FilterEnabled = true;
        Track.LyricMatched = true;
        ShowEvents();
        UpdateTrackVMFlags();
    }

    private void Filter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is not FilterItem filterItem)
        {
            return;
        }

        if (filterItem.IsChecked)
        {
            if (Track.Filter.ContainsKey(filterItem.Key) == false)
            {
                Track.Filter.Add(filterItem.Key, filterItem.Value);
            }
        }
        else
        {
            if (Track.Filter.ContainsKey(filterItem.Key) == true)
            {
                Track.Filter.Remove(filterItem.Key);
            }
        }

        if (FilterOn)
        {
            ShowEvents();
        }
    }

    #endregion

    #region Command

    [RelayCommand]
    private void OnUncheckAllFilters()
    {
        foreach (var item in AllFilters)
        {
            item.IsChecked = false;
        }

        if (FilterOn)
        {
            ShowEvents();
        }
    }

    [RelayCommand]
    private void OnResetDefaultFilters()
    {
        foreach (var item in AllFilters)
        {
            item.IsChecked = SMFConverter.Def.Filter.ContainsKey(item.Key);
        }

        if (FilterOn)
        {
            ShowEvents();
        }
    }

    #endregion

    #region General

    public void ReceiveParameter(object parameter)
    {
        switch (parameter)
        {
            case TrackItemViewModel vm:
                _trackVM = vm;
                IsNotSourceTrack = true;
                break;

            case ITrack track:
                _trackVM = new TrackItemViewModel(track);
                IsNotSourceTrack = false;
                break;
        }

        if (_trackVM == null)
        {
            return;
        }

        if (Track.LyricMatched)
        {
            LyricMatched = true;
        }
        else
        {
            Track.FilterEnabled = false;
            FilterOff = true;
        }

        ShowEvents();
    }

    private void ShowEvents()
    {
        if (Track == null)
        {
            return;
        }

        Events.Clear();

        if (Track.FilterEnabled)
        {
            Track.DoFilter();
        }

        foreach (var ev in Track.FilterEnabled ? Track.FilteredEvents : Track.Events)
        {
            Events.Add(ev);
        }
    }

    protected void UpdateTrackVMFlags()
    {
        _trackVM.FilterEnabled = Track.FilterEnabled;
        _trackVM.LyricMatched = Track.LyricMatched;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            foreach (var item in AllFilters)
            {
                item.PropertyChanged -= Filter_PropertyChanged;
            }
        }
    }

    #endregion

    #endregion
}