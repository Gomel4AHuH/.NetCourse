using System.Windows;

namespace ManageCitizens.Models.Data
{
    public class DataWorker
    {
        public static string CreateCitizen(string firstName, string surName, string middleName, DateTime birthday, string city, string country)
        {
            string result = "Something is wrong";
            using Data.ApplicationDbContext db = new();
            {
                //if (!db.Citizens.Any(el => el.FirstName == firstName && el.SurName == surName && el.MiddleName == middleName && el.Birthday == birthday))
                {
                    /*Citizen citizen = new(firstName, surName, middleName, birthday, city, country);
                    db.Citizens.Add(citizen);
                    db.SaveChanges();
                    result = "New citizen has been added";*/
                }
            }
            return result;
        }
    }
}
