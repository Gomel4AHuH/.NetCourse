using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Models.Data;
using ManageCitizens.Repository;
using ManageCitizens.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ManageCitizens.ViewModels
{
    class ApplicationVM : INotifyPropertyChanged
    {
        #region VIEW MODEL
        private ObservableCollection<Citizen> Citizens { get; set; } = [];
        private ObservableCollection<Citizen> AllCitizens { get; set; } = [];
        public IEnumerable<Citizen> CitizensCollection => Citizens;
        public IEnumerable<Citizen> AllCitizensCollection => AllCitizens;

        public ApplicationVM(ApplicationDbContext applicationDbContext, IDialogService dialogService, JsonFileService jsonFileService, CsvFileService csvFileService, XmlFileService xmlFileService, ExcelFileService excelFileService)
        {
            _dialogService = dialogService;
            _jsonFileService = jsonFileService;
            _csvFileService = csvFileService;
            _xmlFileService = xmlFileService;
            _excelFileService = excelFileService;
            _applicationDbContext = applicationDbContext;

            _citizensRepository = new SQLCitizenRepository(_applicationDbContext);
            ImportDataFromCsvFileAsyncCommand = new RelayCommand(async _ => await ImportDataFromCsvFileAsync());
            ExportDataFromCsvFileAsyncCommand = new RelayCommand(async _ => await ExportDataFromCsvFileAsync());
            ImportDataFromXmlFileAsyncCommand = new RelayCommand(async _ => await ImportDataFromXmlFileAsync());
            /*ExportDataFromXmlFileAsyncCommand = new RelayCommand(async _ => await ExportDataFromXmlFileAsync());
            ImportDataFromJsonFileAsyncCommand = new RelayCommand(async _ => await ImportDataFromJsonFileAsync());
            ExportDataFromJsonFileAsyncCommand = new RelayCommand(async _ => await ExportDataFromJsonFileAsync());
            ImportDataFromExcelFileAsyncCommand = new RelayCommand(async _ => await ImportDataFromExcelFileAsync());
            ExportDataFromExcelFileAsyncCommand = new RelayCommand(async _ => await ExportDataFromExcelFileAsync());*/

            _ = ImportDataFromDbAsync();
        }
        #endregion

        #region IMPORT/EXPORT DATA COMMAND
        private readonly IDialogService _dialogService;
        private static readonly Dictionary<string, string> s_fileExtention = new()
            {
                { "json", "JSON Files | *.json"},
                { "csv", "CSV Files | *.csv"},
                { "xml", "XML Files | *.xml"},
                { "excel", "Excel Files | *.xlsx"}
            };

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
                        if (_dialogService.SaveFileDialog(s_fileExtention["json"]))
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
                        if (_dialogService.OpenFileDialog(s_fileExtention["json"]))
                        {
                            List<Citizen> citizens = _jsonFileService.Open(_dialogService.FilePath);
                            Citizens.Clear();
                            foreach (Citizen ctzn in citizens)
                            {
                                ctzn.Id = 0;
                                _citizensRepository.Insert(ctzn);
                            }
                            
                            _citizensRepository.Save();
                            //_ = ImportDataFromDbAsync();
                            _dialogService.ShowMessage("Data imported.");
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
        public RelayCommand ExportDataFromCsvFileAsyncCommand { get; }

        private async Task ExportDataFromCsvFileAsync()
        {
            try
            {
                if (_dialogService.SaveFileDialog(s_fileExtention["csv"]))
                {
                    await _csvFileService.ExportDataAsync([.. Citizens], _dialogService, _dialogService.FilePath);
                }
                _dialogService.ShowMessage("Data exported.");
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }

        public RelayCommand ImportDataFromCsvFileAsyncCommand { get; }
        private async Task ImportDataFromCsvFileAsync()
        {
            try
            {
                if (_dialogService.OpenFileDialog(s_fileExtention["csv"]))
                {
                    await _csvFileService.ImportDataAsync(_citizensRepository, _dialogService, _dialogService.FilePath);
                }
                _ = ImportDataFromDbAsync();
                _dialogService.ShowMessage("Data imported.");
            }
            catch (Exception ex)
            {
                    _dialogService.ShowMessage(ex.Message);
            }
        }        
        #endregion

        #region XML
        private readonly XmlFileService _xmlFileService;
        public readonly RelayCommand _exportDataToXmlFileCommand;        

        private RelayCommand ExportDataToXmlFileCommand
        {
            get
            {
                return _exportDataToXmlFileCommand ?? new RelayCommand(obj =>
                {
                    try
                    {
                        if (_dialogService.SaveFileDialog(s_fileExtention["xml"]))
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
        
        private RelayCommand ImportDataFromXmlFileAsyncCommand { get; }

        public async Task ImportDataFromXmlFileAsync()
        {
            try
            {
                if (_dialogService.OpenFileDialog(s_fileExtention["xml"]))
                    {
                        await _xmlFileService.ImportDataAsync(_citizensRepository, _dialogService, _dialogService.FilePath);
                        _ = ImportDataFromDbAsync();
                        _dialogService.ShowMessage("Data imported.");
                    }
            }
            catch (Exception ex)
            {
                    _dialogService.ShowMessage(ex.Message);
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
                        if (_dialogService.SaveFileDialog(s_fileExtention["excel"]))
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
                        if (_dialogService.OpenFileDialog(s_fileExtention["excel"]))
                        {
                            List<Citizen> citizens = _excelFileService.Open(_dialogService.FilePath);
                            Citizens.Clear();
                            foreach (Citizen ctzn in citizens)
                                _citizensRepository.Insert(ctzn);
                            _citizensRepository.Save();
                            //Task task = ImportDataFromDbAsync();
                            _dialogService.ShowMessage("Data imported.");
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

        #region PROGRESSBAR

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

        private int _progressBarMax;
        public int ProgressBarMax
        {
            get => _progressBarMax;
            set
            {
                _progressBarMax = value;
                OnPropertyChanged(nameof(ProgressBarMax));
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
        private readonly RelayCommand _testCommand;

        private async Task Test1()
        {
            ProgressBarMax = _applicationDbContext.Citizens.Count();
            ProgressBarValue = 0;
            for (int i = 0; i < ProgressBarMax; i++)
            {
                ProgressBarText = $"Loading... {i} out of {ProgressBarMax} counts";
                ProgressBarValue++;
                await Task.Delay(10);

            }
            ProgressBarMax = 0;
            ProgressBarValue = 0;
            ProgressBarText = "";
        }
        public RelayCommand TestCommand
        {
            get
            {
                return _testCommand ?? new RelayCommand(obj =>
                {
                    try
                    {
                        MessageBox.Show(_citizensRepository.GetCitizens().Count().ToString());
                        //Test1();
                        /*object pb = Application.Current.MainWindow.FindName("ProgressBar");
                        if (pb is ProgressBar)
                        {
                            // Following executed if Text element was found.
                            ProgressBar? pb1 = pb as ProgressBar;
                            pb1.Value = 20;
                            pb1.Width = 100;
                            //pb1.Text = "123";
                            pb1.Height = 100;
                        }*/

                        /*ProgressBarMax = 500;
                        ProgressBarValue = 0;
                        ProgressBarText = "Testing...";
                        for (int i = 0; i < 100; i++)
                        {
                            ProgressBarValue++;
                            Task.Delay(500);

                        }
                        ProgressBarMax = 0;
                        ProgressBarValue = 0;
                        ProgressBarText = "";*/
                        /*progress = new(value =>
                        {
                            ProgressBarValue = value;
                            ProgressBarText = $"{value}%";
                        });

                        await Task.Run(() => ImportDataFromDb(progress));
                        ImportDataFromDbAsync();*/
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }        
        #endregion

        #region DATABASE

        private readonly ApplicationDbContext _applicationDbContext;

        private readonly SQLCitizenRepository _citizensRepository;
                
        private async Task ImportDataFromDbAsync()
        {
            try
            {
                /*ProgressBarText = $"Database data loading...{_applicationDbContext.Citizens.Count()}";
                ProgressBarMax = _applicationDbContext.Citizens.Count();
                ProgressBarValue = 0;*/
                //MessageBox.Show("Test");
                
                IEnumerable<Citizen> citizensList = await _citizensRepository.GetCitizensAsync();
                TotalCount = citizensList.Count();
                //ProgressBarValue++;
                foreach (Citizen citizen in citizensList)
                {
                    Citizens.Add(citizen);
                    /*_ = Task.Run(() => Citizens.Add(citizen));
                    ProgressBarValue++;*/
                /*await Application.Current.Dispatcher.BeginInvoke((Action)delegate () 
                {
                    Citizens.Add(citizen);
                    ProgressBarValue++;
                });*/
                }
                /*for (int i = 0; i < citizensList.Count(); i++)
                {
                    Citizens.Add(citizensList.ElementAt(i));
                    ProgressBarValue++;
                }*/

                AllCitizens = [.. Citizens];

                /*ProgressBarText = "";
                ProgressBarValue = 0;*/
                if (TotalCount != 0) _dialogService.ShowMessage("Data loaded.");
                //_dialogService.Close();
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }        
        #endregion

        #endregion

        #region FILTER CITIZENS

        #region FILTER FILEDS
        private string _firstNameFilter;
        public string FirstNameFilter
        {
            get => _firstNameFilter;
            set
            {
                _firstNameFilter = value;
                OnPropertyChanged(nameof(FirstNameFilter));
            }
        }

        private string _lastNameFilter;
        public string LastNameFilter
        {
            get => _lastNameFilter;
            set
            {
                _lastNameFilter = value;
                OnPropertyChanged(nameof(LastNameFilter));
            }
        }

        private string _middleNameFilter;
        public string MiddleNameFilter
        {
            get => _middleNameFilter;
            set
            {
                _middleNameFilter = value;
                OnPropertyChanged(nameof(MiddleNameFilter));
            }
        }

        private string _cityFilter;
        public string CityFilter
        {
            get => _cityFilter;
            set
            {
                _cityFilter = value;
                OnPropertyChanged(nameof(CityFilter));
            }
        }

        private string _countryFilter;
        public string CountryFilter
        {
            get => _countryFilter;
            set
            {
                _countryFilter = value;
                OnPropertyChanged(nameof(CountryFilter));
            }
        }

        private DateOnly _birthdayFilter;
        public DateOnly BirthdayFilter
        {
            get => _birthdayFilter;
            set
            {
                _birthdayFilter = value;
                OnPropertyChanged(nameof(BirthdayFilter));
            }
        }
        #endregion

        private bool _isDataFiltered;
        public bool IsDataFiltered
        {
            get => _isDataFiltered;
            set
            {
                _isDataFiltered = value;
                OnPropertyChanged(nameof(IsDataFiltered));
            }
        }

        private bool _cancelSearchButtonLock;
        public bool CancelSearchButtonLock
        {
            get => _cancelSearchButtonLock;
            set
            {
                _cancelSearchButtonLock = value;
                OnPropertyChanged(nameof(CancelSearchButtonLock));
            }
        }

        private readonly RelayCommand _filterCitizensCommand;

        public RelayCommand FilterCitizensCommand
        {
            get
            {
                return _filterCitizensCommand ?? new RelayCommand(obj =>
                {
                    if (string.IsNullOrEmpty(_lastNameFilter)
                    && string.IsNullOrEmpty(_firstNameFilter)
                    && string.IsNullOrEmpty(_middleNameFilter)
                    && string.IsNullOrEmpty(_birthdayFilter.ToString())
                    && string.IsNullOrEmpty(_cityFilter)
                    && string.IsNullOrEmpty(_countryFilter))
                    {
                        _dialogService.ShowMessage("Search criteria should't be empty.");
                    }
                    else
                    {
                        FilterService FilterService = new(_firstNameFilter, _lastNameFilter, _middleNameFilter, _birthdayFilter, _cityFilter, _countryFilter);
                        IEnumerable<Citizen> FilteredCol = FilterService.CitizenSearch([.. AllCitizens]);
                        Citizens.Clear();

                        foreach (Citizen citizen in FilteredCol)
                        {
                            Citizens.Add(citizen);
                        }
                        IsDataFiltered = true;
                        FilterCount = Citizens.Count;
                    }
                });
            }
        }

        private readonly RelayCommand _cancelSearchCitizensCommand;

        public RelayCommand CancelSearchCitizensCommand
        {
            get
            {
                return _cancelSearchCitizensCommand ?? new RelayCommand(obj =>
                {
                    Citizens.Clear();
                    foreach (Citizen citizen in AllCitizens)
                    {
                        Citizens.Add(citizen);
                    }
                    FirstNameFilter = "";
                    LastNameFilter = "";
                    MiddleNameFilter = "";
                    BirthdayFilter = new DateOnly(1, 1, 1);
                    CityFilter = "";
                    CountryFilter = "";
                    IsDataFiltered = false;
                    FilterCount = 0;
                });
            }
        }
        #endregion

        /*
        #region LOCKING BUTTONS
        private bool _cleanButtonLock = true;
        public bool CleanButtonLock
        {
            get => _cleanButtonLock;
            set
            {
                _cleanButtonLock = value;
                OnPropertyChanged(nameof(CleanButtonLock));
            }
        }

        private bool _importButtonLock = true;
        public bool ImportButtonLock
        {
            get => _importButtonLock;
            set
            {
                _importButtonLock = value;
                OnPropertyChanged(nameof(ImportButtonLock));
            }
        }

        private bool _exportButtonLock = true;
        public bool ExportButtonLock
        {
            get => _exportButtonLock;
            set
            {
                _exportButtonLock = value;
                OnPropertyChanged(nameof(ExportButtonLock));
            }
        }

        private bool _searchButtonLock;
        public bool SearchButtonLock
        {
            get => _searchButtonLock;
            set
            {
                _searchButtonLock = value;
                OnPropertyChanged(nameof(SearchButtonLock));
            }
        }

        private void UpdateLockStatus(bool status)
        {
            status = !status;
        }
        #endregion
        */

        private int _totalCount;
        public int TotalCount
        {
            get => _totalCount;
            set
            {
                _totalCount = value;
                OnPropertyChanged(nameof(TotalCount));
            }
        }

        private int _filterCount;
        public int FilterCount
        {
            get => _filterCount;
            set
            {
                _filterCount = value;
                OnPropertyChanged(nameof(FilterCount));
            }
        }
        
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

        private readonly RelayCommand _cleanDatabaseCommand;

        public RelayCommand CleanDatabaseCommand
        {
            get
            {
                return _cleanDatabaseCommand ?? new RelayCommand(obj =>
                {
                    //_citizensRepository.DeleteAll();
                    _ = _citizensRepository.DeleteAllAsync();
                    _ = _citizensRepository.SaveChangesAsync();
                    //_citizensRepository.Save();
                    Citizens.Clear();
                    AllCitizens.Clear();
                    TotalCount = 0;
                    _dialogService.ShowMessage("All data deleted.");
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }       
    }
}
