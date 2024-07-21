using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace ManageCitizens.Repository
{
    class SQLCitizenRepository(ApplicationDbContext applicationDbContext) : IRepository
    {
        private ApplicationDbContext _db = applicationDbContext;
        public async Task<IEnumerable<Citizen>> GetCitizensAsync()
        {
            return await _db.Citizens.ToListAsync();
        }

        public async IAsyncEnumerable<Citizen> GetRecordsAsync()
        {
            var records = await _db.Citizens.ToListAsync();
            foreach (var record in records)
            {
                yield return record;
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
