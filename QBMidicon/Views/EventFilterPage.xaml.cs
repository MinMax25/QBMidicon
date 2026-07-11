using System.Windows.Controls;

using QBMidicon.ViewModels;

namespace QBMidicon.Views;

public partial class EventFilterPage : Page
{
    public EventFilterPage(EventFilterViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
