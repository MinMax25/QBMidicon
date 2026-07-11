using CommunityToolkit.Mvvm.ComponentModel;
using QBMidicon.Class;

namespace QBMidicon.ViewModels;

public partial class FilterItem
    : ViewModelBase
{
    public FilterItem(string key, object value, bool isChecked)
    {
        Key = key;
        Value = value;
        IsChecked = isChecked;
    }

    [ObservableProperty]
    private string key;

    [ObservableProperty]
    private object value;

    [ObservableProperty]
    private bool isChecked;
}
