using ManageCitizens.Models;
using System.Windows;

namespace ManageCitizens.Views
{
    /// <summary>
    /// Interaction logic for EditCitizen.xaml
    /// </summary>
    public partial class EditCitizenWindow : Window
    {
        public Citizen Citizen { get; private set; }
        public EditCitizenWindow(Citizen citizen)
        {
            InitializeComponent();
            Citizen = citizen;
            DataContext = citizen;
        }
        void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
