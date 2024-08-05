using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Repository;
using System.Xml.Linq;

namespace ManageCitizens.Services
{
    class XmlFileService : IFileService
    {
        public async Task ImportDataAsync(SQLCitizenRepository citizenRepository, IDialogService dialogService, string fileName)
        {
            try
            {
                using var file = System.IO.File.OpenRead(fileName);
                XDocument xDoc = await XDocument.LoadAsync(file, LoadOptions.None, CancellationToken.None);
                XElement? xRecord = xDoc.Element("TestProgram");
                if (xRecord != null)
                {
                    foreach (XElement record in xRecord.Elements("Record"))
                    {
                        Citizen citizen = new()
                        {
                            Birthday = DateOnly.Parse(record.Element("Birthday").Value),
                            FirstName = record.Element("FirstName").Value,
                            LastName = record.Element("LastName").Value,
                            MiddleName = record.Element("MiddleName").Value,
                            City = record.Element("City").Value,
                            Country = record.Element("Country").Value
                        };

                        await citizenRepository.InsertAsync(citizen);
                    }
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
                    XDocument xDoc = new();
                    XElement root = new("TestProgram");
                    foreach (Citizen citizen in citizensList)
                    {
                        XElement record = new("Record");
                        record.Add(new XAttribute("id", citizen.Id));
                        record.Add(new XElement("Birthday", citizen.Birthday));
                        record.Add(new XElement("FirstName", citizen.FirstName));
                        record.Add(new XElement("LastName", citizen.LastName));
                        record.Add(new XElement("MiddleName", citizen.MiddleName));
                        record.Add(new XElement("City", citizen.City));
                        record.Add(new XElement("Country", citizen.Country));

                        root.Add(record);
                    }

                    xDoc.Add(root);
                    xDoc.Save(fileName);
                });
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }
    }
}
