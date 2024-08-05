using ClosedXML.Excel;
using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Repository;
using Spire.Xls;
using System.Data;

namespace ManageCitizens.Services
{
    class ExcelFileService : IFileService
    {
        public async Task ImportDataAsync(SQLCitizenRepository citizenRepository, IDialogService dialogService, string fileName)
        {
            try
            {
                List<Citizen>? citizens = [];

                await Task.Run(() => 
                {
                    Workbook workbook = new();

                    workbook.LoadFromFile(fileName);

                    Worksheet worksheet = workbook.Worksheets[0];

                    DataTable dataTable = worksheet.ExportDataTable();

                    citizens = ConvertDataTableToList(dataTable);

                    workbook.Dispose();
                });

                foreach (Citizen citizen in citizens)
                {
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
                await Task.Run(() =>
                {
                    XLWorkbook workbook = new();
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Citizens");
                    
                    worksheet.Cell(1, 1).Value = "FirstName";
                    worksheet.Cell(1, 2).Value = "LastName";
                    worksheet.Cell(1, 3).Value = "MiddleName";
                    worksheet.Cell(1, 4).Value = "Birthday";
                    worksheet.Cell(1, 5).Value = "City";
                    worksheet.Cell(1, 6).Value = "Country";

                    for (int i = 0; i < citizensList.Count; i++)
                    {
                        worksheet.Cell(i + 2, 2).Value = citizensList[i].FirstName;
                        worksheet.Cell(i + 2, 3).Value = citizensList[i].LastName;
                        worksheet.Cell(i + 2, 4).Value = citizensList[i].MiddleName;
                        worksheet.Cell(i + 2, 1).Value = citizensList[i].Birthday.ToString();
                        worksheet.Cell(i + 2, 5).Value = citizensList[i].City;
                        worksheet.Cell(i + 2, 6).Value = citizensList[i].Country;
                    }

                    workbook.SaveAs(fileName);
                });
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }            
        }        

        private static List<Citizen> ConvertDataTableToList(DataTable dataTable)
        {
            List<Citizen> citizens = [];

            foreach (DataRow row in dataTable.Rows)
            {
                citizens.Add(new Citizen(
                    firstName: row[1].ToString(),
                    lastName: row[2].ToString(),
                    middleName: row[3].ToString(),
                    birthday: DateOnly.Parse(row[0].ToString()),
                    city: row[4].ToString(),
                    country: row[5].ToString()));
            }

            return citizens;
        }
    }
}
