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
    public class ExtractTrackedProcessMessagesStep : GetStep
    {
        protected string _processName;
        protected List<ControlProperty> _contextProps;
        protected List<ControlField> _xpathProps;
        protected string _targetContextProperty;
        protected OrchestrationInstanceInfo _instance;



        public string TargetContextProperty
        {
            get { return this._targetContextProperty; }
            set { this._targetContextProperty = value; }
        }

        public string ProcessName
        {
            get { return this._processName; }
            set { this._processName = value; }
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


        public ExtractTrackedProcessMessagesStep()
        { }

        public ExtractTrackedProcessMessagesStep(string stepName)
            : base(stepName)
        { }



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


            using (BizTalkDTADbEntities btsDTA = new BizTalkDTADbEntities(System.Configuration.ConfigurationManager.ConnectionStrings["BizTalkDTADbEntities"].ConnectionString))
            {
                TrackingDatabase dta = new TrackingDatabase(BTSTestContext.BizTalkDbServer,
                                                            BTSTestContext.BizTalkDTADb);
                BizTalkOperations operations = new BizTalkOperations(BTSTestContext.BizTalkDbServer, BTSTestContext.BizTalkMgmtDb);
                List<MatchingPartInfo> partList = MatchingPartInfo.GetMatchingPartInfo(btsDTA, dta, operations, this.ContextProps, this.XPathProps, this.ProcessName, null);

                if (partList.Count > 0)
                {
                    List<MultipartMessageDefinition> msgs = new List<MultipartMessageDefinition>();
                    foreach (MatchingPartInfo part in partList)
                    {
                        this.AppendResultDescription(1, "[OK] Matching service instance found, messageID={0}", part.MessageInstanceId);
                        IBaseMessage trackedMsg = operations.GetTrackedMessage(part.MessageInstanceId, dta);

                        MultipartMessageDefinition tempMsg = MultipartMessageManager.GenerateFromMessage(trackedMsg);
                        tempMsg.Description = String.Format("Service : {0} - Message instance: {1}", part.ServiceName, part.MessageInstanceId);
                        msgs.Add(tempMsg);
                    }

                    BTSTestContext.AddParam(this.TargetContextProperty, msgs);
                    this.Result = StepResult.OK;
                }
                else
                {
                    this.AppendResultDescription(1, "[KO] No matching service instance found", EventLogEntryType.Error);
                    this.Result = StepResult.Error;
                }
            }

            return (this.Result == StepResult.OK);
        }
    }
}
