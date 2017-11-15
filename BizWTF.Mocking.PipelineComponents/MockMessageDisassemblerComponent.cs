using System;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Component;
//using Microsoft.BizTalk.Messaging;
using Microsoft.XLANGs.BaseTypes;
using Microsoft.XLANGs.RuntimeTypes;
using Microsoft.XLANGs.Pipeline;
using System.Drawing;
using System.Runtime.InteropServices;

using System.Resources;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.Diagnostics;

using System.Data.SqlClient;
using System.Data.EntityClient;

using System.Globalization;
using DocumentSpec = Microsoft.BizTalk.Component.Interop.DocumentSpec;


using BizWTF.Core.Entities;



namespace BizWTF.Mocking.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_DisassemblingParser)]
    [ComponentCategory(CategoryTypes.CATID_Streamer)]
    [Guid(COMPONENT_CLASSID)] 
    public class MockMessageDisassemblerComponent : IBaseComponent, IDisassemblerComponent, IComponentUI, IProbeMessage, IPersistPropertyBag
    {
        public const string COMPONENT_CLASSID = "7CB6148B-57B2-451F-AC45-DF4C83829380";

        public const string PROP_PipelineToExecute = "PipelineToExecute";

        //public const string NAMESPACE_SystemProps = "http://schemas.microsoft.com/BizTalk/2003/system-properties";
        //public const string PROPERTY_ReceivePortName = "ReceivePortName";
        //public const string PROPERTY_ReceivePortID = "ReceivePortID";


        private IBaseMessage _inputmessage;
        private IBaseMessage _outputmessage;
        System.Collections.Queue qOutputMsgs = new System.Collections.Queue();

        static ResourceManager resManager = new ResourceManager("BizWTF.Mocking.PipelineComponents.Resources.MockMessageDisassemblerComponent", Assembly.GetExecutingAssembly());


        /// <summary>
        /// OPTIONNAL : Fully qualified name of the pipeline to execute.
        /// Please note that a default config will be applied to the pipeline.
        /// </summary>
        public string PipelineToExecute
        {
            get { return this._pipelineName; }
            set { this._pipelineName = value; }
        }
        private string _pipelineName = null;

        protected bool ExecutePipeline
        {
            get
            { return !String.IsNullOrEmpty(this.PipelineToExecute); }
        }

        #region IBaseComponent
        public string Description
        {
            get { return resManager.GetString("Description"); }
        }

        public string Name
        {
            get { return resManager.GetString("Name"); }
        }

        public string Version
        {
            get { return resManager.GetString("Version"); }
        }
        #endregion



        void IPersistPropertyBag.GetClassID(out Guid classID)
        {
            classID = new Guid(COMPONENT_CLASSID);
        }

        void IPersistPropertyBag.InitNew()
        {
        }

        void IPersistPropertyBag.Load(IPropertyBag propertyBag, int errorLog)
        {
            try
            {
                object val = null;
                if (propertyBag != null)
                    propertyBag.Read(PROP_PipelineToExecute, out val, errorLog);
                else
                    throw new Exception();

                if (val != null)
                    this.PipelineToExecute = (string)val;
            }
            catch
            {
                this.PipelineToExecute = String.Empty;
            }
        }

        void IPersistPropertyBag.Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            object val = (object) this.PipelineToExecute;
            if (propertyBag != null)
            {
                propertyBag.Write(PROP_PipelineToExecute, ref val);
            }
        }

        /// <summary>
        /// Reads property value from property bag
        /// </summary>
        /// <param name="pb">Property bag</param>
        /// <param name="propName">Name of property</param>
        /// <returns>Value of the property</returns>
        private object ReadProperty(IPropertyBag propertyBag, String propName)
        {
            object val = null;
            try
            {
                propertyBag.Read(propName, out val, 0);
            }
            catch
            {
            }
            return val;

        }

        /// <summary>
        /// Writes property values into a property bag.
        /// </summary>
        /// <param name="pb">Property bag.</param>
        /// <param name="propName">Name of property.</param>
        /// <param name="val">Value of property.</param>
        private void WritePropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName, object val)
        {
            try
            {
                pb.Write(propName, ref val);
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
        }


        #region IDisassemblerComponent
        void IDisassemblerComponent.Disassemble(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            if (pContext == null)
            {
                throw new ArgumentNullException("context");
            }
            if (pInMsg == null)
            {
                throw new ArgumentNullException("inMsg");
            }

            bool isFault = false;

            object objFault = pInMsg.Context.Read("IsFault", "http://schemas.microsoft.com/BizTalk/2006/01/Adapters/WCF-properties");
            if (objFault != null)
                isFault = (bool)objFault;

            if (!isFault)
            {
                try
                {
                    this._inputmessage = pInMsg;
                    MultipartMessageDefinition netmsg = GetMessage(this._inputmessage);
                    List<Part> partList = new List<Part>(netmsg.Parts);

                    #region Obsolete
                    /// Find body part
                    //Part bodyPart = partList.Find(p => p.IsBodyPart == true);
                    //if (bodyPart == null)
                    //{
                    //    partList[0].IsBodyPart = true;
                    //    bodyPart = partList[0];
                    //}
                    #endregion

                    //ContextProperty executePipeline = netmsg.PropertyBag.FirstOrDefault(prop =>
                    //                                                                        prop.Namespace == BTSProperties.executePipeline.Name.Namespace
                    //                                                                        && prop.Name == BTSProperties.executePipeline.Name.Name);

                    IBaseMessage sourceMessage = this.BuildBTSMessage(pContext, pInMsg.Context, partList);
                    if (sourceMessage.BodyPart == null)
                    {
                        throw new Exception("No part was extracted from the mock message!");
                    }

                    if (this.ExecutePipeline)
                    {
                        object tempInstance = Activator.CreateInstance(Type.GetType(this.PipelineToExecute));
                        Microsoft.BizTalk.PipelineOM.ReceivePipeline rcvPipeline = (Microsoft.BizTalk.PipelineOM.ReceivePipeline)tempInstance;

                        IBasePropertyBag propBag = pContext.GetMessageFactory().CreatePropertyBag();
                        ExecutableRcvPipeline pipeline = new ExecutableRcvPipeline(rcvPipeline);
                        pipeline.Run(pContext, sourceMessage);

                        foreach (IBaseMessage outputMsg in pipeline.OutputMsgs)
                        {
                            this.attachPropetyBag(outputMsg, netmsg.PropertyBag);
                            qOutputMsgs.Enqueue(outputMsg);
                        }
                    }
                    else
                    {
                        this.attachPropetyBag(sourceMessage, netmsg.PropertyBag);
                        qOutputMsgs.Enqueue(sourceMessage);
                    }


                    #region Obsolete
                    //if (bodyPart.Data != null)
                    //{
                    //    string bodyType = bodyPart.Data.NamespaceURI + "#" + bodyPart.Data.LocalName;
                    //    XmlDocument xDoc = new System.Xml.XmlDocument();
                    //    xDoc.LoadXml(bodyPart.Data.OuterXml);
                    //    System.IO.MemoryStream memStream = new MemoryStream();
                    //    xDoc.PreserveWhitespace = true;
                    //    xDoc.Save(memStream);
                    //    memStream.Position = 0;
                    //    memStream.Seek(0, System.IO.SeekOrigin.Begin);


                    //    /// If Body part is an enveloppe, we have to split it
                    //    IDocumentSpec docSpec = pContext.GetDocumentSpecByType(bodyType);
                    //    XPathDocument xp = new XPathDocument(memStream);

                    //    if (!String.IsNullOrEmpty(docSpec.GetBodyPath()))
                    //    {
                    //        XPathNodeIterator xNI = xp.CreateNavigator().Select(docSpec.GetBodyPath());

                    //        while (xNI.MoveNext())
                    //        {
                    //            string nodeName = "";
                    //            string nodeNamespace = "";

                    //            if (xNI.Current.MoveToFirstChild())
                    //            {
                    //                XmlDocument tempNode = new XmlDocument();
                    //                tempNode.LoadXml(xNI.Current.OuterXml);
                    //                nodeName = tempNode.DocumentElement.LocalName;
                    //                nodeNamespace = tempNode.DocumentElement.NamespaceURI;

                    //                this.CreateMsg(pContext, pInMsg.Context, tempNode.OuterXml, bodyPart.PartName, netmsg.PropertyBag, partList.FindAll(p => p.IsBodyPart != true));
                    //            }

                    //            while (xNI.Current.MoveToNext(nodeName, nodeNamespace))
                    //            {
                    //                XmlDocument tempNode = new XmlDocument();
                    //                tempNode.LoadXml(xNI.Current.OuterXml);
                    //                this.CreateMsg(pContext, pInMsg.Context, tempNode.OuterXml, bodyPart.PartName, netmsg.PropertyBag, partList.FindAll(p => p.IsBodyPart != true));
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        this.CreateMsg(pContext, pInMsg.Context, bodyPart.Data.OuterXml, bodyPart.PartName, netmsg.PropertyBag, partList.FindAll(p => p.IsBodyPart != true));
                    //    }
                    //}
                    //else
                    //{
                    //    this.CreateMsg(pContext, pInMsg.Context, bodyPart.RawData, bodyPart.PartName, netmsg.PropertyBag, partList.FindAll(p => p.IsBodyPart != true));
                    //}
                    #endregion

                }
                catch (Exception e)
                {
                    if (this.GetFault(pInMsg) != null)
                    {
                        qOutputMsgs.Enqueue(pInMsg);
                        string faultText = this.GetFault(pInMsg).SelectSingleNode("/*[local-name()='Fault']/*[local-name()='faultstring']").InnerText;
                        throw new Exception(faultText);
                    }
                    else
                    {
                        System.Diagnostics.EventLog.WriteEntry(Constants.EVENT_SOURCE, "IDisassemblerComponent.Disassemble : \n" + e.Message + "\n" + e.StackTrace, System.Diagnostics.EventLogEntryType.Error, 8000, 1);
                        throw e;
                    }
                }
            }
            else
            {
                qOutputMsgs.Enqueue(pInMsg);
            }
        }

        private IBaseMessage BuildBTSMessage(IPipelineContext pContext, IBaseMessageContext sourceContext, List<Part> partList)
        {
            IBaseMessage newMessage = pContext.GetMessageFactory().CreateMessage();
            IBaseMessagePart msgPart = pContext.GetMessageFactory().CreateMessagePart();

            newMessage.Context = sourceContext;
            foreach (Part part in partList)
            {
                if (part.Data != null)
                {
                    msgPart.Charset = "utf-8";
                    msgPart.ContentType = part.ContentType;
                    msgPart.Data = GetPartStream(part.Data);
                    newMessage.AddPart(part.PartName, msgPart, part.IsBodyPart);
                }
                else
                {
                    System.IO.MemoryStream memStrm = new MemoryStream();
                    StreamWriter sw = new StreamWriter(memStrm);
                    sw.Write(part.RawData);
                    sw.Flush();
                    memStrm.Position = 0;

                    msgPart.ContentType = part.ContentType;
                    msgPart.Data = memStrm;
                    newMessage.AddPart(part.PartName, msgPart, part.IsBodyPart);
                }
            }
            return newMessage;
        }

        #region CreateMessage - Obsolete
        //private void CreateMsg(IPipelineContext pContext, IBaseMessageContext sourceContext, string nodeXml, string bodyPartName, ContextProperty[] propertyBag, List<Part> otherPartList)
        //{
        //    XmlDocument xDoc = new System.Xml.XmlDocument();
        //    xDoc.LoadXml(nodeXml);
            
        //    IBaseMessage newMsg = pContext.GetMessageFactory().CreateMessage();
        //    IBaseMessagePart bodyMsgPart = pContext.GetMessageFactory().CreateMessagePart();
            
        //    System.IO.MemoryStream memStrm = new MemoryStream();
        //    xDoc.PreserveWhitespace = true;
        //    xDoc.Save(memStrm);
        //    memStrm.Position = 0;
        //    memStrm.Seek(0, System.IO.SeekOrigin.Begin);
        //    bodyMsgPart.Data = memStrm;

        //    newMsg.Context = sourceContext;

        //    newMsg.AddPart(bodyPartName, bodyMsgPart, true);
        //    foreach(Part otherPart in otherPartList)
        //    {
        //        IBaseMessagePart part = pContext.GetMessageFactory().CreateMessagePart();
        //        part.Charset = "utf-8";
        //        part.ContentType = otherPart.ContentType;
        //        part.Data = GetPartStream(otherPart.Data);
        //        newMsg.AddPart(otherPart.PartName, part, false);
        //    }

        //    ContextProperty emulateXmlProp = propertyBag.FirstOrDefault(prop => prop.Namespace == BTSProperties.emulateXMLDisassembler.Name.Namespace
        //                                && prop.Name == BTSProperties.emulateXMLDisassembler.Name.Name);
        //    if (emulateXmlProp != null && emulateXmlProp.Value == "true")
        //    {
        //        IDocumentSpec docSpec = pContext.GetDocumentSpecByType(xDoc.DocumentElement.NamespaceURI + "#" + xDoc.DocumentElement.LocalName);
        //        IEnumerator annotations = docSpec.GetPropertyAnnotationEnumerator();
        //        newMsg.Context.Write(BTSProperties.documentSpecName.Name.Name,
        //                                                BTSProperties.documentSpecName.Name.Namespace,
        //                                                docSpec.DocSpecStrongName);
        //        newMsg.Context.Promote(BTSProperties.messageType.Name.Name, BTSProperties.messageType.Name.Namespace, docSpec.DocType);
        //        newMsg.Context.Promote(BTSProperties.suspendOnRoutingFailure.Name.Name, BTSProperties.suspendOnRoutingFailure.Name.Namespace, true);
        //        if (annotations != null)
        //            while (annotations.MoveNext())
        //            {
        //                IPropertyAnnotation annotation = (IPropertyAnnotation)annotations.Current;
        //                // Use annotation.Name or Namespace to get the data to demote in message
        //                // Make manual demoting using annotation.Xpath.

        //                System.Xml.XmlNode contextNode = xDoc.SelectSingleNode(annotation.XPath);
        //                if (contextNode != null)
        //                {
        //                    newMsg.Context.Promote(annotation.Name,
        //                                        annotation.Namespace,
        //                                        contextNode.InnerText);
        //                }


        //            }

        //        annotations = docSpec.GetDistinguishedPropertyAnnotationEnumerator();
        //        if (annotations != null)
        //        {
        //            while (annotations.MoveNext())
        //            {
        //                DictionaryEntry de = (DictionaryEntry)annotations.Current;
        //                XsdDistinguishedFieldDefinition distinguishedField = (XsdDistinguishedFieldDefinition)de.Value;

        //                // Use annotation.Name or Namespace to get the data to 
        //                // promote from message 
        //                System.Xml.XmlNode contextNode = xDoc.SelectSingleNode(distinguishedField.XPath);

        //                if (contextNode != null)
        //                {
        //                    newMsg.Context.Write(de.Key.ToString(),
        //                                    Globals.DistinguishedFieldsNamespace,
        //                                    contextNode.InnerText);
        //                }
        //            }
        //        }
        //    }


        //    this.attachPropetyBag(newMsg, propertyBag);

        //    qOutputMsgs.Enqueue(newMsg);
        //    pContext.ResourceTracker.AddResource(memStrm);
        //}

        //private void CreateRawMsg(IPipelineContext pContext, IBaseMessageContext sourceContext, string messageContent, string bodyPartName, ContextProperty[] propertyBag, List<Part> otherPartList)
        //{   
        //    IBaseMessage newMsg = pContext.GetMessageFactory().CreateMessage();
        //    IBaseMessagePart bodyMsgPart = pContext.GetMessageFactory().CreateMessagePart();
        //    System.IO.MemoryStream memStrm = new MemoryStream();
        //    StreamWriter sw = new StreamWriter(memStrm);
        //    sw.Write(messageContent);
        //    sw.Flush();
        //    memStrm.Position = 0;

        //    bodyMsgPart.Data = memStrm;

        //    newMsg.Context = sourceContext;
        //    newMsg.AddPart(bodyPartName, bodyMsgPart, true);
        //    foreach (Part otherPart in otherPartList)
        //    {
        //        IBaseMessagePart part = pContext.GetMessageFactory().CreateMessagePart();
        //        part.Charset = "utf-8";
        //        part.ContentType = otherPart.ContentType;
        //        part.Data = GetPartStream(otherPart.Data);
        //        newMsg.AddPart(otherPart.PartName, part, false);
        //    }

        //    newMsg.Context.Promote(BTSProperties.suspendOnRoutingFailure.Name.Name, BTSProperties.suspendOnRoutingFailure.Name.Namespace, true);

        //    this.attachPropetyBag(newMsg, propertyBag);

        //    qOutputMsgs.Enqueue(newMsg);
        //    pContext.ResourceTracker.AddResource(memStrm);
        //}
        #endregion

        private void attachPropetyBag(IBaseMessage newMsg, ContextProperty[] propertyBag)
        {
            ContextProperty rcvportNameProp = null;
            foreach (ContextProperty prop in propertyBag)
            {
                if (prop.Promoted)
                    newMsg.Context.Promote(prop.Name,
                                            prop.Namespace,
                                            formatValue(prop.Value));
                else
                    newMsg.Context.Write(prop.Name,
                                            prop.Namespace,
                                            formatValue(prop.Value));

                if (prop.Namespace == BTSProperties.receivePortName.Name.Namespace && prop.Name == BTSProperties.receivePortName.Name.Name)
                    rcvportNameProp = prop;
            }

            if (rcvportNameProp != null)
            {
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\BizTalk Server\3.0\Administration");
                string mgmtDbName = (string)registryKey.GetValue("MgmtDBName");
                string mgmtDbServer = (string)registryKey.GetValue("MgmtDBServer");

                using (DBEntities.MgmtDbEntities entities = new DBEntities.MgmtDbEntities(this.getConnectionStrings(mgmtDbServer, mgmtDbName)))
                {
                    var portIDs = from p in entities.bts_receiveport
                                  where p.nvcName == rcvportNameProp.Value
                                  select new PortInfo
                                  {
                                      ID = p.uidGUID,
                                      Name = p.nvcName
                                  };


                    if (portIDs.Count() > 0)
                    {
                        Guid portID = portIDs.First().ID;
                        newMsg.Context.Promote(BTSProperties.receivePortID.Name.Name,
                                            BTSProperties.receivePortID.Name.Namespace,
                                            portID.ToString("B"));
                    }
                }

            }
        }

        private string getConnectionStrings(string serverName, string mgmtDBName)
        {
            string entityMgmtConnStr = String.Empty;
            string providerName = "System.Data.SqlClient";

            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = mgmtDBName;
            sqlBuilder.IntegratedSecurity = true;

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;
            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
            entityBuilder.Metadata = @"res://*/DBEntities.MgmtDBEntities.csdl|res://*/DBEntities.MgmtDBEntities.ssdl|res://*/DBEntities.MgmtDBEntities.msl";

            entityMgmtConnStr = entityBuilder.ToString();

            return entityMgmtConnStr;
        }

        private object formatValue(string val)
        {
            object result = null;

            bool boolVal;
            int intVal;
            if (Boolean.TryParse(val, out boolVal))
                result = boolVal;
            else if (Int32.TryParse(val, out intVal))
                result = intVal;
            else
                result = val;

            return result;
        }

        IBaseMessage IDisassemblerComponent.GetNext(IPipelineContext pContext)
        {
            if (qOutputMsgs.Count > 0)
                return (IBaseMessage)qOutputMsgs.Dequeue();
            else
                return null;
        }
        #endregion


        #region IComponentUI
        IntPtr IComponentUI.Icon
        {
            get { return ((Bitmap)resManager.GetObject("Icon")).GetHicon(); }
        }

        IEnumerator IComponentUI.Validate(object projectSystem)
        {
            return null;
        }
        #endregion

        
        #region IProbeMessage
        bool IProbeMessage.Probe(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            return true;
        }
        #endregion


        private MultipartMessageDefinition DeserializeXMLMessage(XmlDocument xmlMessage)
        {
            //XmlSerializer ser = new XmlSerializer(typeof(MultipartMessage), Constants.SUBMISSION_NAMESPACE);
            Stream stm = new MemoryStream();
            xmlMessage.Save(stm);
            stm.Position = 0;

            return BizWTF.Core.Entities.Mocking.MultipartMessageSerializer.Deserialize(xmlMessage.DocumentElement); // (MultipartMessage)ser.Deserialize(stm);
        }

        private MultipartMessageDefinition GetMessage(IBaseMessage inMsg)
        {
            XmlDocument doc = new XmlDocument();
            MultipartMessageDefinition netmsg = null;
            try
            {
                inMsg.BodyPart.Data.Position = 0;
                doc.Load(inMsg.BodyPart.Data);
                netmsg = DeserializeXMLMessage(doc);
            }
            catch
            {
                throw new ArgumentNullException("inMsg");
            }
            return netmsg;
        }

        private XmlDocument GetFault(IBaseMessage inMsg)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                inMsg.BodyPart.Data.Position = 0;
                doc.Load(inMsg.BodyPart.Data);
                inMsg.BodyPart.Data.Position = 0;

                if (doc.DocumentElement.LocalName == "Fault")
                    return doc;
                else
                    return null;
            }
            catch
            {
                return null; //throw new ArgumentNullException("inMsg");
            }
            
        }

        private Stream GetPartStream(XmlElement xmlElement)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlElement.OuterXml);

            XmlNode elemsub = xmlElement.FirstChild;

            Stream partstm = new MemoryStream();
            Encoding encoding = Encoding.UTF8;
            if ((elemsub != null) && (xmlElement.Name.ToLower() == "bin"))
            {
                byte[] bin = Convert.FromBase64String(elemsub.InnerText);
                partstm.Write(bin, 0, bin.Length);
            }
            else if ((elemsub != null) && (xmlElement.Name.ToLower() == "text"))
            {
                byte[] text = encoding.GetBytes(elemsub.InnerText);
                partstm.Write(text, 0, text.Length);
            }
            else if ((elemsub != null) && (xmlElement.Name.ToLower() == "edifact"))
            {
                byte[] text = encoding.GetBytes(elemsub.InnerText);
                partstm.Write(text, 0, text.Length);
            }
            else
            {
                doc.Save(partstm);
            }
            partstm.Position = 0;
            return partstm;
        }


    }
}
