using System;

namespace ManageCitizens.Model
{
    public class Citizen(string name, string surName, string middleName, DateTime birthday)
    {
        public int Id { get; set; }
        public string Name { get; set; } = name;
        public string SurName { get; set; } = surName;
        public string MiddleName { get; set; } = middleName;
        public DateTime Birthday { get; set; } = birthday;
        public City City { get; set; } = new("");
        public int CityId { get; set; }
    }
}
