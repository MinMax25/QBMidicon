using System.Windows.Input;
using MahApps.Metro.Controls;

namespace QBMidicon.Views
{
    public partial class SavePage
        : MetroWindow
    {
        public SavePage()
        {
            InitializeComponent();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
