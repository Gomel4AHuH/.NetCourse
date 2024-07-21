using MahApps.Metro.Controls;
using ManageCitizens.Models;
using ManageCitizens.Models.Data;
using ManageCitizens.Services;
using ManageCitizens.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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
        private string _progressBarText;
        public string ProgressBarText
        {
            get => _progressBarText;
            set
            {
                _progressBarText = value;
                OnPropertyChanged(nameof(ProgressBarText));
            }
        }
        private double _progressBarValue;
        public double ProgressBarValue
        {
            get => _progressBarValue;
            set
            {
                _progressBarValue = value;
                OnPropertyChanged(nameof(ProgressBarValue));
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<int>(value =>
            {
                ProgressBarValue = value;
                ProgressBarText = $"{value}%";
                TBProgress.Text = $"Data loading...{value}%";
                ProgressBar.Value = value;
            });
            ApplicationDbContext applicationDbContext = new();
            
            //await Task.Run(() => TestMethod(applicationDbContext.Citizens.Count(), progress));
            await TestMethod(applicationDbContext.Citizens, progress);

            TBProgress.Text = "Done";
            ProgressBarText = "Done1";
        }

        public async Task TestMethod(DbSet<Citizen> citizens, IProgress<int> progress)
        {
            var count = await citizens.CountAsync();
            for (int i = 1; i <= count; i++)
            {
                Citizen citizen = await citizens.ElementAtAsync(i);
                int progressPerc = i * 100 / count;
                progress.Report(progressPerc);
            }
            TBProgress.Text = "Done3";
            ProgressBarText = "Done4";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
