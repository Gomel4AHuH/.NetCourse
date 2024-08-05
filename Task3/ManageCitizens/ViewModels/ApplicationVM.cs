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

        public ApplicationVM(ApplicationDbContext applicationDbContext,
                             IDialogService dialogService,
                             JsonFileService jsonFileService,
                             CsvFileService csvFileService,
                             XmlFileService xmlFileService,
                             ExcelFileService excelFileService)
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
            ImportDataFromExcelFileAsyncCommand = new RelayCommand(async _ => await ImportDataFromExcelFileAsync());
            ExportDataToExcelFileAsyncCommand = new RelayCommand(async _ => await ExportDataToExcelFileAsync());
            
            Language.SetLanguage("en");
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
        public enum AppActions
        {
            Init,
            Clean,
            Import,
            Search,
            CancelSearch
        }

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
                    _dialogService.ShowMessage(Application.Current.Resources["DataExportedMessage"].ToString());
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
                    _ = ImportDataFromDbAsync();
                    UpdateLockButtonStatus(AppActions.Import);
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
                    _dialogService.ShowMessage(Application.Current.Resources["DataExportedMessage"].ToString());
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
                    UpdateLockButtonStatus(AppActions.Import);
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
                    _dialogService.ShowMessage(Application.Current.Resources["DataExportedMessage"].ToString());
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
                    _ = ImportDataFromDbAsync();
                    UpdateLockButtonStatus(AppActions.Import);
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
        public RelayCommand ExportDataToExcelFileAsyncCommand { get; }

        private async Task ExportDataToExcelFileAsync()
        {
            try
            {
                if (_dialogService.SaveFileDialog(s_fileExtention["excel"]))
                {
                    await _excelFileService.ExportDataAsync([.. Citizens], _dialogService, _dialogService.FilePath);
                    _dialogService.ShowMessage(Application.Current.Resources["DataExportedMessage"].ToString());
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }

        public RelayCommand ImportDataFromExcelFileAsyncCommand { get; }
        private async Task ImportDataFromExcelFileAsync()
        {
            try
            {
                if (_dialogService.OpenFileDialog(s_fileExtention["excel"]))
                {
                    await _excelFileService.ImportDataAsync(_citizensRepository, _dialogService, _dialogService.FilePath);
                    _ = ImportDataFromDbAsync();
                    UpdateLockButtonStatus(AppActions.Import);
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }       
        #endregion             

        #region DATABASE

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
                    UpdateLockButtonStatus(AppActions.Clean);
                    TotalCount = 0;
                    FilterCount = 0;
                    _dialogService.ShowMessage(Application.Current.Resources["DataDeletedMessage"].ToString());
                });
            }
        }        

        private readonly ApplicationDbContext _applicationDbContext;

        private readonly SQLCitizenRepository _citizensRepository;

        public async Task ImportDataFromDbAsync()
        {
            try
            {
                Citizens.Clear();

                await foreach (Citizen citizen in _citizensRepository.GetCitizensAsync())
                {                    
                    Citizens.Add(citizen);
                }
                
                TotalCount = Citizens.Count;
                                
                AllCitizens = [.. Citizens];

                UpdateLockButtonStatus(AppActions.Init);
                string message = (AllCitizens.Count > 0) ? "DataLoadedMessage" : "EmptyDataLoadedMessage";
                _dialogService.ShowMessage(Application.Current.Resources[message].ToString());
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage(ex.Message);
            }
        }             
        #endregion

        #endregion

        #region FILTER

        #region FIELDS
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

        private DateTime _birthdayFromFilter;
        public DateTime BirthdayFromFilter
        {
            get => _birthdayFromFilter;
            set
            {
                _birthdayFromFilter = value;
                OnPropertyChanged(nameof(BirthdayFromFilter));
            }
        }

        private DateTime _birthdayToFilter;
        public DateTime BirthdayToFilter
        {
            get => _birthdayToFilter;
            set
            {
                _birthdayToFilter = value;
                OnPropertyChanged(nameof(BirthdayToFilter));
            }
        }
        #endregion

        #region ACTIONS
        private readonly RelayCommand _searchCitizensCommand;

        public RelayCommand SearchCitizensCommand
        {
            get
            {
                return _searchCitizensCommand ?? new RelayCommand(obj =>
                {
                    if (string.IsNullOrEmpty(_lastNameFilter)
                    && string.IsNullOrEmpty(_firstNameFilter)
                    && string.IsNullOrEmpty(_middleNameFilter)
                    && string.IsNullOrEmpty(_birthdayFromFilter.ToString())
                    && string.IsNullOrEmpty(_birthdayToFilter.ToString())
                    && string.IsNullOrEmpty(_cityFilter)
                    && string.IsNullOrEmpty(_countryFilter))
                    {
                        _dialogService.ShowMessage(Application.Current.Resources["SearchEmptyMessage"].ToString());
                    }
                    else
                    {
                        FilterService FilterService = new(_firstNameFilter, _lastNameFilter, _middleNameFilter, DateOnly.FromDateTime(_birthdayFromFilter), DateOnly.FromDateTime(_birthdayToFilter), _cityFilter, _countryFilter);
                        IEnumerable<Citizen> FilteredCol = FilterService.CitizenSearch([.. AllCitizens]);
                        Citizens.Clear();

                        foreach (Citizen citizen in FilteredCol)
                        {
                            Citizens.Add(citizen);
                        }
                        FilterCount = Citizens.Count;
                        UpdateLockButtonStatus(AppActions.Search);
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
                    BirthdayFromFilter = new DateTime();
                    BirthdayToFilter = new DateTime();
                    CityFilter = "";
                    CountryFilter = "";
                    FilterCount = 0;
                    UpdateLockButtonStatus(AppActions.CancelSearch);
                });
            }
        }
        #endregion

        #endregion
            
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

        #region STATISTIC DATA
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
        #endregion

        #region APPLICATION
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
        #endregion

        #region LOCKING BUTTONS
        private bool _cleanButtonLock = false;
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

        private bool _exportButtonLock = false;
        public bool ExportButtonLock
        {
            get => _exportButtonLock;
            set
            {
                _exportButtonLock = value;
                OnPropertyChanged(nameof(ExportButtonLock));
            }
        }

        private bool _searchButtonLock = false;
        public bool SearchButtonLock
        {
            get => _searchButtonLock;
            set
            {
                _searchButtonLock = value;
                OnPropertyChanged(nameof(SearchButtonLock));
            }
        }

        private bool _cancelSearchButtonLock = false;
        public bool CancelSearchButtonLock
        {
            get => _cancelSearchButtonLock;
            set
            {
                _cancelSearchButtonLock = value;
                OnPropertyChanged(nameof(CancelSearchButtonLock));
            }
        }
        public void UpdateLockButtonStatus(AppActions action)
        {
            switch (action)
            {
                case AppActions.Clean:
                    CleanButtonLock = false;
                    ExportButtonLock = false;
                    SearchButtonLock = false;
                    break;
                case AppActions.Import:
                    if (Citizens.Count > 0)
                    {
                        CleanButtonLock = true;
                        ExportButtonLock = true;
                        SearchButtonLock = true;
                    }
                    break;
                case AppActions.Search:
                    CancelSearchButtonLock = true;
                    if (Citizens.Count == 0)
                    {
                        CleanButtonLock = false;
                        ExportButtonLock = false;
                        ImportButtonLock = false;
                    }
                    break;
                case AppActions.CancelSearch:
                    CancelSearchButtonLock = false;
                    ImportButtonLock = true;
                    if (Citizens.Count > 0)
                    {
                        CleanButtonLock = true;
                        ExportButtonLock = true;
                        SearchButtonLock = true;                        
                    }
                    break;
                case AppActions.Init:
                    if (Citizens.Count > 0)
                    {
                        CleanButtonLock = true;
                        ExportButtonLock = true;
                        SearchButtonLock = true;
                    }
                    break;
                default: break;
            }

        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }       
    }
}
