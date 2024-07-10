using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Models.Data;
using ManageCitizens.Repository;
using Spire.Xls;
using System.Data;

namespace ManageCitizens.Services
{
    class ExcelFileService : IFileService
    {
        public async Task ImportDataAsync(SQLCitizenRepository citizenRepository, IDialogService dialogService, string fileName)
        {

        }

        public async Task ExportDataAsync(List<Citizen> citizensList, IDialogService dialogService, string fileName)
        {

        }

        public List<Citizen> Open(string fileName)
        {
            List<Citizen>? citizens = [];

            Workbook workbook = new();

            workbook.LoadFromFile(fileName);

            Worksheet worksheet = workbook.Worksheets[0];

            DataTable dataTable = worksheet.ExportDataTable();

            citizens = ConvertDataTableToList(dataTable);

            workbook.Dispose();

            return citizens;
        }

        public void Save(string fileName, List<Citizen> citizensList)
        {
            Workbook workbook = new();

            workbook.Worksheets.Clear();

            Worksheet worksheet = workbook.Worksheets.Add("Citizens");

            DataTable dataTable = ConvertListToDataTable(citizensList);

            worksheet.InsertDataTable(dataTable, true, 1, 1, true);

            workbook.SaveToFile(fileName, ExcelVersion.Version2016);

            workbook.Dispose();
        }

        private static DataTable ConvertListToDataTable(List<Citizen> citizenList)
        {
            DataTable dataTable = new();

            foreach (var prop in new Citizen().GetType().GetProperties())
            {
                if (prop.Name != "Id")
                {
                    dataTable.Columns.Add(prop.Name, prop.PropertyType);
                }
            }

            foreach (var citizen in citizenList)
            {
                DataRow row = dataTable.NewRow();

                foreach (var prop in citizen.GetType().GetProperties())
                {
                    if (prop.Name != "Id")
                    {
                        row[prop.Name] = prop.GetValue(citizen);
                    }
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private static List<Citizen> ConvertDataTableToList(DataTable dataTable)
        {
            List<Citizen> citizens = [];

            foreach (DataRow row in dataTable.Rows)
            {
                citizens.Add(new Citizen(firstName: row[0].ToString(),
                                         lastName: row[1].ToString(),
                                         middleName: row[2].ToString(),
                                         birthday: DateOnly.Parse(row[3].ToString()),
                                         city: row[4].ToString(),
                                         country: row[5].ToString()));
            }

            return citizens;
        }
    }
}
