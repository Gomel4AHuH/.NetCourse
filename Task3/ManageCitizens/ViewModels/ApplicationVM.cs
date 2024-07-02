using ManageCitizens.Commands;
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
        private ObservableCollection<Citizen> _citizens { get; set; } = [];
        private ObservableCollection<Citizen> _allCitizens { get; set; } = [];
        public IEnumerable<Citizen> CitizensCollection => _citizens;
        public IEnumerable<Citizen> AllCitizensCollection => _allCitizens;


        public ApplicationVM(ApplicationDbContext applicationDbContext, IDialogService dialogService, JsonFileService jsonFileService, CsvFileService csvFileService, XmlFileService xmlFileService, ExcelFileService excelFileService)
        {
            _dialogService = dialogService;
            _jsonFileService = jsonFileService;
            _csvFileService = csvFileService;
            _xmlFileService = xmlFileService;
            _excelFileService = excelFileService;
            _applicationDbContext = applicationDbContext;

            _citizensRepository = new SQLCitizenRepository(new ApplicationDbContext());
            //_importDataFromCsvFileAsyncCommand = new RelayCommand(ImportDataFromCsvFileAsync);
            Task task = ImportDataFromDbAsync();
        }
        #endregion
        
        #region IMPORT/EXPORT DATA COMMAND
        private readonly IDialogService _dialogService;
        private static readonly Dictionary<string, string> _fileExtention = new()
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
                        if (_dialogService.SaveFileDialog(_fileExtention["json"]))
                        {
                            _jsonFileService.Save(_dialogService.FilePath, [.. _citizens]);
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
                        if (_dialogService.OpenFileDialog(_fileExtention["json"]))
                        {
                            List<Citizen> citizens = _jsonFileService.Open(_dialogService.FilePath);
                            _citizens.Clear();
                            foreach (Citizen ctzn in citizens)
                            {
                                ctzn.Id = 0;
                                _citizensRepository.Insert(ctzn);
                            }
                            
                            _citizensRepository.Save();
                            Task task = ImportDataFromDbAsync();
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
        private readonly RelayCommand _exportDataToCsvFileCommand;

        public RelayCommand ExportDataToCsvFileCommand
        {
            get
            {
                return _exportDataToCsvFileCommand ?? new RelayCommand(obj =>
                {
                    try
                    {                        
                        if (_dialogService.SaveFileDialog(_fileExtention["csv"]))
                        {
                            _csvFileService.Save(_dialogService.FilePath, [.. _citizens]);
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

        //private AsyncRelayCommand _importDataFromCsvFileAsyncCommand;
        //public AsyncRelayCommand ImportDataFromCsvFileAsyncCommand => _importDataFromCsvFileAsyncCommand;
        private async Task ImportDataFromCsvFileAsync()
        {
            MessageBox.Show("Dialog csv async");
            try
            {
                if (_dialogService.OpenFileDialog(_fileExtention["csv"]))
                {
                    MessageBox.Show("Before OpenAsync");
                    IAsyncEnumerable<Citizen> citizens = _csvFileService.OpenAsync(_dialogService.FilePath);
                    MessageBox.Show("After OpenAsync");
                    _citizens.Clear();
                    await foreach (Citizen ctzn in citizens)
                        await _citizensRepository.InsertAsync(ctzn);
                    MessageBox.Show("Afer InsertAsync OpenAsync");
                    await _citizensRepository.SaveAsync();
                    Task task = ImportDataFromDbAsync();
                    _dialogService.ShowMessage("Data imported.");
                }
            }
            catch (Exception ex)
            {
                    _dialogService.ShowMessage(ex.Message);
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
                        if (_dialogService.OpenFileDialog(_fileExtention["csv"]))
                        {
                            List<Citizen> citizens = _csvFileService.Open(_dialogService.FilePath);
                            _citizens.Clear();
                            foreach (Citizen ctzn in citizens)
                                _citizensRepository.Insert(ctzn);
                            _citizensRepository.Save();
                            Task task = ImportDataFromDbAsync();
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
                        if (_dialogService.SaveFileDialog(_fileExtention["xml"]))
                        {
                            _xmlFileService.Save(_dialogService.FilePath, [.. _citizens]);
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
                        if (_dialogService.OpenFileDialog(_fileExtention["xml"]))
                        {
                            List<Citizen> citizens = _xmlFileService.Open(_dialogService.FilePath);
                            _citizens.Clear();
                            foreach (Citizen ctzn in citizens)
                                _citizensRepository.Insert(ctzn);                            
                            _citizensRepository.Save();
                            Task task = ImportDataFromDbAsync();
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
                        if (_dialogService.SaveFileDialog(_fileExtention["excel"]))
                        {
                            _excelFileService.Save(_dialogService.FilePath, [.. _citizens]);
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
                        if (_dialogService.OpenFileDialog(_fileExtention["excel"]))
                        {
                            List<Citizen> citizens = _excelFileService.Open(_dialogService.FilePath);
                            _citizens.Clear();
                            foreach (Citizen ctzn in citizens)
                                _citizensRepository.Insert(ctzn);
                            _citizensRepository.Save();
                            Task task = ImportDataFromDbAsync();
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

        #region DATABASE
        private ApplicationDbContext _applicationDbContext;

        private SQLCitizenRepository _citizensRepository;
        private async Task ImportDataFromDbAsync()
        {
            try
            {
                IEnumerable<Citizen> citizensList = await _citizensRepository.GetCitizensAsync();
                 foreach (Citizen citizen in citizensList)
                {
                    _citizens.Add(citizen);
                }
                _allCitizens = [.. _citizens];
                TotalCount = _allCitizens.Count;
                DataNotEmpty();
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

        private bool _isDataNotEmpty;
        public bool IsDataNotEmpty
        {
            get => _isDataNotEmpty;
            set
            {
                _isDataNotEmpty = value;
                OnPropertyChanged(nameof(IsDataNotEmpty));
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
                        FilterService filterService = new(_firstNameFilter, _lastNameFilter, _middleNameFilter, _birthdayFilter, _cityFilter, _countryFilter);
                        IEnumerable<Citizen> testCol = filterService.CitizenSearch([.. _citizens]);
                        _citizens.Clear();

                        foreach (Citizen citizen in testCol)
                        {
                            _citizens.Add(citizen);
                        }
                        IsDataFiltered = true;
                        FilterCount = _citizens.Count;
                        DataNotEmpty();
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
                    _citizens.Clear();
                    foreach (Citizen citizen in _allCitizens)
                    {
                        _citizens.Add(citizen);
                    }
                    FirstNameFilter = "";
                    LastNameFilter = "";
                    MiddleNameFilter = "";
                    BirthdayFilter = new DateOnly(1, 1, 1);
                    CityFilter = "";
                    CountryFilter = "";
                    IsDataFiltered = false;
                    FilterCount = 0;
                    DataNotEmpty();
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
        private void DataNotEmpty()
        {
            IsDataNotEmpty = (_citizens.Count > 0);
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
                    _citizensRepository.DeleteAll();
                    _citizensRepository.Save();
                    _citizens.Clear();
                    DataNotEmpty();
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
