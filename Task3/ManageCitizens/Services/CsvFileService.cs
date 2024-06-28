using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.ViewModels;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Text;
using System.Windows;


namespace ManageCitizens.Services
{
    public class CsvFileService : IFileService
    {
        public List<Citizen> Open(string fileName)
        {
            List<Citizen>? citizens = [];

            TextFieldParser parser = new(fileName);

            parser.SetDelimiters(";");

            while (!parser.EndOfData)
            {
                string[] row = parser.ReadFields();
                if ((row !=  null) && (row.Length > 0))
                {
                    citizens.Add(new Citizen(row[1], row[2], row[3], DateTime.Parse(row[0]), row[4], row[5]));
                }
            }

            return citizens;
        }

        public void Save(string fileName, List<Citizen> citizensList)
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
    }
}
