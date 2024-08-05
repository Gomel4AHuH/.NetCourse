using ManageCitizens.Interfaces;
using ManageCitizens.Views;
using Microsoft.Win32;

namespace ManageCitizens.Services
{
    public class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }
        private MessageView? msgVw;

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
            msgVw = new(message);            
            msgVw.ShowDialog();
        }
    }
}
