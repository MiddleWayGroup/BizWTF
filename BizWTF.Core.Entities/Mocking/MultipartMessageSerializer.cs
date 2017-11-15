using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Serialization;


namespace BizWTF.Core.Entities.Mocking
{
    public static class MultipartMessageSerializer
    {
        public static MultipartMessageDefinition Deserialize(XmlElement elt)
        {
            //XmlSerializer ser = new XmlSerializer(typeof(MultipartMessageDefinition));
            //return (MultipartMessageDefinition)ser.Deserialize(new StringReader(elt.OuterXml));

            System.IO.StringReader reader = new System.IO.StringReader(elt.OuterXml);
            XmlReader xReader = XmlReader.Create(reader, new XmlReaderSettings { CloseInput = true, IgnoreWhitespace=false });

            return Deserialize(xReader);
        }

        public static MultipartMessageDefinition Deserialize(XmlReader source)
        {
            MultipartXMLSerializer ser = new MultipartXMLSerializer();
            
            ser.ReadXml(source);

            return ser.MPMessage;
        }


        public static XmlDocument Serialize(MultipartMessageDefinition source)
        {
            StringWriter sw = new StringWriter();

            XmlSerializer ser = new XmlSerializer(typeof(MultipartMessageDefinition));
            ser.Serialize(sw, source);

            XmlDocument target = new XmlDocument();
            target.PreserveWhitespace = true;
            target.LoadXml(sw.ToString());
            return target;
        }

        public static MultipartMessageDefinition RetrieveDocument(string uri)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(uri);
            
            return Deserialize(doc.DocumentElement);
        }

        public static MultipartMessageDefinition RetrieveDocument(string resourceName, System.Reflection.Assembly resourceAsm)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(resourceAsm.GetManifestResourceStream(resourceName));

            return Deserialize(doc.DocumentElement);
        }
    }



    [XmlRoot("Class")] 
    class MultipartXMLSerializer : IXmlSerializable
    {
        public MultipartMessageDefinition MPMessage { get; set; }

        public System.Xml.Schema.XmlSchema GetSchema() { return null; }
        public void WriteXml(System.Xml.XmlWriter writer) { throw new NotImplementedException(); }

        
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            this.MPMessage = new MultipartMessageDefinition();

            //again this is a simple contrived example
            this.MPMessage.Description = doc.DocumentElement.SelectSingleNode("*[local-name()='Description']") == null ? String.Empty : doc.DocumentElement.SelectSingleNode("*[local-name()='Description']").InnerText;

            List<ContextProperty> propList = new List<ContextProperty>();
            foreach (XmlElement xmlProp in doc.DocumentElement.SelectNodes("*[local-name()='PropertyBag']/*[local-name()='ContextProperty']") )
            {
                ContextProperty tempProp = new ContextProperty();
                tempProp.Name = xmlProp.SelectSingleNode("*[local-name()='Name']").InnerText;
                tempProp.Namespace = xmlProp.SelectSingleNode("*[local-name()='Namespace']").InnerText;
                tempProp.Value = xmlProp.SelectSingleNode("*[local-name()='Value']").InnerText;
                tempProp.Promoted = Boolean.Parse(xmlProp.SelectSingleNode("*[local-name()='Promoted']").InnerText);

                propList.Add(tempProp);
            }
            this.MPMessage.PropertyBag = propList.ToArray();

            List<Part> partList = new List<Part>();
            foreach (XmlElement xmlPart in doc.DocumentElement.SelectNodes("*[local-name()='Parts']/*[local-name()='Part']"))
            {
                Part tempPart = new Part();
                tempPart.PartName = xmlPart.SelectSingleNode("*[local-name()='PartName']").InnerText;
                if (xmlPart.SelectSingleNode("*[local-name()='PartNumber']") != null)
                    tempPart.PartNumber = Int32.Parse(xmlPart.SelectSingleNode("*[local-name()='PartNumber']").InnerText);
                if (xmlPart.SelectSingleNode("*[local-name()='IsBodyPart']") != null)
                    tempPart.IsBodyPart = Boolean.Parse(xmlPart.SelectSingleNode("*[local-name()='IsBodyPart']").InnerText);
                if (xmlPart.SelectSingleNode("*[local-name()='ContentType']") != null)
                    tempPart.ContentType = xmlPart.SelectSingleNode("*[local-name()='ContentType']").InnerText;


                if (xmlPart.SelectSingleNode("*[local-name()='Data']") != null)
                {
                    XmlDocument tempData = new XmlDocument();

                    string dataXmlString = xmlPart.SelectSingleNode("*[local-name()='Data']").OuterXml;
                    dataXmlString = dataXmlString.Substring(dataXmlString.IndexOf('>') + 1);
                    dataXmlString = dataXmlString.Substring(0, dataXmlString.LastIndexOf("</Data>"));
                    tempData.LoadXml(dataXmlString);
                    tempPart.Data = tempData.DocumentElement;
                    //tempPart.Data = xmlPart.SelectSingleNode("*[local-name()='Data']") as XmlElement;
                }
                if (xmlPart.SelectSingleNode("*[local-name()='RawData']") != null)
                    tempPart.RawData = xmlPart.SelectSingleNode("*[local-name()='RawData']").InnerText;

                partList.Add(tempPart);
            }
            this.MPMessage.Parts = partList.ToArray();
        }
    }
}
