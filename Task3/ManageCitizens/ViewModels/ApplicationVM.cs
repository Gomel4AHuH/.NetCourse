using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Services;
using ManageCitizens.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ManageCitizens.ViewModels
{
    class ApplicationVM : INotifyPropertyChanged
    {
        #region VIEW MODEL
        private Citizen _selectedCitizen;
        public ObservableCollection<Citizen> Citizens { get; set; }

        public Citizen SelectedCitizen
        {
            get { return _selectedCitizen; }
            set
            {
                _selectedCitizen = value;
                OnPropertyChanged("SelectedCitizen");
            }
        }

        public ApplicationVM(IDialogService dialogService, JsonFileService jsonFileService, CsvFileService csvFileService, XmlFileService xmlFileService, ExcelFileService excelFileService)
        {
            this._dialogService = dialogService;
            this._jsonFileService = jsonFileService;
            this._csvFileService = csvFileService;
            this._xmlFileService = xmlFileService;
            this._excelFileService = excelFileService;

            Citizens =
            [
                /*new("Ivan", "Ivanov", "QQQ", new DateTime(2024, 01, 01), "Gomel1", "Belarus"),
                new("Petr", "Petrov", "QQQ", new DateTime(2016, 07, 12), "Gomel2", "Poland"),
                new("Sidor", "Sidorov", "QQQ", new DateTime(2009, 03, 24), "Gomel3", "Belarus")*/
            ];
        }
        #endregion

        #region WORK WITH CITIZENS COMMANDS
        private readonly RelayCommand _createCitizenCommand;
        private readonly RelayCommand _deleteCitizenCommand;
        private readonly RelayCommand _copyPasteCitizenCommand;
        private readonly RelayCommand _openCreateCitizenWindowCommand;

        public RelayCommand OpenCreateCitizenWindowCommand
        {
            get
            {
                return _openCreateCitizenWindowCommand ?? new RelayCommand(obj =>
                {
                    OpenCreateCitizenWindow();
                });
            }
        }

        private void OpenCreateCitizenWindow()
        {
            CreateCitizenWindow wnd = new(new Citizen())
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            wnd.ShowDialog();
        }

        public RelayCommand CreateCitizenCommand
        {
            get
            {
                return _createCitizenCommand ?? new RelayCommand(obj =>
                {
                    /*Citizen citizen = new();
                    Citizens.Insert(Citizens.Count, citizen);
                    SelectedCitizen = citizen;*/
                    //DataWorker.CreateCitizen("Andrei", "Patskou", "QQQ", new DateTime(), "Gomel", "Belarus");
                    MessageBox.Show("Test1");
                    Citizen citizen = obj as Citizen;
                    MessageBox.Show(citizen.LastName);
                    Citizens.Insert(Citizens.Count, citizen);
                    SelectedCitizen = citizen;
                    MessageBox.Show("Test2");
                });
            }
        }

        public RelayCommand DeleteCitizenCommand
        {
            get
            {
                return _deleteCitizenCommand ?? new RelayCommand(obj =>
                {
                    if (obj is Citizen citizen)
                    {
                        Citizens.Remove(citizen);
                    }
                },
                (obj) => Citizens.Count > 0);
            }
        }

        public RelayCommand CopyPasteCitizenCommand
        {
            get
            {
                return _copyPasteCitizenCommand ?? new RelayCommand(obj =>
                {
                    if (obj is Citizen citizen)
                    {
                        Citizen newCitizen = new(citizen.LastName, citizen.FirstName, citizen.MiddleName, citizen.Birthday, citizen.City, citizen.Country);
                        Citizens.Insert(Citizens.Count, newCitizen);
                        SelectedCitizen = newCitizen;
                    }
                },
                (obj) => Citizens.Count > 0);
            }
        }
        #endregion

        #region IMPORT/EXPORT DATA COMMAND
        private readonly IDialogService _dialogService;

        #region JSON
        private readonly JsonFileService _jsonFileService;
        private readonly RelayCommand _exportDataToJsonFileCommand;

        public RelayCommand ExportDataToJsonFileCommand
        {
            get
            {
                return _exportDataToJsonFileCommand ?? new RelayCommand(obj =>
                {
                    try
                    {
                        if (_dialogService.SaveFileDialog("JSON Files | *.json") == true)
                        {
                            _jsonFileService.Save(_dialogService.FilePath, [.. Citizens]);
                            _dialogService.ShowMessage("Data exported");
                        }
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }

        private readonly RelayCommand _importDataFromJsonFileCommand;
        public RelayCommand ImportDataFromJsonFileCommand
        {
            get
            {
                return _importDataFromJsonFileCommand ?? new RelayCommand(obj =>
                {
                    try
                    {
                        if (_dialogService.OpenFileDialog("JSON Files| *.json") == true)
                        {
                            List<Citizen> citizens = _jsonFileService.Open(_dialogService.FilePath);
                            Citizens.Clear();
                            foreach (Citizen ctzn in citizens)
                                Citizens.Add(ctzn);
                            _dialogService.ShowMessage("Data imported");
                        }
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }
        #endregion

        #region CSV
        private readonly CsvFileService _csvFileService;
        private readonly RelayCommand _exportDataToCsvFileCommand;

        public RelayCommand ExportDataToCsvFileCommand
        {
            get
            {
                return _exportDataToCsvFileCommand ?? new RelayCommand(obj =>
                {
                    try
                    {                        
                        if (_dialogService.SaveFileDialog("CSV Files | *.csv") == true)
                        {
                            _csvFileService.Save(_dialogService.FilePath, [.. Citizens]);
                            _dialogService.ShowMessage("Data exported");
                        }
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }

        private readonly RelayCommand _importDataFromCsvFileCommand;
        public RelayCommand ImportDataFromCsvFileCommand
        {
            get
            {
                return _importDataFromCsvFileCommand ?? new RelayCommand(obj =>
                {
                    try
                    {
                        if (_dialogService.OpenFileDialog("CSV Files | *.csv") == true)
                        {
                            List<Citizen> citizens = _csvFileService.Open(_dialogService.FilePath);
                            Citizens.Clear();
                            foreach (Citizen ctzn in citizens)
                                Citizens.Add(ctzn);
                            _dialogService.ShowMessage("Data imported");
                        }
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }
        #endregion

        #region XML
        private readonly XmlFileService _xmlFileService;
        private readonly RelayCommand _exportDataToXmlFileCommand;        

        public RelayCommand ExportDataToXmlFileCommand
        {
            get
            {
                return _exportDataToXmlFileCommand ?? new RelayCommand(obj =>
                {
                    try
                    {
                        if (_dialogService.SaveFileDialog("XML Files | *.xml") == true)
                        {
                            _xmlFileService.Save(_dialogService.FilePath, [.. Citizens]);
                            _dialogService.ShowMessage("Data exported");
                        }
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }

        private readonly RelayCommand _importDataFromXmlFileCommand;

        public RelayCommand ImportDataFromXmlFileCommand
        {
            get
            {
                return _importDataFromXmlFileCommand ?? new RelayCommand(obj =>
                {
                    try
                    {
                        if (_dialogService.OpenFileDialog("XML Files | *.xml") == true)
                        {
                            List<Citizen> citizens = _xmlFileService.Open(_dialogService.FilePath);
                            Citizens.Clear();
                            foreach (Citizen ctzn in citizens)
                                Citizens.Add(ctzn);
                            _dialogService.ShowMessage("Data imported");
                        }
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }
        #endregion

        #region EXCEL
        private readonly ExcelFileService _excelFileService;
        private readonly RelayCommand _exportDataToExcelFileCommand;

        public RelayCommand ExportDataToExcelFileCommand
        {
            get
            {
                return _exportDataToExcelFileCommand ?? new RelayCommand(obj =>
                {
                    try
                    {
                        if (_dialogService.SaveFileDialog("Excel Files | *.xlsx") == true)
                        {
                            _excelFileService.Save(_dialogService.FilePath, [.. Citizens]);
                            _dialogService.ShowMessage("Data exported");
                        }
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }

        private readonly RelayCommand _importDataFromExcelFileCommand;

        public RelayCommand ImportDataFromExcelFileCommand
        {
            get
            {
                return _importDataFromExcelFileCommand ?? new RelayCommand(obj =>
                {
                    try
                    {
                        if (_dialogService.OpenFileDialog("Excel Files | *.xlsx") == true)
                        {
                            List<Citizen> citizens = _excelFileService.Open(_dialogService.FilePath);
                            Citizens.Clear();
                            foreach (Citizen ctzn in citizens)
                                Citizens.Add(ctzn);
                            _dialogService.ShowMessage("Data imported");
                        }
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }
        #endregion

        #endregion

        private readonly RelayCommand _exitApplicationCommand;

        public RelayCommand ExitApplicationCommand
        {
            get
            {
                return _exitApplicationCommand ?? new RelayCommand(obj =>
                {
                    Application.Current.Shutdown();
                });
            }
        }
        private static void OpenMessageWindow()
        {
            ErrorMessageWindow wnd = new("Doesn't implemented yet.")
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            wnd.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }        
    }
}
