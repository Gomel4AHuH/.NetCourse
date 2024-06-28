using ManageCitizens.Models;
using ManageCitizens.Services;
using ManageCitizens.ViewModels;
using System.Windows;

namespace ManageCitizens.Views
{
    /// <summary>
    /// Interaction logic for CreateCitizenWindow.xaml
    /// </summary>
    public partial class CreateCitizenWindow : Window
    {
        public Citizen Citizen { get; private set; }
        public CreateCitizenWindow(Citizen citizen)
        {
            InitializeComponent();
            Citizen = citizen;
            DataContext = citizen;
        }
    }
}
