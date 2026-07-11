using System.Windows.Controls;

using QBMidicon.ViewModels;

namespace QBMidicon.Views;

public partial class ConvertedDataPage : Page
{
    public ConvertedDataPage(ConvertedDataViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
