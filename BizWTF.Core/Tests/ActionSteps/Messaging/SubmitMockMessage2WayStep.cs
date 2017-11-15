using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.Serialization;

using BizWTF.Core.Utilities;
using BizWTF.Core;
using BizWTF.Core.Entities;
using BizWTF.Core.Entities.Mocking;

namespace BizWTF.Core.Tests.ActionSteps.Messaging
{
    public class SubmitMockMessage2WayStep : ActionStep
    {
        protected string _sourceFile;
        protected string _destURI;
        protected string _targetContextProperty;

        public string SourceFile
        {
            get { return this._sourceFile; }
            set { this._sourceFile = value; }
        }
        public string SourceResource { get; set; }
        public string SourceResourceAssembly { get; set; }

        public string DestURI
        {
            get { return this._destURI; }
            set { this._destURI = value; }
        }
        public string TargetContextProperty
        {
            get { return this._targetContextProperty; }
            set { this._targetContextProperty = value; }
        }


        public SubmitMockMessage2WayStep()
            : base()
        {}

        public SubmitMockMessage2WayStep(string stepName)
            : base(stepName)
        {
        }


        public override bool ExecuteStep()
        {
            MultipartMessageDefinition tempMsg = null;
            XmlTextReader xmlBodyReader = null;

            this.AppendResultDescription(0, "Executing step {0} ({1}):", this.StepName, this.StepType);

            if (!File.Exists(this.SourceFile) && String.IsNullOrEmpty(this.SourceResource))
            {
                this.AppendResultDescription(1, "File {0} does not exist.", this.SourceFile);
                this.Result = StepResult.Error;
            }
            else if (File.Exists(this.SourceFile))
            {
                //xmlBodyReader = new XmlTextReader(this.SourceFile);
                tempMsg = MultipartMessageSerializer.RetrieveDocument(this.SourceFile);
            }
            else
            {
                Assembly asmb = Assembly.Load(this.SourceResourceAssembly);
                //xmlBodyReader = new XmlTextReader(asmb.GetManifestResourceStream(this.SourceResource));
                tempMsg = MultipartMessageSerializer.RetrieveDocument(this.SourceResource, asmb);
            }

            if(this.Result != StepResult.Error)
            {
                if (!string.IsNullOrEmpty(this.TestID))
                {
                    List<ContextProperty> props = tempMsg.PropertyBag.ToList<ContextProperty>();
                    props.Add(new ContextProperty
                    {
                        Name = BTSProperties.testID.Name.Name,
                        Namespace = BTSProperties.testID.Name.Namespace,
                        Promoted = false,
                        Value = this.TestID
                    });
                    tempMsg.PropertyBag = props.ToArray();
                }
                XmlDocument doc = MultipartMessageSerializer.Serialize(tempMsg);
                xmlBodyReader = new XmlTextReader(new StringReader(doc.OuterXml));

                BizTalk2WayReference.TwoWayAsyncClient client = new BizTalk2WayReference.TwoWayAsyncClient();
                try
                {
                    client.Endpoint.Address = new System.ServiceModel.EndpointAddress(this.DestURI);
                    client.Open();

                    Message request = Message.CreateMessage(MessageVersion.Soap11, "Get2WayMultipartMessage", xmlBodyReader);
                    
                    Message response = client.BizTalkSubmit(request);
                    this.AppendResultDescription(1, "[OK] Send file {0}, received response from {1}", this.SourceFile, DestURI);

                    
                    
                    HttpResponseMessageProperty httpResponse = (HttpResponseMessageProperty) response.Properties["httpResponse"];
                    if (httpResponse.StatusCode != System.Net.HttpStatusCode.InternalServerError)
                    {
                        MemoryStream ms = new MemoryStream();
                        XmlWriter xw = XmlWriter.Create(ms, new XmlWriterSettings { Indent = true, IndentChars = "  ", OmitXmlDeclaration = true });
                        XmlDictionaryReader bodyReader = response.GetReaderAtBodyContents();

                        while (bodyReader.NodeType != XmlNodeType.EndElement && bodyReader.LocalName != "Body" && bodyReader.NamespaceURI != "http://schemas.xmlsoap.org/soap/envelope/")
                        {
                            if (bodyReader.NodeType != XmlNodeType.Whitespace)
                            {
                                xw.WriteNode(bodyReader, true);
                            }
                            else
                            {
                                bodyReader.Read(); // ignore whitespace; maintain if you want
                            }
                        }
                        xw.Flush();

                        XmlSerializer serializer = new XmlSerializer(typeof(MultipartMessageDefinition));
                        ms.Seek(0, SeekOrigin.Begin);
                        BTSTestContext.AddParam(this.TargetContextProperty, serializer.Deserialize(ms));
                    }
                    else
                    {
                        MessageFault fault = MessageFault.CreateFault(response, 10000000);
                        string faultString = fault.Reason.ToString();

                        //XmlDocument faultDoc = new XmlDocument();
                        //faultDoc.Load(bodyReader);

                        this.AppendResultDescription(1, "[KO] Error when sending file {0} to {1} : {2}", this.SourceFile, DestURI, faultString);
                        this.Result = StepResult.Error;
                    }
                }
                catch (Exception exc)
                {
                    this.AppendResultDescription(1, "[KO] Error when sending file {0} to {1} : {2}", this.SourceFile, DestURI, exc.Message);
                    this.Result = StepResult.Error;
                }
                finally
                {
                    if (client != null)
                    {
                        if (client.State == System.ServiceModel.CommunicationState.Opened)
                            client.Close();
                    }
                }
            }
            if (this.Result == StepResult.Working)
                this.Result = StepResult.OK;

            return (this.Result != StepResult.Error);
        }


        protected string processFileName(string sourceFile, string destPattern)
        {
            string destFileName = String.Empty;

            FileInfo fi = new FileInfo(sourceFile);
            destFileName = destPattern.Replace("%SourceFileName%", fi.Name.Replace(fi.Extension, ""))
                                        .Replace("%ShortDate%", DateTime.Now.ToString("yyyyMMdd"))
                                        .Replace("%LongDate%", DateTime.Now.ToString("yyyyMMddHHmmss"))
                                        .Replace("%Guid%", Guid.NewGuid().ToString())
                                        .Replace("%SourceFileExtension%", fi.Extension);
            return destFileName;
        }
    }
}
