using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("AdminCollection")]
public class AdminContainer
{
    [XmlArray("Admins"), XmlArrayItem("Admin")]
    public List<Admin> admins = new List<Admin>();

    //Saves to the XML file
    public void Save(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(AdminContainer));

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    //Loads the XML file
    public static AdminContainer Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(AdminContainer));

        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as AdminContainer;
        }
    }
}