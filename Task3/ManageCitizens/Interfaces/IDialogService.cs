﻿namespace ManageCitizens.Interfaces
{
    public interface IDialogService
    {
        string FilePath { get; set; }
        bool OpenFileDialog(string fileExtention);
        bool SaveFileDialog(string fileExtention);
        void ShowMessage(string message);
    }
}
