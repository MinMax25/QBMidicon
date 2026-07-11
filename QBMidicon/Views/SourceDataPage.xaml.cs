using System.Windows.Controls;

using QBMidicon.ViewModels;

namespace QBMidicon.Views;

public partial class SourceDataPage : Page
{
    public SourceDataPage(SourceDataViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
