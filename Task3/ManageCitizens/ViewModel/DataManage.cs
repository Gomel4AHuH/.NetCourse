using ManageCitizens.Model;
using ManageCitizens.View;
using System.ComponentModel;
using System.Windows;

namespace ManageCitizens.ViewModel
{
    public class DataManage : INotifyPropertyChanged
    {
        //all citizens
        private List<Citizen> allCitizens = DataWorker.GetAllCitizens();

        public List<Citizen> AllCitizens
        {
            get { return allCitizens; }
            set { allCitizens = value; NotifyPropertyChanged("AllCitizens"); }
        }
        //all cities
        private List<City> allCities = DataWorker.GetAllCities();

        public List<City> AllCities
        {
            get { return allCities; }
            set { allCities = value; NotifyPropertyChanged("AllCities"); }
        }

        //all countries
        private List<Country> allCountries = DataWorker.GetAllCountries();

        public List<Country> AllCountries
        {
            get { return allCountries; }
            set { allCountries = value; NotifyPropertyChanged("AllCountries"); }
        }

        #region METHODS TO OPEN WINDOWS
        //opening modal windows
        private void OpenCreateCitizen()
        {
            CreateCitizenWindow createCitizenWindow = new();
            SetCenterPositionAndOpen(createCitizenWindow);
        }
        private void OpenCreateCity()
        {
            CreateCityWindow createCityWindow = new();
            SetCenterPositionAndOpen(createCityWindow);
        }

        private void OpenCreateCountry()
        {
            CreateCountryWindow createCountryWindow = new();
            SetCenterPositionAndOpen(createCountryWindow);
        }

        private void OpenEditCitizen()
        {
            EditCitizenWindow editCitizenWindow = new();
            SetCenterPositionAndOpen(editCitizenWindow);
        }
        private void OpenEditCity()
        {
            EditCityWindow editCityWindow = new();
            SetCenterPositionAndOpen(editCityWindow);
        }

        private void OpenEditCountry()
        {
            EditCountryWindow editCountryWindow = new();
            SetCenterPositionAndOpen(editCountryWindow);
        }
        #endregion

        private void SetCenterPositionAndOpen(Window window)
        {
            window.Owner = Application.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
