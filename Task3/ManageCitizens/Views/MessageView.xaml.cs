using System.Windows;

namespace ManageCitizens.Views
{
    /// <summary>
    /// Interaction logic for MessageView.xaml
    /// </summary>
    public partial class MessageView : Window
    {
        public MessageView(string message)
        {
            InitializeComponent();
            Message.Text = message;
        }
    }
}
