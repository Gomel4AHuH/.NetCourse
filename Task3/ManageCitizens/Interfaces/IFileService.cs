using ManageCitizens.Models;
using ManageCitizens.Repository;

namespace ManageCitizens.Interfaces
{
    interface IFileService
    {
        List<Citizen> Open(string fileName);
        void Save(string fileName, List<Citizen> citizenList);

        Task ImportDataAsync(SQLCitizenRepository citizenRepository, IDialogService dialogService, string fileName);

        Task ExportDataAsync(List<Citizen> citizensList, IDialogService dialogService, string fileName);
    }
}
