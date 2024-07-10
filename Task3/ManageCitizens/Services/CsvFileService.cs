using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Repository;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Text;


namespace ManageCitizens.Services
{
    class CsvFileService : IFileService
    {
        public async Task ImportDataAsync(SQLCitizenRepository citizenRepository, IDialogService dialogService, string fileName)
        {
            try
            {
                string[] lines = await File.ReadAllLinesAsync(fileName);

                foreach (string line in lines)
                {
                    string[] values = line.Split(';');
                    Citizen citizen = new(values[1], values[2], values[3], DateOnly.Parse(values[0]), values[4], values[5]);
                    await citizenRepository.InsertAsync(citizen);
                }

                await citizenRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        public async Task ExportDataAsync(List<Citizen> citizensList, IDialogService dialogService, string fileName)
        {
            try 
            {
                using FileStream fs = new(fileName, FileMode.OpenOrCreate);
                string separator = ";";
                StringBuilder output = new();
                byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);

                foreach (Citizen citizen in citizensList)
                {
                    string[] newLine = { citizen.Birthday.ToString(), citizen.FirstName, citizen.LastName, citizen.MiddleName, citizen.City, citizen.Country };
                    byte[] buffer = Encoding.Default.GetBytes(string.Join(separator, newLine));
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Write(newline, 0, newline.Length);
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        public List<Citizen> Open(string fileName)
        {
            List<Citizen>? citizens = [];

            return citizens;
        }

        public void Save(string fileName, List<Citizen> citizensList)
        {            
        }
    }
}
