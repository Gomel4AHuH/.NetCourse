using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Models.Data;
using ManageCitizens.Repository;
using ManageCitizens.Services;
using Microsoft.EntityFrameworkCore;
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

        public Progress<double> progress;

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
            ExportDataToCsvFileAsyncCommand = new RelayCommand(async _ => await ExportDataToCsvFileAsync());
            ImportDataFromXmlFileAsyncCommand = new RelayCommand(async _ => await ImportDataFromXmlFileAsync());
            ExportDataToXmlFileAsyncCommand = new RelayCommand(async _ => await ExportDataToXmlFileAsync());
            ImportDataFromJsonFileAsyncCommand = new RelayCommand(async _ => await ImportDataFromJsonFileAsync());
            ExportDataToJsonFileAsyncCommand = new RelayCommand(async _ => await ExportDataToJsonFileAsync());
            //ImportDataFromExcelFileAsyncCommand = new RelayCommand(async _ => await ImportDataFromExcelFileAsync());
            //ExportDataToExcelFileAsyncCommand = new RelayCommand(async _ => await ExportDataToExcelFileAsync());

            Language.SetLanguage("en");

            /*progress = new Progress<double>(value =>
            {
                ProgressBarValue = value * 100 / ProgressBarMax;
                //ProgressBarText = $"{value}%";
                ProgressBarText = $"Data loading... {value} out of {ProgressBarMax}";
            });*/
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
        public RelayCommand ExportDataToJsonFileAsyncCommand { get; }

        public async Task ExportDataToJsonFileAsync()
        {
            try
            {
                if (_dialogService.SaveFileDialog(s_fileExtention["json"]))
                {
                    await _jsonFileService.ExportDataAsync([.. Citizens], _dialogService, _dialogService.FilePath);
                    _dialogService.ShowMessage("Data exported.");
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }

        public RelayCommand ImportDataFromJsonFileAsyncCommand { get; }
        public async Task ImportDataFromJsonFileAsync()
        {
            try
            {
                if (_dialogService.OpenFileDialog(s_fileExtention["json"]))
                {
                    await _jsonFileService.ImportDataAsync(_citizensRepository, _dialogService, _dialogService.FilePath);
                    //_ = ImportDataFromDbAsync();
                }
            }
            catch (Exception ex)
            {
                    _dialogService.ShowMessage(ex.Message);
            }
        }       
        #endregion

        #region CSV
        private readonly CsvFileService _csvFileService;
        public RelayCommand ExportDataToCsvFileAsyncCommand { get; }

        private async Task ExportDataToCsvFileAsync()
        {
            try
            {
                if (_dialogService.SaveFileDialog(s_fileExtention["csv"]))
                {
                    await _csvFileService.ExportDataAsync([.. Citizens], _dialogService, _dialogService.FilePath);
                    _dialogService.ShowMessage("Data exported.");
                }
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
                    _ = ImportDataFromDbAsync();
                }
            }
            catch (Exception ex)
            {
                    _dialogService.ShowMessage(ex.Message);
            }
        }
        #endregion

        #region XML
        private readonly XmlFileService _xmlFileService;
        public RelayCommand ExportDataToXmlFileAsyncCommand { get; }

        private async Task ExportDataToXmlFileAsync()
        {
            try
            {
                if (_dialogService.SaveFileDialog(s_fileExtention["xml"]))
                {
                    await _xmlFileService.ExportDataAsync([.. Citizens], _dialogService, _dialogService.FilePath);
                    _dialogService.ShowMessage("Data exported.");
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }
        
        public RelayCommand ImportDataFromXmlFileAsyncCommand { get; }

        public async Task ImportDataFromXmlFileAsync()
        {
            try
            {
                if (_dialogService.OpenFileDialog(s_fileExtention["xml"]))
                    {
                        await _xmlFileService.ImportDataAsync(_citizensRepository, _dialogService, _dialogService.FilePath);
                        //_ = ImportDataFromDbAsync();
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
                            //foreach (Citizen ctzn in citizens)
                                //_citizensRepository.Insert(ctzn);
                            //_citizensRepository.Save();
                            //Task task = ImportDataFromDbAsync();
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

        private bool _progressBarVisibility;
        public bool ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set
            {
                _progressBarVisibility = value;
                OnPropertyChanged(nameof(ProgressBarVisibility));
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
        #endregion

        #region DATABASE

        private readonly ApplicationDbContext _applicationDbContext;

        private readonly SQLCitizenRepository _citizensRepository;

        //private async Task ImportDataFromDbAsync(IProgress<double> progress)
        private async Task ImportDataFromDbAsync()
        {
            try
            {
                //ProgressBarMax = await _applicationDbContext.Citizens.CountAsync();
                ProgressBarText = $"Database data loading...{_applicationDbContext.Citizens.Count()}";                

                //ProgressBarValue = 0;
                //ProgressBarVisibility = true;
                Citizens.Clear();
                
                IEnumerable<Citizen> citizensList = await _citizensRepository.GetCitizensAsync();

                await foreach (var record in _citizensRepository.GetRecordsAsync())
                {
                    Citizens.Add(record);
                }
                //for (int i = 1; i <= ProgressBarMax; i++)
                //{
                //Citizen citizen = await _applicationDbContext.Citizens.ElementAtAsync(i);

                //Citizens.Add(citizen);
                //int progressPerc = i * 100 / ProgressBarMax;
                //int progressPerc = i;
                //progress.Report(progressPerc);
                //}
                ProgressBarText = "Data loaded";
                /*Citizen[] citizens = citizensList.ToArray();
                for (int i = 1; i <= citizens.Length; i++)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProgressBarValue = i;
                    });
                }*/
                TotalCount = citizensList.Count();

                //int i = 0;
                /*foreach (Citizen citizen in citizensList)
                {
                    Citizens.Add(citizen);
                    //i++;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ProgressBarValue = i * 100 / TotalCount - 1;
                        //ProgressBarValue = Citizens.Count;
                    });
                    int progressPerc = i * 100 / TotalCount;
                    progress.Report(progressPerc);
                }*/
                //Citizens = [.. citizensList];
                AllCitizens = [.. Citizens];

                //ProgressBarText = "All data loaded.";
                //ProgressBarValue = 0;
                //if (TotalCount != 0) _dialogService.ShowMessage("Data loaded.");
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

        #region LANGUAGES

        private bool _setEnLanguageLock;
        public bool SetEnLanguageLock
        {
            get => _setEnLanguageLock;
            set
            {
                _setEnLanguageLock = value;
                OnPropertyChanged(nameof(SetEnLanguageLock));
            }
        }

        private bool _setRuLanguageLock;
        public bool SetRuLanguageLock
        {
            get => _setRuLanguageLock;
            set
            {
                _setRuLanguageLock = value;
                OnPropertyChanged(nameof(SetRuLanguageLock));
            }
        }

        private readonly RelayCommand _setEnLanguage;

        public RelayCommand SetEnLanguage
        {
            get
            {
                return _setEnLanguage ?? new RelayCommand(obj =>
                {
                    try
                    {
                        Language.SetLanguage("en");
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }

        private readonly RelayCommand _setRuLanguage;

        public RelayCommand SetRuLanguage
        {
            get
            {
                return _setRuLanguage ?? new RelayCommand(obj =>
                {
                    try
                    {
                        Language.SetLanguage("ru");
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage(ex.Message);
                    }
                });
            }
        }
        #endregion

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
                    _ = _citizensRepository.DeleteAllAsync();
                    _ = _citizensRepository.SaveChangesAsync();
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
