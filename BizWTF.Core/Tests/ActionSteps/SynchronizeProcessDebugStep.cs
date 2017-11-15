using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

using System.IO;
using System.Xml;
using Microsoft.BizTalk.Operations;
using Microsoft.BizTalk.Message.Interop;

using BizWTF.Core.Entities;
using BizWTF.Core.Entities.ProcessValidation;
using BizWTF.Core.BizTalk.Entities;
using BizWTF.Core.Utilities;

namespace BizWTF.Core.Tests.ActionSteps
{
    [Serializable]
    public class SynchronizeProcessDebugStep : GetStep
    {
        private List<Guid> _instanceList = new List<Guid>();
        private Guid _semaphoreID = Guid.NewGuid();

        protected string _processName;
        protected List<ControlProperty> _contextProps;
        protected List<ControlField> _xpathProps;
        protected string _targetContextProperty;
        protected int _pollingInterval;
        protected int _pollingCount;

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
        public int PollingInterval
        {
            get { return this._pollingInterval; }
            set { this._pollingInterval = value; }
        }
        public int PollingCount
        {
            get { return this._pollingCount; }
            set
            {
                if (value < 1)
                    this._pollingCount = 1;
                else
                    this._pollingCount = value;
            }
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


        protected OrchestrationInstanceInfo _instance;

        public SynchronizeProcessDebugStep()
        { }

        public SynchronizeProcessDebugStep(string stepName)
            : base(stepName)
        {
            //Semaphore s = new Semaphore(0, 1, this._semaphoreID.ToString());
        }

        //public override object GetData()
        //{ return null; }

        public override bool ExecuteStep()
        {
            BTSTestContext.AddParam(this.TargetContextProperty, new List<OrchestrationInstance>());

            this.AppendResultDescription(0, "Executing step [{0}] ({1}):", this.StepName, this.StepType);
            this.AppendResultDescription(1, "Context Properties :");
            foreach (ControlProperty prop in this.ContextProps)
            {
                this.AppendResultDescription(2, "{0}/{1} : {2}", prop.Namespace, prop.Property, prop.Value);
                if (!String.IsNullOrEmpty(prop.TargetContextProperty))
                {
                    this.AppendResultDescription(3, "(overridden by context prop '{0}')", prop.TargetContextProperty);
                }
            }
            this.AppendResultDescription(1, "XPath Values :");
            foreach (ControlField field in this.XPathProps)
            {
                this.AppendResultDescription(2, "{0} : {1}", field.XPath, field.Value);
            }
            this.AppendResultDescription(1, "PollingInterval : {0}", this.PollingInterval);
            this.AppendResultDescription(1, "PollingCount : {0}", this.PollingCount);

            TrackingDatabase dta = new TrackingDatabase(BTSTestContext.BizTalkDbServer,
                                                            BTSTestContext.BizTalkDTADb);
            BizTalkOperations operations = new BizTalkOperations(BTSTestContext.BizTalkDbServer, BTSTestContext.BizTalkMgmtDb);
            //bool result = true;

            

            int loopIterator = 0;
            while (((IList)BTSTestContext.GetParam(this.TargetContextProperty)).Count == 0 && loopIterator < this.PollingCount)
            {
                loopIterator++;

                //IEnumerable services = operations.GetServiceInstances();
                IEnumerator servicesEnum = operations.GetServiceInstances().GetEnumerator();

                while (servicesEnum.MoveNext())
                {
                    if (servicesEnum.Current.GetType() == typeof(Microsoft.BizTalk.Operations.OrchestrationInstance))
                    {
                        OrchestrationInstance tempInstance = (OrchestrationInstance)servicesEnum.Current;

                        Analyzer analyser = new Analyzer(tempInstance);
                        this.onProcess += analyser.ProcessInstance;
                        analyser.ProcessStopped = this.ProcessStopped;
                        analyser.AddToContext = this.addInstanceInContext;
                    }
                }
                if (this.onProcess != null)
                    this.onProcess(operations, this.ContextProps, this.XPathProps, this.ProcessName, this.AppendResultDescription);



                if (((IList)BTSTestContext.GetParam(this.TargetContextProperty)).Count == 0)
                {
                    this.AppendResultDescription(1, "[{0}] No matching service instance found, retrying in {1} milliseconds.", EventLogEntryType.Warning, this.PollingInterval);
                    System.Threading.Thread.Sleep(this.PollingInterval);
                }
                else
                    this.Result = StepResult.OK;
            }


            if (this.Result != StepResult.OK)
            {
                this.AppendResultDescription(1, "[{0}] No matching service instance found!!", EventLogEntryType.Error);
                this.Result = StepResult.Error;
            }

            return (this.Result == StepResult.OK);
        }



        private delegate void addInstanceInContextHandler(OrchestrationInstance instance);
        private void addInstanceInContext(OrchestrationInstance instance)
        {
            lock(BTSTestContext.Params)
                ((List<OrchestrationInstance>)BTSTestContext.GetParam(this.TargetContextProperty)).Add(instance);
        }


        private delegate void processInstanceHandler(BizTalkOperations operations, 
                                    List<ControlProperty> contextProps, 
                                    List<ControlField> xpathProps, 
                                    string processName,
                                    AppendResultDescriptionHandler writeLog);
        private event processInstanceHandler onProcess;

        /// <summary>
        /// Handler used when an orchestration has stopped processing, ie when it is Completed, Dehydrated, Terminated or Suspended
        /// </summary>
        /// <param name="operations">Used to perform operations on the instance</param>
        /// <param name="instance">Orchestration instance which has stopped its processing</param>
        /// <returns>True if the instance is supposed to be processing again</returns>
        public delegate bool ProcessStoppedHandler(OrchestrationInstance instance);
        public ProcessStoppedHandler ProcessStopped;

        
        private class Analyzer
        {
            OrchestrationInstance tempInstance;

            public addInstanceInContextHandler AddToContext;
            public ProcessStoppedHandler ProcessStopped;

            public Analyzer(OrchestrationInstance instance)
            {
                this.tempInstance = instance;
            }


            public void ProcessInstance(BizTalkOperations operations, 
                                        List<ControlProperty> contextProps, 
                                        List<ControlField> xpathProps,
                                        string processName,
                                        AppendResultDescriptionHandler writeLog)
            {
                OrchestrationInstance instance = null;
                StringWriter log = new StringWriter();

                log.WriteLine("  Analysing instance of '{0}'", tempInstance.ServiceType);
                if (tempInstance.ServiceType.StartsWith(processName))
                {
                    log.WriteLine("    [OK] Service Type match : {0}", processName);
                    foreach (var rawMessage in tempInstance.Messages)
                        if (rawMessage.GetType() == typeof(BizTalkMessage))
                        {
                            bool currentMatch = true;
                            BizTalkMessage btsMsg = (BizTalkMessage)rawMessage;

                            #region Test context and XPath
                            foreach (ControlProperty prop in contextProps)
                            {
                                string expectedValue = prop.Value;
                                string foundValue = (string)btsMsg.Context.Read(
                                                                            prop.Property,
                                                                            prop.Namespace);

                                if (expectedValue != foundValue)
                                {
                                    log.WriteLine("      [KO] Prop '{0}/{1}' : mismatch - Expected {2}, found {3}.",
                                                   prop.Namespace,
                                                   prop.Property,
                                                   expectedValue,
                                                   foundValue);
                                    currentMatch = false;
                                    break;
                                }
                                else
                                {
                                    log.WriteLine("      [OK] Prop '{0}/{1}' : match - Expected {2}, found {3}.",
                                                    prop.Namespace,
                                                    prop.Property,
                                                    expectedValue,
                                                    foundValue);
                                }
                            }

                            if (xpathProps.Count > 0 && currentMatch)
                            {
                                string body = string.Empty;
                                using (StreamReader streamReader = new StreamReader(btsMsg.BodyPart.Data))
                                {
                                    body = streamReader.ReadToEnd();
                                }
                                XmlDocument testedMsg = new XmlDocument();
                                testedMsg.LoadXml(body);

                                foreach (ControlField field in xpathProps)
                                {
                                    string expectedValue = field.Value;
                                    XmlNode foundValueNode = testedMsg.SelectSingleNode(field.XPath);

                                    if (foundValueNode != null)
                                    {
                                        string foundValue = testedMsg.SelectSingleNode(field.XPath).InnerText;
                                        if (expectedValue != foundValue)
                                        {
                                            log.WriteLine("      [KO] XPath '{0}' : mismatch - Expected {1}, found {2}.",
                                                                field.XPath,
                                                                expectedValue,
                                                                foundValue);
                                            currentMatch = false;
                                            break;
                                        }
                                        else
                                        {
                                            log.WriteLine("      [OK] XPath '{0}' : match - Expected {1}, found {2}.",
                                                                field.XPath,
                                                                expectedValue,
                                                                foundValue);
                                        }
                                    }
                                    else
                                    {
                                        log.WriteLine("      [KO] XPath '{0}' : returned no value.",
                                                                field.XPath);
                                        currentMatch = false;
                                        break;
                                    }
                                }
                            }
                            #endregion

                            if (currentMatch)
                            {
                                instance = tempInstance;
                                log.WriteLine("  Matching service instance found : {0}", instance.ID);
                                this.AddToContext(instance);

                                try
                                {
                                    log.WriteLine("  Waiting for instance to finish...");

                                    bool loop = true;
                                    OrchestrationInstance waitInstance = null;
                                    while (loop)
                                    {
                                        while (instance.InstanceStatus != InstanceStatus.Completed
                                               && instance.InstanceStatus != InstanceStatus.Dehydrated
                                               && instance.InstanceStatus != InstanceStatus.Suspended
                                               && instance.InstanceStatus != InstanceStatus.SuspendedAll
                                               && instance.InstanceStatus != InstanceStatus.SuspendedNotResumable
                                               && instance.InstanceStatus != InstanceStatus.Terminated)
                                        {
                                            System.Threading.Thread.Sleep(5000);

                                            waitInstance = (OrchestrationInstance)operations.GetServiceInstance(instance.ID);
                                            instance = waitInstance;
                                        }

                                        loop = this.ProcessStopped(instance);
                                        if (loop)
                                        {
                                            System.Threading.Thread.Sleep(2000);
                                            waitInstance = (OrchestrationInstance)operations.GetServiceInstance(instance.ID);
                                            instance = waitInstance;
                                        }
                                    }
                                }
                                catch { }

                                writeLog(0, log.ToString());

                                break;
                            }
                            else
                                writeLog(0, log.ToString());
                        }
                }
            }
        }
    }
}
