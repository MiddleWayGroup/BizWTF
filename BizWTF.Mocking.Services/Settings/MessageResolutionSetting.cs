using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.XPath;

using BizWTF.Core.Entities;
using BizWTF.Core.Entities.ProcessValidation;

namespace BizWTF.Mocking.Services.Settings
{
    public class MessageResolutionSetting
    {
        protected List<ControlProperty> _contextProps;
        protected List<PartSetting> _partSettings;

        public List<ControlProperty> ContextProps
        {
            get
            {
                if (this._contextProps == null)
                    this._contextProps = new List<ControlProperty>();
                return this._contextProps;
            }
            set { this._contextProps = value; }
        }

        public List<PartSetting> PartSettings
        {
            get
            {
                if (this._partSettings == null)
                    this._partSettings = new List<PartSetting>();
                return this._partSettings;
            }
            set { this._partSettings = value; }
        }

        public string Description;
        public MultipartMessageDefinition TargetMessage;


        /// <summary>
        /// Checks if this resolution setting applies to the given message.
        /// </summary>
        /// <param name="request">Message to probe</param>
        /// <returns>True if there is a match</returns>
        public bool Probe(MultipartMessageDefinition request)
        {
            foreach (ControlProperty prop in this.ContextProps)
            {
                if (!request.PropertyBag.ToList<ContextProperty>().Exists(cp => cp.Namespace == prop.Namespace && cp.Name == prop.Property && cp.Value == prop.Value))
                {
                    return false;
                }
            }

            foreach (PartSetting ps in this.PartSettings)
            {
                string partName;
                
                if (request.Parts == null)
                    return false;

                if (String.IsNullOrEmpty(ps.PartName))
                    partName = request.Parts[0].PartName;
                else
                    partName = ps.PartName;

                if (request.Parts.ToList().Exists(p => p.PartName == partName))
                {
                    Part messagePart = request.Parts.ToList().SingleOrDefault<Part>(p => p.PartName == partName);
                    if (messagePart.Data != null)
                    {
                        foreach (ControlField field in ps.XPathProps)
                        {
                            XPathDocument doc = new XPathDocument(new StringReader(messagePart.Data.OuterXml));
                            XPathNavigator nav = doc.CreateNavigator();
                            nav.Select(field.XPath);

                            XmlDocument partXml = new XmlDocument();
                            partXml.LoadXml(messagePart.Data.OuterXml);
                            XmlNodeList nodeList = partXml.SelectNodes(field.XPath);
                            foreach (XmlNode node in nodeList)
                                if (node.InnerText != field.Value && node.Value != field.Value)
                                    return false;
                        }
                    }
                }
                else
                    return false;

            }

            return true;
        }
    }

    public class PartSetting
    {
        protected List<ControlField> _xpathProps;

        public string PartName;

        public List<ControlField> XPathProps
        {
            get
            {
                if (this._xpathProps == null)
                    this._xpathProps = new List<ControlField>();
                return this._xpathProps;
            }
            set { this._xpathProps = value; }
        }

        public MultipartMessageDefinition TargetMessage;
    }
}
