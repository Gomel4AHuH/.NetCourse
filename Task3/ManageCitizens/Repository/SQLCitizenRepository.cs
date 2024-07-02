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
        public IEnumerable<Citizen> GetCitizens()
        {
            return _db.Citizens;
        }
        public async Task InsertAsync(Citizen citizen)
        {
            await _db.Citizens.AddAsync(citizen);
        }
        public void Insert(Citizen citizen)
        {
            _db.Citizens.Add(citizen);
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }       
        public void Save()
        {
            _db.SaveChanges();
        }

        public void DeleteAll()
        {
            foreach (Citizen ctzn in _db.Citizens)
            {
                Delete(ctzn.Id);
            }
        }

        public void Delete(int id)
        {
            Citizen citizen = _db.Citizens.Find(id);
            if (citizen != null)
                _db.Citizens.Remove(citizen);
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
