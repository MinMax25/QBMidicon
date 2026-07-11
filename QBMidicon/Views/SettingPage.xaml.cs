using System.Windows.Controls;
using QBMidicon.ViewModels;

namespace QBMidicon.Views
{
    public partial class SettingPage
        : Page
    {
        public SettingPage(SettingViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
