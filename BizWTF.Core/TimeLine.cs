using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


using System.Xml;
using System.Xml.Serialization;

using BizWTF.Core.Tests;
using BizWTF.Core.Tests.ActionSteps;
using BizWTF.Core.Tests.ActionSteps.Messaging;
using BizWTF.Core.Tests.GetSteps;
using BizWTF.Core.Tests.TestSteps;
using BizWTF.Core.Utilities;
using BizWTF.Core.Entities;

namespace BizWTF.Core
{
    public static class Constants
    {
        public static string XMLNamespace = "http://BizWTF.Core/";
    }


    [Serializable]
    public class TimeLine
    {
        protected List<TimeLineStep> _steps;

        public List<TimeLineStep> Steps
        {
            get
            {
                if (this._steps == null)
                    this._steps = new List<TimeLineStep>();
                return this._steps;
            }
            set { this._steps = value; }
        }


        
        
        public static TimeLine ImportTimeLine(string uri)
        {
            XmlSerializer tpSer = new XmlSerializer(typeof(TimeLine));
            XmlTextReader wr = new XmlTextReader(uri);
            
            return (TimeLine)tpSer.Deserialize(wr);
        }

        public static TimeLine ImportTimeLine(string assemblyName, string resourceName)
        {
            Assembly assembly = Assembly.Load(assemblyName);
            return ImportTimeLine(assembly, resourceName);
        }

        public static TimeLine ImportTimeLine(Assembly assembly, string resourceName)
        {
            XmlSerializer tpSer = new XmlSerializer(typeof(TimeLine));
            XmlTextReader wr = new XmlTextReader(assembly.GetManifestResourceStream(resourceName));

            return (TimeLine)tpSer.Deserialize(wr);
        }




        public void ExportTimeLine(string uri)
        {
            XmlSerializer tpSer = new XmlSerializer(typeof(TimeLine));
            XmlTextWriter wr = new XmlTextWriter(uri, Encoding.Unicode);

            tpSer.Serialize(wr, this);
        }

        public void Execute()
        {
            Logger.CurrentLogger.InsertBlank();
            Logger.CurrentLogger.InsertSeparator();
            Logger.CurrentLogger.Write(0, "Executing steps", System.Diagnostics.EventLogEntryType.Information);
            Logger.CurrentLogger.InsertSeparator();

            foreach (TimeLineStep step in this.Steps)
            {
                bool isOK = step.Step.ExecuteStep();
                System.Diagnostics.EventLogEntryType severity = isOK ? System.Diagnostics.EventLogEntryType.Information : System.Diagnostics.EventLogEntryType.Error;
                string status = String.Empty;
                if (isOK)
                    status = "SUCCESS";
                else
                    status = "ERROR";

                Logger.CurrentLogger.Write(0, "[{0}] : {1}", severity, step.Step.StepName, status);
                Logger.CurrentLogger.InsertBlank();
            }
            Logger.CurrentLogger.Write(0, "TimeLine processed successfully.", System.Diagnostics.EventLogEntryType.Information);
        }
    }

    [Serializable]
    //[XmlInclude(typeof(GetProcessDebugStep))]
    //[XmlInclude(typeof(ExtractLiveProcessMessagesStep))]
    //[XmlInclude(typeof(ExtractTrackedProcessMessagesStep))]
    //[XmlInclude(typeof(SynchronizeProcessDebugStep))]
    //[XmlInclude(typeof(TestProcessDebugStep))]
    //[XmlInclude(typeof(WaitStep))]
    //[XmlInclude(typeof(SubmitFileStep))]
    //[XmlInclude(typeof(ForceTrackedMessageCopyJobStep))]
    public class TimeLineStep
    {
        [XmlIgnore]
        public BaseStep Step
        {
            get { return this._step; }
            set { this._step = value; }
        }
        BaseStep _step;

        [XmlElement("Step")]
        public BaseStepSerializer XmlStep
        {
            get
            {
                if (this.Step == null)
                    return null;
                else
                {
                    return new BaseStepSerializer(this.Step);
                }
            }
            set
            {
                this._step = value.Step;
            }
        }

        public TimeLineStep() { }

        public TimeLineStep(BaseStep step)
        { this.Step = step; }
    }
}
