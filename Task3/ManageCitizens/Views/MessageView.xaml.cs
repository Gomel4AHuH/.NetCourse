using MahApps.Metro.Controls;

namespace ManageCitizens.Views
{
    /// <summary>
    /// Interaction logic for MessageView.xaml
    /// </summary>
    public partial class MessageView : MetroWindow
    {
        public MessageView(string message)
        {
            InitializeComponent();
            Message.Text = message;
        }
    }
}
