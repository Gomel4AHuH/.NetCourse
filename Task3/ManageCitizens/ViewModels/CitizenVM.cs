using ManageCitizens.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ManageCitizens.ViewModels
{
    public class CitizenVM : INotifyPropertyChanged
    {
        private Citizen citizen;

        public CitizenVM(Citizen citizen)
        {
            this.citizen = citizen;
        }

        public string FirstName
        {
            get { return citizen.FirstName; }
            set
            {
                citizen.FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return citizen.LastName; }
            set
            {
                citizen.LastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public string MiddleName
        {
            get { return citizen.MiddleName; }
            set
            {
                citizen.MiddleName = value;
                OnPropertyChanged("MiddleName");
            }
        }
        public DateTime Birthday
        {
            get { return citizen.Birthday; }
            set
            {
                citizen.Birthday = value;
                OnPropertyChanged("Birthday");
            }
        }
        public string City
        {
            get { return citizen.City; }
            set
            {
                citizen.City = value;
                OnPropertyChanged("City");
            }
        }
        public string Country
        {
            get { return citizen.Country; }
            set
            {
                citizen.Country = value;
                OnPropertyChanged("Country");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }   
}
