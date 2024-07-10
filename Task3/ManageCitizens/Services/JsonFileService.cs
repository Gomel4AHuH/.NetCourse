using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Repository;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace ManageCitizens.Services
{
    class JsonFileService : IFileService
    {
        public async Task ImportDataAsync(SQLCitizenRepository citizenRepository, IDialogService dialogService, string fileName)
        {
            /*using FileStream fs = new(fileName, FileMode.OpenOrCreate);
            return await JsonSerializer.DeserializeAsync<Dictionary<string, int>>(fs);*/
        }

        public async Task ExportDataAsync(List<Citizen> citizensList, IDialogService dialogService, string fileName)
        {
            using FileStream fs = new(fileName, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fs, citizensList);
        }
        public List<Citizen> Open(string fileName)
        {
            List<Citizen>? citizens = [];
            DataContractJsonSerializer jsonFormatter =
                new(typeof(List<Citizen>));
            using (FileStream fs = new(fileName, FileMode.OpenOrCreate))
            citizens = jsonFormatter.ReadObject(fs) as List<Citizen>;

            return citizens;
        }

        public void Save(string fileName, List<Citizen> citizensList)
        {
            DataContractJsonSerializer jsonFormatter =
                new(typeof(List<Citizen>));
            using FileStream fs = new(fileName, FileMode.Create);
            jsonFormatter.WriteObject(fs, citizensList);
        }
    }
}
