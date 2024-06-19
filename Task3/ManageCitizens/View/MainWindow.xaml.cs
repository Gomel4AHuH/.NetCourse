using ManageCitizens.ViewModel;
using System.Windows;

namespace ManageCitizens.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new DataManage();
        }
    }
}
