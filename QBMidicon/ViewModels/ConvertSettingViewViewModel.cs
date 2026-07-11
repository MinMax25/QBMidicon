using CommunityToolkit.Mvvm.ComponentModel;
using libMidi.SMF;
using libMidi.SMF.enums;
using libQB.Attributes;
using QBMidicon.Class;
using QBMidicon.Class.Services;
using QBMidicon.Views;

namespace QBMidicon.ViewModels;

[DIPage<ConvertSettingPage>]
public partial class ConvertSettingPageViewModel
    : ViewModelBase
{
    #region Properties

    [ObservableProperty]
    private bool channelFix;

    [ObservableProperty]
    private bool insertTrackName;

    [ObservableProperty]
    private bool removeProgramChange;

    [ObservableProperty]
    private bool replaceNoteOn;

    [ObservableProperty]
    private bool createCodeTrack;

    [ObservableProperty]
    private bool lyricAdjustment;

    [ObservableProperty]
    private bool lyricPaddingPlus;

    [ObservableProperty]
    private int sRTOffset;

    [ObservableProperty]
    private bool sRTRemoveComment;

    [ObservableProperty]
    private bool xFStyleConvert;

    [ObservableProperty]
    private SMFEncode encode;

    #endregion

    #region Fields

    MidiData MidiData;

    #endregion


    #region ctor

    public ConvertSettingPageViewModel(IDIContainer dIContainer, MidiData midiData)
            : base(dIContainer)
    {
        MidiData = midiData;

        ChannelFix = SMFConverter.Def.Setting.ChannelFix;
        InsertTrackName = SMFConverter.Def.Setting.InsertTrackName;
        RemoveProgramChange = SMFConverter.Def.Setting.RemoveProgramChange;
        ReplaceNoteOn = SMFConverter.Def.Setting.ReplaceNoteOn;
        CreateCodeTrack = SMFConverter.Def.Setting.CreateCodeTrack;
        LyricAdjustment = SMFConverter.Def.Setting.LyricAdjustment;
        LyricPaddingPlus = SMFConverter.Def.Setting.LyricPaddingPlus;
        SRTOffset = SMFConverter.Def.Setting.SRTOffset;
        SRTRemoveComment = SMFConverter.Def.Setting.SRTRemoveComment;
        XFStyleConvert = SMFConverter.Def.Setting.XFStyleConvert;
        Encode = SMFConverter.Def.Setting.Encode;
    }

    #endregion

    #region Methods

    partial void OnChannelFixChanged(bool oldValue, bool newValue)
    {
        SMFConverter.Def.Setting.ChannelFix = newValue;
    }

    partial void OnInsertTrackNameChanged(bool oldValue, bool newValue)
    {
        SMFConverter.Def.Setting.InsertTrackName = newValue;
    }

    partial void OnRemoveProgramChangeChanged(bool oldValue, bool newValue)
    {
        SMFConverter.Def.Setting.RemoveProgramChange = newValue;
    }

    partial void OnReplaceNoteOnChanged(bool oldValue, bool newValue)
    {
        SMFConverter.Def.Setting.ReplaceNoteOn = newValue;
    }

    partial void OnCreateCodeTrackChanged(bool oldValue, bool newValue)
    {
        SMFConverter.Def.Setting.CreateCodeTrack = newValue;
    }

    partial void OnLyricAdjustmentChanged(bool oldValue, bool newValue)
    {
        SMFConverter.Def.Setting.LyricAdjustment = newValue;
    }

    partial void OnLyricPaddingPlusChanged(bool oldValue, bool newValue)
    {
        SMFConverter.Def.Setting.LyricPaddingPlus = newValue;
    }

    partial void OnSRTOffsetChanged(int oldValue, int newValue)
    {
        SMFConverter.Def.Setting.SRTOffset = newValue;
    }

    partial void OnSRTRemoveCommentChanged(bool oldValue, bool newValue)
    {
        SMFConverter.Def.Setting.SRTRemoveComment = newValue;
    }

    partial void OnXFStyleConvertChanged(bool oldValue, bool newValue)
    {
        SMFConverter.Def.Setting.XFStyleConvert = newValue;
    }

    partial void OnEncodeChanged(SMFEncode oldValue, SMFEncode newValue)
    {
        SMFConverter.Def.Setting.Encode = newValue;
    }

    #endregion
}
