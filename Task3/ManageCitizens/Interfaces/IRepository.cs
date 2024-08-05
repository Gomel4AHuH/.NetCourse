using ManageCitizens.Models;

namespace ManageCitizens.Interfaces
{
    public interface IRepository : IDisposable
    {
        IAsyncEnumerable<Citizen> GetCitizensAsync();
        Task InsertAsync(Citizen citizen);
        Task SaveChangesAsync();
        Task DeleteAllAsync();
    }
}
