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
        private readonly ApplicationVM _applicationVW;
        public MainWindow()
        {
            InitializeComponent();
            _applicationVW = new(new ApplicationDbContext(), new DefaultDialogService(), new JsonFileService(), new CsvFileService(), new XmlFileService(), new ExcelFileService());
            DataContext = _applicationVW;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {            
            try
            {
                await _applicationVW.ImportDataFromDbAsync();
            }
            catch (Exception ex)
            {
                new DefaultDialogService().ShowMessage(ex.Message);
            }
        }
    }
}
