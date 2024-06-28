using ManageCitizens.Services;
using ManageCitizens.ViewModels;
using System.Windows;

namespace ManageCitizens.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationVM(new DefaultDialogService(), new JsonFileService(), new CsvFileService(), new XmlFileService(), new ExcelFileService());
        }
    }
}
