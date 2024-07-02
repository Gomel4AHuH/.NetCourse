using ManageCitizens.Models;

namespace ManageCitizens.Interfaces
{
    public interface IRepository : IDisposable
    {
        Task<IEnumerable<Citizen>> GetCitizensAsync();
        IEnumerable<Citizen> GetCitizens();
        Task InsertAsync(Citizen citizen);
        void Insert(Citizen citizen);
        Task SaveAsync();
        void Save();
        void DeleteAll();
        void Delete(int id);
    }
}
