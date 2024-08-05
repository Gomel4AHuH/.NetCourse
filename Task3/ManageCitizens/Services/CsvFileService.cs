using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Repository;
using System.IO;
using System.Text;


namespace ManageCitizens.Services
{
    class CsvFileService : IFileService
    {
        const string _separator = ";";
        public async Task ImportDataAsync(SQLCitizenRepository citizenRepository, IDialogService dialogService, string fileName)
        {
            try
            {
                string[] lines = await File.ReadAllLinesAsync(fileName);

                foreach (string line in lines)
                {
                    string[] values = line.Split(_separator);
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
                byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                using FileStream fs = new(fileName, FileMode.OpenOrCreate);
                foreach (Citizen citizen in citizensList)
                {
                    string[] newLine = { citizen.Birthday.ToString(), citizen.FirstName, citizen.LastName, citizen.MiddleName, citizen.City, citizen.Country };
                    byte[] buffer = Encoding.Default.GetBytes(string.Join(_separator, newLine));
                    await fs.WriteAsync(buffer);
                    await fs.WriteAsync(newline);
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }
    }
}
