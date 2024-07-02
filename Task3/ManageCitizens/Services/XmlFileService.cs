using ManageCitizens.Interfaces;
using ManageCitizens.Models;
using System.Xml;
using System.Xml.Linq;

namespace ManageCitizens.Services
{
    public class XmlFileService : IFileService
    {
        public List<Citizen> Open(string fileName)
        {
            List<Citizen>? citizens = [];

            XmlDocument xDoc = new();
            xDoc.Load(fileName);

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
                            case "Birthday" :
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
            }

            return citizens;
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
