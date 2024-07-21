using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using ManageCitizens.Repository;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace ManageCitizens.Services
{
    class XmlFileService : IFileService
    {

        public async Task ImportDataAsync(SQLCitizenRepository citizenRepository, IDialogService dialogService, string fileName)
        {
            try
            {
                XmlReaderSettings settings = new()
                {
                    Async = true
                };
                XmlReader reader = XmlReader.Create(fileName, settings);
                while (await reader.ReadAsync())
                {
                    Citizen citizen = new();
                    switch (reader.Name)
                    {
                        case "FirstName":
                            MessageBox.Show(await reader.GetValueAsync());
                            break;
                    }
                    /*switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            MessageBox.Show("Start Element {0}", reader.Name);
                            break;
                        case XmlNodeType.Text:
                            MessageBox.Show("Text Node: {0}",
                                     await reader.GetValueAsync());
                            break;
                        case XmlNodeType.EndElement:
                            MessageBox.Show("End Element {0}", reader.Name);
                            break;
                        default:
                            MessageBox.Show("Other node {0} with value {1}"+reader.NodeType+reader.Value);
                            break;
                    }*/
                    //await citizenRepository.InsertAsync(citizen);
                }

                //await citizenRepository.SaveChangesAsync();
                //string str = await reader.GetValueAsync();
                //MessageBox.Show(str);

                /*XmlTextReader xmlTextReader = new(fileName);
                string node = await xmlTextReader.GetValueAsync();
                MessageBox.Show(node);*/


                //Close the reader.
                //xmlTextReader.Close();
                /*XDocument xDoc = new();
                
                Task<XDocument> xDoc1 = await xDoc.LoadAsync(fileName);

                XmlElement? xRoot = xDoc.DocumentElement;
                if (xRoot != null)
                {
                    foreach (XmlElement xNode in xRoot)
                    {
                        Citizen citizen = new();

                        foreach (XmlNode childnode in xNode.ChildNodes)
                        {
                            switch (childnode.Name)
                            {
                                case "Birthday":
                                    citizen.Birthday = DateOnly.Parse(childnode.InnerText);
                                    break;
                                case "FirstName":
                                    citizen.FirstName = childnode.InnerText;
                                    break;
                                case "LastName":
                                    citizen.LastName = childnode.InnerText;
                                    break;
                                case "MiddleName":
                                    citizen.MiddleName = childnode.InnerText;
                                    break;
                                case "City":
                                    citizen.City = childnode.InnerText;
                                    break;
                                case "Country":
                                    citizen.Country = childnode.InnerText;
                                    break;
                            }

                        }
                        citizens.Add(citizen);
                    }
                }*/
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        public async Task ExportDataAsync(List<Citizen> citizensList, IDialogService dialogService, string fileName)
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
            //xDoc.Save(fileName);
            await Task.Run(() => xDoc.Save(fileName));

        }

        public void Save(string fileName, List<Citizen> citizensList)
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
        }
    }
}
