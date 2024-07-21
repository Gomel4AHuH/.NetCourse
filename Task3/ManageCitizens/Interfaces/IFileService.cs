using ManageCitizens.Models;
using ManageCitizens.Repository;

namespace ManageCitizens.Interfaces
{
    interface IFileService
    {
        Task ImportDataAsync(SQLCitizenRepository citizenRepository, IDialogService dialogService, string fileName);

        Task ExportDataAsync(List<Citizen> citizensList, IDialogService dialogService, string fileName);
    }
}
