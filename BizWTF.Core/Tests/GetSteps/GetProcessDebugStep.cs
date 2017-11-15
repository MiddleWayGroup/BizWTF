using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.IO;
using System.Xml;
using Microsoft.BizTalk.Operations;
using Microsoft.BizTalk.Message.Interop;

using BizWTF.Core.Entities;
using BizWTF.Core.Entities.ProcessValidation;
using BizWTF.Core.BizTalk.Entities;
using BizWTF.Core.Utilities;

namespace BizWTF.Core.Tests.GetSteps
{
    [Serializable]
    public class GetProcessDebugStep : GetStep
    {
        protected string _processName;
        protected List<ControlProperty> _contextProps;
        protected List<ControlField> _xpathProps;
        protected string _sourceContextProperty;
        protected string _targetContextProperty;


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
        public string SourceContextProperty
        {
            get { return this._sourceContextProperty; }
            set { this._sourceContextProperty = value; }
        }
        public string TargetContextProperty
        {
            get { return this._targetContextProperty; }
            set { this._targetContextProperty = value; }
        }


        protected List<OrchestrationInstanceInfo> _instanceList;
        protected List<OrchestrationInstanceInfo> InstanceList
        {
            get
            {
                if (this._instanceList == null)
                    this._instanceList = new List<OrchestrationInstanceInfo>();
                return this._instanceList;
            }
        }

        public GetProcessDebugStep()
        { }

        public GetProcessDebugStep(string stepName)
            : base(stepName)
        {}



        //public override List<OrchestrationInstanceInfo> GetData<List<OrchestrationInstanceInfo>>()
        //{
        //    return this.InstanceList;
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
                    this.AppendResultDescription(2, "(overrided by context prop '{0}')", prop.TargetContextProperty);
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
                List<MatchingPartInfo> partList = MatchingPartInfo.GetMatchingPartInfo(btsDTA, dta, operations, 
                                                        this.ContextProps, 
                                                        this.XPathProps, 
                                                        this.ProcessName, 
                                                        this.SourceContextProperty
                                                        ).Distinct<MatchingPartInfo>(new MatchingPartServiceIDComparer()).ToList();

                if (partList.Count > 0)
                {
                    this.AppendResultDescription(1, "[OK] Matching service {0} instances found ", partList.Count);
                    foreach (MatchingPartInfo part in partList)
                    {
                        this.AppendResultDescription(2, "Matching service instance : {0}", part.ServiceInstanceId);

                        OrchestrationInstanceInfo currentInstance = new OrchestrationInstanceInfo(part.txtSymbol);
                        currentInstance.FlowDefinition.ProcessName = this.ProcessName;

                        var filteredMsgIds = from me in btsDTA.dta_MessageInOutEvents
                                             where me.uidServiceInstanceId == part.ServiceInstanceId
                                             orderby me.dtInsertionTimeStamp descending
                                             select new
                                             {
                                                 me.uidMessageInstanceId
                                             };
                        foreach (var msg in filteredMsgIds)
                        {
                            currentInstance.Messages.Add(operations.GetTrackedMessage(msg.uidMessageInstanceId, dta));
                        }
                        this.AppendResultDescription(3, "Found {0} attached messages", filteredMsgIds.Count());

                        this.AppendResultDescription(3, "Pre-processing DebugShapes", filteredMsgIds.Count());
                        var debugTraces = from dt in btsDTA.dta_DebugTrace
                                          where dt.uidServiceInstanceId == part.ServiceInstanceId
                                          orderby dt.dtInsertionTimeStamp ascending
                                          select new DebugShape
                                          {
                                              ShapeID = dt.vtInstructionId,
                                              StartTime = dt.dtBeginTimeStamp,
                                              FinishTime = dt.dtEndTimeStamp
                                          };
                        foreach (DebugShape trace in debugTraces)
                        {
                            string shapePath = String.Empty;
                            ShapeInfo shape = currentInstance.FlowDefinition.GetShape(trace.ShapeID, ref shapePath);
                            trace.shapeText = shape.shapeText;
                            trace.shapeType = shape.shapeType;
                            trace.ShapePath = shapePath;

                            currentInstance.Trace.TraceDetails.Add(trace);
                            this.AppendResultDescription(4, "Processed shape '{0}' ({1})", trace.shapeText, trace.ShapePath);

                        }
                        this.InstanceList.Add(currentInstance);
                    }

                    this.Result = StepResult.OK;
                }
                else
                {
                    this.AppendResultDescription(1, "No matching service instance found", EventLogEntryType.Error);
                    this.Result = StepResult.Error;
                }
            }

            BTSTestContext.AddParam(this.TargetContextProperty, this.InstanceList);

            return (this.Result == StepResult.OK);
        }


        
    }
}
