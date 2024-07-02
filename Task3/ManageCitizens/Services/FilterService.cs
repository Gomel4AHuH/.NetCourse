using ManageCitizens.Models;
using System.Collections.ObjectModel;

namespace ManageCitizens.Services
{
    public class FilterService(string firstName, string lastName, string middleName, DateOnly birthday, string city, string country)
    {
        private string _firstName { get; set; } = firstName;
        private string _lastName { get; set; } = lastName;
        private string _middleName { get; set; } = middleName;
        private DateOnly _birthday { get; set; } = birthday;
        private string _city { get; set; } = city;
        private string _country { get; set; } = country;

        private IEnumerable<Citizen> _filteredCitizens = [];

        public IEnumerable<Citizen> CitizenSearch(ObservableCollection<Citizen> citizens)
        {
            _filteredCitizens = citizens.Where(citizen =>
                (string.IsNullOrEmpty(_firstName) || citizen.FirstName.Contains(_firstName, StringComparison.CurrentCultureIgnoreCase)) &&
                (string.IsNullOrEmpty(_lastName) || citizen.LastName.Contains(_lastName, StringComparison.CurrentCultureIgnoreCase)) &&
                (string.IsNullOrEmpty(_middleName) || citizen.MiddleName.Contains(_middleName, StringComparison.CurrentCultureIgnoreCase)) &&
                citizen.Birthday.Equals(_birthday) &&
                (string.IsNullOrEmpty(_city) || citizen.City.Contains(_city, StringComparison.CurrentCultureIgnoreCase)) &&
                (string.IsNullOrEmpty(_country) || citizen.Country.Contains(_country, StringComparison.CurrentCultureIgnoreCase))
                ).OrderBy(citizen => citizen.LastName);
            
            return _filteredCitizens;
        }
    }

}
