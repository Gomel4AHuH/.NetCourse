using ManageCitizens.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace ManageCitizens.Services
{
    public class FilterService(string firstName, string lastName, string middleName, DateOnly birthday, string city, string country)
    {
        private string FirstName { get; set; } = firstName;
        private string LastName { get; set; } = lastName;
        private string MiddleName { get; set; } = middleName;
        private DateOnly Birthday { get; set; } = birthday;
        private string City { get; set; } = city;
        private string Country { get; set; } = country;

        private DateOnly Date = new(1, 1, 1);

        private IEnumerable<Citizen> FilteredCitizens = [];

        public IEnumerable<Citizen> CitizenSearch(ObservableCollection<Citizen> citizens)
        {
            FilteredCitizens = citizens.Where(citizen =>
                (string.IsNullOrEmpty(FirstName) || citizen.FirstName.Contains(FirstName, StringComparison.CurrentCultureIgnoreCase)) &&
                (string.IsNullOrEmpty(LastName) || citizen.LastName.Contains(LastName, StringComparison.CurrentCultureIgnoreCase)) &&
                (string.IsNullOrEmpty(MiddleName) || citizen.MiddleName.Contains(MiddleName, StringComparison.CurrentCultureIgnoreCase)) &&
                //((_birthday <> _date) || citizen.Birthday.Equals(_birthday)) &&
                (string.IsNullOrEmpty(City) || citizen.City.Contains(City, StringComparison.CurrentCultureIgnoreCase)) &&
                (string.IsNullOrEmpty(Country) || citizen.Country.Contains(Country, StringComparison.CurrentCultureIgnoreCase))
                ).OrderBy(citizen => citizen.LastName);
            MessageBox.Show(FilteredCitizens.Count().ToString());
            return FilteredCitizens;
        }
    }

}
