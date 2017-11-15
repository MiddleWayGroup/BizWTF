using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using Microsoft.BizTalk.Operations;
using Microsoft.BizTalk.Message.Interop;

using BizWTF.Core.Entities;
using BizWTF.Core.Entities.Mocking;
using BizWTF.Core.Entities.ProcessValidation;
using BizWTF.Core.BizTalk.Entities;
using BizWTF.Core.Utilities;


namespace BizWTF.Core.Tests.GetSteps
{
    [Serializable]
    public class ExtractLiveProcessMessagesStep : GetStep
    {
        protected string _processName;
        protected List<ControlProperty> _contextProps;
        protected List<ControlField> _xpathProps;
        protected string _targetContextProperty;


        public string ProcessName
        {
            get { return this._processName; }
            set { this._processName = value; }
        }
        public string TargetContextProperty
        {
            get { return this._targetContextProperty; }
            set { this._targetContextProperty = value; }
        }

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


        public List<MultipartMessageDefinition> FoundMessages
        {
            get
            {
                if (BTSTestContext.GetParam(this.TargetContextProperty) == null)
                    BTSTestContext.AddParam(this.TargetContextProperty, new List<MultipartMessageDefinition>());
                return (List<MultipartMessageDefinition>)BTSTestContext.GetParam(this.TargetContextProperty);
            }
        }

        public ExtractLiveProcessMessagesStep()
        { }

        public ExtractLiveProcessMessagesStep(string stepName)
            : base(stepName)
        {}



        //public override object GetData()
        //{
        //    // Not implemented
        //    return null;
        //}

        public override bool ExecuteStep()
        {
            this.AppendResultDescription(0, "Executing step [{0}] ({1}):", this.StepName, this.StepType);
            this.AppendResultDescription(1, "Context Properties :");
            foreach (ControlProperty prop in this.ContextProps)
            {
                this.AppendResultDescription(2, "{0}/{1} : {2}", prop.Namespace, prop.Property, prop.Value);
                if (!String.IsNullOrEmpty(prop.TargetContextProperty))
                {
                    this.AppendResultDescription(3, "(overrided by context prop '{0}')", prop.TargetContextProperty);
                }
            }
            this.AppendResultDescription(1, "XPath Values :");
            foreach (ControlField field in this.XPathProps)
            {
                this.AppendResultDescription(2, "{0} : {1}", field.XPath, field.Value);
            }

            TrackingDatabase dta = new TrackingDatabase(BTSTestContext.BizTalkDbServer,
                                                            BTSTestContext.BizTalkDTADb);
            BizTalkOperations operations = new BizTalkOperations(BTSTestContext.BizTalkDbServer, BTSTestContext.BizTalkMgmtDb);
            IEnumerable services = operations.GetServiceInstances();
            IEnumerator servicesEnum = services.GetEnumerator();

            while (servicesEnum.MoveNext())
            {
                if (servicesEnum.Current.GetType() == typeof(Microsoft.BizTalk.Operations.OrchestrationInstance))
                {
                    OrchestrationInstance tempInstance = (OrchestrationInstance)servicesEnum.Current;
                    this.AppendResultDescription(1, "Analysing instance of '{0}'", tempInstance.ServiceType);
                    this.AppendResultDescription(1, "Service status : {0}", tempInstance.InstanceStatus);

                    if (tempInstance.ServiceType.StartsWith(this.ProcessName))
                    {
                        this.AppendResultDescription(2, "[OK] Service Type match : {0}", this.ProcessName);
                        foreach (var rawMessage in tempInstance.Messages)
                            if (rawMessage.GetType() == typeof(BizTalkMessage))
                            {
                                bool currentMatch = true;
                                BizTalkMessage btsMsg = (BizTalkMessage)rawMessage;

                                string body = string.Empty;
                                using (StreamReader streamReader = new StreamReader(btsMsg.BodyPart.Data))
                                {
                                    body = streamReader.ReadToEnd();
                                }

                                foreach (ControlProperty prop in this.ContextProps)
                                {
                                    string expectedValue = prop.Value;
                                    string foundValue = (string)btsMsg.Context.Read(
                                                                                prop.Property,
                                                                                prop.Namespace);

                                    if (expectedValue != foundValue)
                                    {
                                        this.AppendResultDescription(3, "[KO] Prop '{0}/{1}' : mismatch - Expected {2}, found {3}.",
                                                       prop.Namespace,
                                                       prop.Property,
                                                       expectedValue,
                                                       foundValue);
                                        currentMatch = false;
                                        break;
                                    }
                                    else
                                    {
                                        this.AppendResultDescription(3, "[OK] Prop '{0}/{1}' : match - Expected {2}, found {3}.",
                                                        prop.Namespace,
                                                        prop.Property,
                                                        expectedValue,
                                                        foundValue);
                                    }
                                }

                                if (this.XPathProps.Count > 0 && currentMatch)
                                {
                                    
                                    XmlDocument testedMsg = new XmlDocument();
                                    testedMsg.LoadXml(body);

                                    foreach (ControlField field in this.XPathProps)
                                    {
                                        string expectedValue = field.Value;
                                        string foundValue = testedMsg.SelectSingleNode(field.XPath).InnerText;

                                        if (expectedValue != foundValue)
                                        {
                                            this.AppendResultDescription(3, "[KO] XPath '{0}' : mismatch - Expected {1}, found {2}.",
                                                                field.XPath,
                                                                expectedValue,
                                                                foundValue);
                                            currentMatch = false;
                                            break;
                                        }
                                        else
                                        {
                                            this.AppendResultDescription(3, "[OK] XPath '{0}' : match - Expected {1}, found {2}.",
                                                                field.XPath,
                                                                expectedValue,
                                                                foundValue);
                                        }
                                    }
                                }

                                if (currentMatch)
                                {
                                    // Implement copy here
                                    this.AppendResultDescription(2, "Matching message found : instance {0}, message {1}", tempInstance.ID, btsMsg.MessageID);

                                    
                                    foreach (IBaseMessage processMessage in tempInstance.Messages)
                                    {
                                        MultipartMessageDefinition tempMessage = MultipartMessageManager.GenerateFromMessage(processMessage);
                                        tempMessage.Description = "Message from instance:" + tempInstance.ID.ToString();
                                        this.FoundMessages.Add(tempMessage);
                                    }
                                    this.Result = StepResult.OK;
                                }
                            }
                    }

                }
            }

            if (this.FoundMessages.Count == 0)
            {
                this.AppendResultDescription(1, "No matching instance found!");
                this.Result = StepResult.Error;
            }

            return (this.Result == StepResult.OK);
        }
    }
}
