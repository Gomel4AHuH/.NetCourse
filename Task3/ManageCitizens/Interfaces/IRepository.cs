using ManageCitizens.Models;

namespace ManageCitizens.Interfaces
{
    public interface IRepository : IDisposable
    {
        IEnumerable<Citizen> GetCitizenList();
        Citizen GetCitizen(int id);
        void Create(Citizen citizen);
        void Update(Citizen citizen);
        void Delete(int id);
        void Save();
    }
}
