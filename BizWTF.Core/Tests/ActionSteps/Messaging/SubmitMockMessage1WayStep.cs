using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ServiceModel.Channels;
using System.Xml;

using BizWTF.Core.Utilities;
using BizWTF.Core;
using BizWTF.Core.Entities;
using BizWTF.Core.Entities.Mocking;
using System.ServiceModel.Configuration;

namespace BizWTF.Core.Tests.ActionSteps.Messaging
{
    public class SubmitMockMessage1WayStep : ActionStep
    {
        protected string _sourceFile;
        protected string _destURI;

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


        public SubmitMockMessage1WayStep()
            : base()
        { }

        public SubmitMockMessage1WayStep(string stepName)
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

            if (this.Result != StepResult.Error)
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
                xmlBodyReader.WhitespaceHandling = WhitespaceHandling.All;

                //xmlBodyReader.i

                BizTalk1WayReference.TwoWayAsyncVoidClient client = new BizTalk1WayReference.TwoWayAsyncVoidClient();
                try
                {
                    client.Endpoint.Address = new System.ServiceModel.EndpointAddress(this.DestURI);
                    client.Open();

                    Message request = Message.CreateMessage(MessageVersion.Soap11, "Get1WayMultipartMessage", xmlBodyReader);
                    client.BizTalkSubmit(request);
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

                this.AppendResultDescription(1, "[OK] Sent file {0} to {1}", this.SourceFile, DestURI);
            }

            if (this.Result == StepResult.Working)
                this.Result = StepResult.OK;

            return (this.Result == StepResult.OK);
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
