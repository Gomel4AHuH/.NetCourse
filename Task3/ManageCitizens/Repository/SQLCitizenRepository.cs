using ManageCitizens.Models;
using ManageCitizens.Interfaces;
using ManageCitizens.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace ManageCitizens.Repository
{
    class SQLCitizenRepository(ApplicationDbContext applicationDbContext) : IRepository
    {
        private ApplicationDbContext _db = applicationDbContext;

        public IEnumerable<Citizen> GetCitizenList()
        {
            return _db.Citizens;
        }

        public Citizen GetCitizen(int id)
        {
            return _db.Citizens.Find(id);
        }

        public void Create(Citizen citizen)
        {
            _db.Citizens.Add(citizen);
        }

        public void Update(Citizen citizen)
        {
            _db.Entry(citizen).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Citizen citizen = _db.Citizens.Find(id);
            if (citizen != null)
                _db.Citizens.Remove(citizen);
        }

        public void Save()
        {
            _db.SaveChanges();
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
