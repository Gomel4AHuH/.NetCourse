using System.Collections.Generic;

namespace ManageCitizens.Model
{
    public class Country(string name)
    {
        public int Id { get; set; }
        public string Name { get; set; } = name;
        public List<City> Cities { get; set; } = [];
    }
}
