using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ManageCitizens.Services
{
    public class JsonFileService : IFileService
    {
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
