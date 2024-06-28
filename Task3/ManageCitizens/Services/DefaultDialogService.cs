using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Views;
using Microsoft.Win32;
using System.Windows;

namespace ManageCitizens.Services
{
    class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }

        public bool OpenFileDialog(string fileExtention)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = fileExtention
            };
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        public bool SaveFileDialog(string fileExtention)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = fileExtention
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        public void ShowMessage(string message)
        {
            MessageView msgVw = new(message)
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            msgVw.ShowDialog();
        }
    }
}
