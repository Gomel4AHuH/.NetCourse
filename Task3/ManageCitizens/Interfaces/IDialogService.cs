namespace ManageCitizens.Interfaces
{
    interface IDialogService
    {
        void ShowMessage(string message);
        string FilePath { get; set; }
        bool OpenFileDialog(string fileExtention);
        bool SaveFileDialog(string fileExtention);
    }
}
