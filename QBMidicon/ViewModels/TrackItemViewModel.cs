using CommunityToolkit.Mvvm.ComponentModel;
using libMidi.Messages;
using libMidi.SMF.interfaces;
using QBMidicon.Class.Enums;

namespace QBMidicon.ViewModels;

public partial class TrackItemViewModel : ObservableObject
{
    #region Fields

    [ObservableProperty]
    private bool _filterEnabled = false;

    [ObservableProperty]
    private bool _lyricMatched;

    [ObservableProperty]
    private byte? _minPitch;

    [ObservableProperty]
    private byte? _maxPitch;

    [ObservableProperty]
    private OctaveTranspose _transpose = OctaveTranspose.Zero;

    #endregion

    #region Properties

    public ITrack Track { get; }

    public int TrackNumber
    {
        get
        {
            return Track.TrackNumber;
        }
    }

    public string TrackName
    {
        get
        {
            return Track.ToString();
        }
    }

    public byte Channel
    {
        get
        {
            return Track.Channel;
        }
    }

    public bool IsDrum
    {
        get
        {
            return Track.IsDrum;
        }
    }

    public bool CanTranspose
    {
        get
        {
            return !Track.IsDrum && (MinPitch != null || MaxPitch != null) && TrackName != "#Code Track#";
        }
    }

    public double LyricMatchRatio
    {
        get
        {
            return Track.LyricMatchRatio;
        }
    }

    public int EventCount
    {
        get
        {
            return Track.Events.Count();
        }
    }

    #endregion

    #region ctor

    public TrackItemViewModel(ITrack track)
    {
        Track = track;
        FilterEnabled = track.FilterEnabled;
        LyricMatched = track.LyricMatched;
        Transpose = (OctaveTranspose)(track.Transpose / 12);

        CalculatePitchRange();
    }

    #endregion

    #region Methods

    #region Property Change Handler

    partial void OnLyricMatchedChanged(bool value)
    {
        Track.LyricMatched = value;
    }

    partial void OnFilterEnabledChanged(bool value)
    {
        Track.FilterEnabled = value;
    }

    partial void OnTransposeChanged(OctaveTranspose value)
    {
        Track.Transpose = (int)value;
    }

    #endregion

    #region General

    private void CalculatePitchRange()
    {
        var notes = Track.Events
                         .Where(x => x.Message is NoteOn)
                         .Select(x => ((NoteOn)x.Message).Pitch)
                         .ToList();

        if (notes.Count == 0)
        {
            MinPitch = null;
            MaxPitch = null;
            return;
        }

        MinPitch = notes.Min();
        MaxPitch = notes.Max();
    }

    #endregion

    #endregion
}