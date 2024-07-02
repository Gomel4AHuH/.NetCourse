using ManageCitizens.Models;

namespace ManageCitizens.Interfaces
{
    interface IFileService
    {
        public List<Citizen> Open(string fileName);
        public void Save(string fileName, List<Citizen> citizenList);
    }
}
