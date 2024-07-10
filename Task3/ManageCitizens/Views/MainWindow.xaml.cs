using MahApps.Metro.Controls;
using ManageCitizens.Models.Data;
using ManageCitizens.Services;
using ManageCitizens.ViewModels;

namespace ManageCitizens.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationVM(new ApplicationDbContext(), new DefaultDialogService(), new JsonFileService(), new CsvFileService(), new XmlFileService(), new ExcelFileService());            
        }
    }
}
