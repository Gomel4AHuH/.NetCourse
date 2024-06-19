using System.Collections.Generic;

namespace ManageCitizens.Model
{
    public class City(string name)
    {
        public int Id { get; set; }
        public string Name { get; set; } = name;
        public int CountryId { get; set; }
        public Country Country { get; set; } = new("");
        public List<Citizen> Citizens { get; set; } = [];
    }
}
