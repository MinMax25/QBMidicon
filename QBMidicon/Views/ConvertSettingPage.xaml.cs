using System.Windows.Controls;

using QBMidicon.ViewModels;

namespace QBMidicon.Views;

public partial class ConvertSettingPage
    : Page
{
    public ConvertSettingPage(ConvertSettingPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
