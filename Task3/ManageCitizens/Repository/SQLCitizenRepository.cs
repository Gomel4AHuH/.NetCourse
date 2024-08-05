using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace ManageCitizens.Repository
{
    class SQLCitizenRepository(ApplicationDbContext applicationDbContext) : IRepository
    {
        private readonly ApplicationDbContext _db = applicationDbContext;
        public async IAsyncEnumerable<Citizen> GetCitizensAsync()
        {
            List<Citizen> citizens = await _db.Citizens.ToListAsync();

            foreach (Citizen citizen in citizens)
            {
                yield return citizen;
            }
        }
        public async Task InsertAsync(Citizen citizen)
        {
            await _db.Citizens.AddAsync(citizen);            
        }
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }       

        public async Task DeleteAllAsync()
        {
            await _db.Citizens.ExecuteDeleteAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
