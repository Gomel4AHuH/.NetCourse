using System.Windows;

namespace ManageCitizens.Views
{
    public partial class ErrorMessageWindow : Window
    {
        public ErrorMessageWindow(string message)
        {
            InitializeComponent();
            ErrorMessage.Text = message;
        }
    }
}
