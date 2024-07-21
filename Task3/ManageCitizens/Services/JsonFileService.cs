using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Repository;
using System.IO;
using System.Text.Json;

namespace ManageCitizens.Services
{
    class JsonFileService : IFileService
    {
        public async Task ImportDataAsync(SQLCitizenRepository citizenRepository, IDialogService dialogService, string fileName)
        {
            using (FileStream fs = new(fileName, FileMode.OpenOrCreate))
            {
                List<Citizen>? citizenList = await JsonSerializer.DeserializeAsync<List<Citizen>>(fs);
                foreach (Citizen citizen in citizenList)
                {
                    await citizenRepository.InsertAsync(citizen);
                }
            }

            await citizenRepository.SaveChangesAsync();
        }

        public async Task ExportDataAsync(List<Citizen> citizensList, IDialogService dialogService, string fileName)
        {
            using FileStream fs = new(fileName, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, citizensList);
        }
    }
}
