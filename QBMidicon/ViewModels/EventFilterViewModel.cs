using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using libMidi.SMF;
using libQB.Attributes;
using QBMidicon.Class;
using QBMidicon.Class.Services;
using QBMidicon.Views;

namespace QBMidicon.ViewModels;

[DIPage<EventFilterPage>]
public partial class EventFilterViewModel : ViewModelBase
{
    #region Fields

    private MidiData _midiData;

    [ObservableProperty]
    private ObservableCollection<FilterItem> _filterMeta = new ObservableCollection<FilterItem>();

    [ObservableProperty]
    private ObservableCollection<FilterItem> _filterChannelVoice = new ObservableCollection<FilterItem>();

    [ObservableProperty]
    private ObservableCollection<FilterItem> _filterSysEx = new ObservableCollection<FilterItem>();

    [ObservableProperty]
    private ObservableCollection<FilterItem> _filterControlChange = new ObservableCollection<FilterItem>();

    #endregion

    #region ctor

    public EventFilterViewModel(IDIContainer dIContainer, MidiData midiData) : base(dIContainer)
    {
        _midiData = midiData;

        foreach (var item in SMFConverter.Def.InitMeta)
        {
            var filter = new FilterItem(item.Key, item.Value, SMFConverter.Def.Filter.ContainsKey(item.Key));
            filter.PropertyChanged -= Filter_PropertyChanged;
            filter.PropertyChanged += Filter_PropertyChanged;
            FilterMeta.Add(filter);
        }

        foreach (var item in SMFConverter.Def.InitChannelVoice)
        {
            var filter = new FilterItem(item.Key, item.Value, SMFConverter.Def.Filter.ContainsKey(item.Key));
            filter.PropertyChanged -= Filter_PropertyChanged;
            filter.PropertyChanged += Filter_PropertyChanged;
            FilterChannelVoice.Add(filter);
        }

        foreach (var item in SMFConverter.Def.InitSysEx)
        {
            var filter = new FilterItem(item.Key, item.Value, SMFConverter.Def.Filter.ContainsKey(item.Key));
            filter.PropertyChanged -= Filter_PropertyChanged;
            filter.PropertyChanged += Filter_PropertyChanged;
            FilterSysEx.Add(filter);
        }

        foreach (var item in SMFConverter.Def.InitControlChange)
        {
            var filter = new FilterItem(item.Key, item.Value, SMFConverter.Def.Filter.ContainsKey(item.Key));
            filter.PropertyChanged -= Filter_PropertyChanged;
            filter.PropertyChanged += Filter_PropertyChanged;
            FilterControlChange.Add(filter);
        }
    }

    #endregion

    #region Methods

    #region Property Change Handler

    private void Filter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        SMFConverter.Def.Filter.Clear();

        var allFilters =
            FilterMeta
            .Union(FilterChannelVoice)
            .Union(FilterSysEx)
            .Union(FilterControlChange);

        foreach (var item in allFilters)
        {
            if (item.IsChecked)
            {
                SMFConverter.Def.Filter.Add(item.Key, item.Value);
            }
        }
    }

    #endregion

    #region General

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            var allFilters =
                FilterMeta
                .Union(FilterChannelVoice)
                .Union(FilterSysEx)
                .Union(FilterControlChange);

            foreach (var item in allFilters)
            {
                item.PropertyChanged -= Filter_PropertyChanged;
            }
        }
    }

    #endregion

    #endregion
}