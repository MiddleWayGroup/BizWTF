using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Microsoft.BizTalk.Message.Interop;

namespace BizWTF.Core.Entities.Mocking
{
    public static class MultipartMessageManager
    {
        public static MultipartMessageDefinition GenerateFromMessage(IBaseMessage pInMsg)
        {
            MultipartMessageDefinition tempMessage = new MultipartMessageDefinition();
            List<ContextProperty> propList = new List<ContextProperty>();
            
            List<Part> partList = new List<Part>();
            for (int i = 0; i < pInMsg.PartCount; i++)
            {
                string partName = String.Empty;
                IBaseMessagePart msgPart = pInMsg.GetPartByIndex(i, out partName);

                Part mockPart = new Part();
                mockPart.ContentType = msgPart.ContentType;
                mockPart.PartName = partName;
                mockPart.PartNumber = i;
                mockPart.IsBodyPart = (pInMsg.BodyPart.PartID == msgPart.PartID);
                mockPart.Data = null;

                try
                {
                    XmlDocument tempXML = new XmlDocument();
                    tempXML.Load(msgPart.Data);
                    mockPart.Data = tempXML.DocumentElement;

                    //ContextProperty performXMLdsm = new ContextProperty();
                    //performXMLdsm.Name = BTSProperties.emulateXMLDisassembler.Name.Name;
                    //performXMLdsm.Namespace = BTSProperties.emulateXMLDisassembler.Name.Namespace;
                    //performXMLdsm.Value = "true";
                    //propList.Add(performXMLdsm);
                }
                catch
                {
                    XmlTextReader rawStreamReader = new XmlTextReader(msgPart.Data);
                    mockPart.RawData = rawStreamReader.ReadContentAsString(); // String.Format("<![CDATA[{0}]]>", rawStreamReader.ReadContentAsString()); 
                }
                //msgPart.Data.Seek(0, System.IO.SeekOrigin.Begin);
                //msgPart.Data.Position = 0;

                partList.Add(mockPart);
            }
            tempMessage.Parts = partList.ToArray();

            for (uint iProp = 0; iProp < pInMsg.Context.CountProperties; iProp++)
            {
                string propName;
                string propNamespace;
                object value = pInMsg.Context.ReadAt((int)iProp, out propName, out propNamespace);

                ContextProperty prop = new ContextProperty();
                prop.Name = propName;
                prop.Namespace = propNamespace;
                prop.Value = value.ToString();
                prop.Promoted = pInMsg.Context.IsPromoted(propName, propNamespace);

                propList.Add(prop);
            }
            tempMessage.PropertyBag = propList.ToArray();

            return tempMessage;
        }
    }
}
