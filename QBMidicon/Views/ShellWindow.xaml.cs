using System.Windows.Controls;
using libQB.WindowServices;
using MahApps.Metro.Controls;
using QBMidicon.Contracts.Views;
using QBMidicon.ViewModels;

namespace QBMidicon.Views
{
    public partial class ShellWindow
        : MetroWindow
        , IShellWindow
    {
        public ShellWindow(ShellViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Closing += OnClosing;
        }

        private async void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is IWindowClosingAware vm)
            {
                e.Cancel = true;

                bool shouldClose = await vm.OnWindowClosingAsync();
                if (shouldClose)
                {
                    Closing -= OnClosing;
                    Close();
                }
            }
        }

        public Frame GetNavigationFrame() => shellFrame;

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();
    }
}
