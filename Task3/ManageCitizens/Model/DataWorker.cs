using ManageCitizens.Model.Data;
using System.Linq;
using System.Web;

namespace ManageCitizens.Model
{
    public static class DataWorker
    {
        //create citizen
        public static string CreateCitizen(string name, string surName, string middleName, DateTime birthday)
        {
            string result = "Something is wrong";
            using Data.AppContext db = new();
            {
                if (!db.Citizens.Any(el => el.Name == name && el.SurName == surName && el.MiddleName == middleName && el.Birthday == birthday))
                {
                    Citizen citizen = new(name, surName, middleName, birthday);
                    db.Citizens.Add(citizen);
                    db.SaveChanges();
                    result = "New citizen has been added";
                }
            }

            return result;
        }

        //create city
        public static string CreateCity(string name)
        {
            string result = $"{name} already exists";
            using Data.AppContext db = new();
            {
                if (!db.Cities.Any(el => el.Name == name))
                {
                    City city = new(name);
                    db.Cities.Add(city);
                    db.SaveChanges();
                    result = $"{name} has been added";
                }
            }

            return result;
        }

        //create country
        public static string CreateCountry(string name)
        {
            string result = $"{name} already exists";
            using Data.AppContext db = new();
            {
                if (!db.Countries.Any(el => el.Name == name))
                {
                    Country country = new(name);
                    db.Countries.Add(country);
                    db.SaveChanges();
                    result = $"{name} has been added";
                }
            }

            return result;
        }

        //delete citizen
        public static string DeleteCitizen(Citizen citizen)
        {
            string result = $"{citizen.Name} doesn't exists";
            using Data.AppContext db = new();
            {
                if (db.Citizens.Any(el => el.Name == citizen.Name && el.SurName == citizen.SurName && el.MiddleName == citizen.MiddleName && el.Birthday == citizen.Birthday))
                {
                    db.Citizens.Remove(citizen);
                    db.SaveChanges();
                    result = $"'{citizen.SurName}' has been deleted";
                }
            }

            return result;
        }
        //delete city
        public static string DeleteCity(City city)
        {
            string result = $"{city.Name} doesn't exists";
            using Data.AppContext db = new();
            {
                if (db.Cities.Any(el => el.Name == city.Name))
                {
                    db.Cities.Remove(city);
                    db.SaveChanges();
                    result = $"'{city.Name}' has been deleted";
                }
            }

            return result;
        }
        //delete country
        public static string DeleteCountry(Country country)
        {
            string result = $"{country.Name} doesn't exists";
            using Data.AppContext db = new();
            {
                if (db.Countries.Any(el => el.Name == country.Name))
                {
                    db.Countries.Remove(country);
                    db.SaveChanges();
                    result = $"'{country.Name}' has been deleted";
                }
            }

            return result;
        }
        //edit citizen
        public static string EditCitizen(Citizen curCitizen, string newName, string newSurName, string newMiddleName, City city)
        {
            string result = $"{curCitizen.Name} doesn't exists";
            using Data.AppContext db = new();
            {
                Citizen citizen = db.Citizens.FirstOrDefault(el => el.Id == curCitizen.Id);
                if (citizen != null)
                {
                    citizen.Name = newName;
                    citizen.SurName = newSurName;
                    citizen.MiddleName = newMiddleName;
                    citizen.CityId = city.Id;
                    db.SaveChanges();
                    result = $"'{curCitizen.Name}' has been changed";
                }
            }

            return result;
        }
        //edit city
        public static string EditCity(City curCity, string newName)
        {
            string result = $"{curCity.Name} doesn't exists";
            using Data.AppContext db = new();
            {
                City city = db.Cities.FirstOrDefault(el => el.Id == curCity.Id);
                if (city != null)
                {
                    city.Name = newName;
                    db.SaveChanges();
                    result = $"'{curCity.Name}' has been changed";
                }
            }

            return result;
        }
        //edit country
        public static string EditCountry(Country curCountry, string newName)
        {
            string result = $"{curCountry.Name} doesn't exists";
            using Data.AppContext db = new();
            {
                Country country = db.Countries.FirstOrDefault(el => el.Id == curCountry.Id);
                if (country != null)
                {
                    country.Name = newName;
                    db.SaveChanges();
                    result = $"'{curCountry.Name}' has been changed";
                }
            }

            return result;
        }
        //get all citizens
        public static List<Citizen> GetAllCitizens()
        {
            using Data.AppContext db = new();
            return [.. db.Citizens];
        }
        //get all cities
        public static List<City> GetAllCities()
        {
            using Data.AppContext db = new();
            return [.. db.Cities];
        }
        //get all countries
        public static List<Country> GetAllCountries()
        {
            using Data.AppContext db = new();
            return [.. db.Countries];
        }
    }
}
